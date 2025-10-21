using JapanesePracticeApp.Core.Models;

namespace JapanesePracticeApp.Core.Services;

/// <summary>
/// Service responsible for generating grammar questions using an LLM.
/// </summary>
public class QuestionGenerationService
{
    private readonly ILlmService _llmService;

    /// <summary>
    /// Initializes a new instance of the <see cref="QuestionGenerationService"/> class.
    /// </summary>
    /// <param name="llmService">The LLM service to use for question generation.</param>
    public QuestionGenerationService(ILlmService llmService)
    {
        _llmService = llmService ?? throw new ArgumentNullException(nameof(llmService));
    }

    /// <summary>
    /// Generates a new grammar question at the specified difficulty level.
    /// </summary>
    /// <param name="difficultyLevel">The desired difficulty level (e.g., "Beginner", "Intermediate", "Advanced").</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A newly generated grammar question.</returns>
    public async Task<GrammarQuestion> GenerateQuestionAsync(string difficultyLevel = "Beginner", CancellationToken cancellationToken = default)
    {
        if (!_llmService.IsInitialized)
        {
            throw new InvalidOperationException("LLM service is not initialized. Call InitializeAsync first.");
        }

        return await _llmService.GenerateQuestionAsync(difficultyLevel, cancellationToken);
    }

    /// <summary>
    /// Initializes the question generation service by loading the LLM model.
    /// </summary>
    /// <param name="modelPath">The file path to the GGUF model file.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task InitializeAsync(string modelPath)
    {
        await _llmService.InitializeAsync(modelPath);
    }

    /// <summary>
    /// Gets a value indicating whether the service is ready to generate questions.
    /// </summary>
    public bool IsReady => _llmService.IsInitialized;
}
