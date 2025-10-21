using JapanesePracticeApp.Core.Models;
using JapanesePracticeApp.Core.Services;
using LLama;
using LLama.Common;

namespace JapanesePracticeApp.Infrastructure.Services;

/// <summary>
/// Implementation of ILlmService using LLamaSharp library.
/// </summary>
public class LlamaSharpService : ILlmService
{
    private LLamaWeights? _model;
    private LLamaContext? _context;
    private bool _isInitialized;

    /// <inheritdoc/>
    public bool IsInitialized => _isInitialized;

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

        await Task.Run(() =>
        {
            var parameters = new ModelParams(modelPath)
            {
                ContextSize = 2048,
                GpuLayerCount = 0 // CPU only, set higher for GPU acceleration
            };

            _model = LLamaWeights.LoadFromFile(parameters);
            _context = _model.CreateContext(parameters);
            _isInitialized = true;
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

        var executor = new InteractiveExecutor(_context);
        var inferenceParams = new InferenceParams();

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
