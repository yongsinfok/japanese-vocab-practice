using JapanesePracticeApp.Core.Models;
using JapanesePracticeApp.Core.Services;
using LLama;
using LLama.Common;
using LLama.Native;
using System.Runtime.InteropServices;
using System.Text;

namespace JapanesePracticeApp.Infrastructure.Services;

/// <summary>
/// Implementation of ILlmService using LLamaSharp library.
/// </summary>
public class LlamaSharpService : ILlmService
{
    private LLamaWeights? _model;
    private LLamaContext? _context;
    private bool _isInitialized;

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern int GetShortPathName(string lpszLongPath, StringBuilder lpszShortPath, int cchBuffer);

    /// <inheritdoc/>
    public bool IsInitialized => _isInitialized;

    /// <summary>
    /// Converts a long path with Unicode characters to Windows short path (8.3 format)
    /// </summary>
    private static string GetShortPath(string longPath)
    {
        var shortPath = new StringBuilder(255);
        var result = GetShortPathName(longPath, shortPath, shortPath.Capacity);
        if (result == 0)
        {
            return longPath; // Fallback to long path if conversion fails
        }
        return shortPath.ToString();
    }

    /// <inheritdoc/>
    public async Task InitializeAsync(string modelPath)
    {
        if (_isInitialized)
        {
            return;
        }

        if (!File.Exists(modelPath))
        {
            throw new FileNotFoundException($"Model file not found: {modelPath}");
        }

        // Convert to short path to handle Unicode characters in path
        var shortModelPath = GetShortPath(modelPath);

        await Task.Run(() =>
        {
            try
            {
                // Configure native library loading for better diagnostics
                NativeLibraryConfig
                    .All
                    .WithLogCallback((level, message) =>
                    {
                        Console.WriteLine($"[{level}] {message}");
                    });

                var parameters = new ModelParams(shortModelPath)
                {
                    ContextSize = 512, // Reduced context size to lower memory requirements
                    GpuLayerCount = 0 // CPU only, set higher for GPU acceleration
                };

                _model = LLamaWeights.LoadFromFile(parameters);
                _context = _model.CreateContext(parameters);
                _isInitialized = true;
            }
            catch (DllNotFoundException dllEx)
            {
                throw new InvalidOperationException($"Native library not found. This usually means Visual C++ Runtime is missing. Install VC++ 2015-2022 Redistributable (x64). Details: {dllEx.Message}", dllEx);
            }
            catch (BadImageFormatException imgEx)
            {
                throw new InvalidOperationException($"Native library architecture mismatch. Make sure you're running as x64. Details: {imgEx.Message}", imgEx);
            }
            catch (Exception ex)
            {
                var innerMsg = ex.InnerException != null ? $" Inner: {ex.InnerException.Message}" : "";
                throw new InvalidOperationException($"Failed to load model '{modelPath}'. Error: {ex.GetType().Name} - {ex.Message}{innerMsg}", ex);
            }
        });
    }

    /// <inheritdoc/>
    public async Task<GrammarQuestion> GenerateQuestionAsync(string difficultyLevel = "Beginner", CancellationToken cancellationToken = default)
    {
        if (!_isInitialized || _context == null || _model == null)
        {
            throw new InvalidOperationException("LLM service is not initialized.");
        }

        var prompt = $@"Generate a Japanese grammar fill-in-the-blank question at {difficultyLevel} level.

Format your response as follows:
QUESTION: [Japanese sentence with ___ for the blank]
ANSWER: [The correct word/particle to fill in the blank]
EXPLANATION: [Brief explanation in English]

Example:
QUESTION: 私は毎日学校___行きます。
ANSWER: に
EXPLANATION: The particle 'に' indicates the direction of movement towards a destination.

Now generate a new question:";

        // Use StatelessExecutor instead of InteractiveExecutor to avoid state management issues
        var executor = new StatelessExecutor(_model, _context.Params);

        var inferenceParams = new InferenceParams
        {
            MaxTokens = 300,  // Limit the number of tokens to generate
            AntiPrompts = new List<string> { "Now generate" }  // Stop at certain phrases
        };

        var responseBuilder = new System.Text.StringBuilder();

        await foreach (var text in executor.InferAsync(prompt, inferenceParams, cancellationToken))
        {
            responseBuilder.Append(text);
            // Limit the response length to prevent infinite generation
            if (responseBuilder.Length > 1000)
                break;
        }

        var response = responseBuilder.ToString();
        return ParseQuestionFromResponse(response, difficultyLevel);
    }

    /// <summary>
    /// Parses the LLM response and extracts question components.
    /// </summary>
    private GrammarQuestion ParseQuestionFromResponse(string response, string difficultyLevel)
    {
        var question = new GrammarQuestion
        {
            Id = Guid.NewGuid(),
            DifficultyLevel = difficultyLevel,
            CreatedAt = DateTime.UtcNow
        };

        try
        {
            // Extract question text
            var questionMatch = System.Text.RegularExpressions.Regex.Match(response, @"QUESTION:\s*(.+?)(?:\n|$)", System.Text.RegularExpressions.RegexOptions.Singleline);
            if (questionMatch.Success)
            {
                question.QuestionText = questionMatch.Groups[1].Value.Trim();
            }

            // Extract answer
            var answerMatch = System.Text.RegularExpressions.Regex.Match(response, @"ANSWER:\s*(.+?)(?:\n|$)", System.Text.RegularExpressions.RegexOptions.Singleline);
            if (answerMatch.Success)
            {
                question.CorrectAnswer = answerMatch.Groups[1].Value.Trim();
            }

            // Extract explanation
            var explanationMatch = System.Text.RegularExpressions.Regex.Match(response, @"EXPLANATION:\s*(.+?)(?:\n\n|$)", System.Text.RegularExpressions.RegexOptions.Singleline);
            if (explanationMatch.Success)
            {
                question.Explanation = explanationMatch.Groups[1].Value.Trim();
            }

            // Fallback: if parsing failed, create a default question
            if (string.IsNullOrWhiteSpace(question.QuestionText))
            {
                question.QuestionText = "私は毎日学校___行きます。";
                question.CorrectAnswer = "に";
                question.Explanation = "The particle 'に' indicates the direction of movement towards a destination.";
            }
        }
        catch
        {
            // On error, provide a default question
            question.QuestionText = "私は毎日学校___行きます。";
            question.CorrectAnswer = "に";
            question.Explanation = "The particle 'に' indicates the direction of movement towards a destination.";
        }

        return question;
    }

    /// <summary>
    /// Disposes of the LLM resources.
    /// </summary>
    public void Dispose()
    {
        _context?.Dispose();
        _model?.Dispose();
        _isInitialized = false;
    }
}
