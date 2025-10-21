using JapanesePracticeApp.Core.Models;

namespace JapanesePracticeApp.Core.Services;

/// <summary>
/// Interface for Language Learning Model service that generates grammar questions.
/// </summary>
public interface ILlmService
{
    /// <summary>
    /// Generates a new fill-in-the-blank grammar question using the LLM.
    /// </summary>
    /// <param name="difficultyLevel">The desired difficulty level for the question.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A newly generated grammar question.</returns>
    Task<GrammarQuestion> GenerateQuestionAsync(string difficultyLevel = "Beginner", CancellationToken cancellationToken = default);

    /// <summary>
    /// Initializes the LLM service and loads the model.
    /// </summary>
    /// <param name="modelPath">The file path to the GGUF model file.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task InitializeAsync(string modelPath);

    /// <summary>
    /// Gets a value indicating whether the LLM service is ready to generate questions.
    /// </summary>
    bool IsInitialized { get; }
}
