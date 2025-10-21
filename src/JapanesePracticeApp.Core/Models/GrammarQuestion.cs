namespace JapanesePracticeApp.Core.Models;

/// <summary>
/// Represents a fill-in-the-blank Japanese grammar question.
/// </summary>
public class GrammarQuestion
{
    /// <summary>
    /// Gets or sets the unique identifier for this question.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the question text with a blank to be filled.
    /// Example: "私は毎日学校___行きます。"
    /// </summary>
    public string QuestionText { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the correct answer for the blank.
    /// Example: "に"
    /// </summary>
    public string CorrectAnswer { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets an optional explanation or hint for the question.
    /// </summary>
    public string? Explanation { get; set; }

    /// <summary>
    /// Gets or sets the difficulty level of the question.
    /// </summary>
    public string DifficultyLevel { get; set; } = "Beginner";

    /// <summary>
    /// Gets or sets the timestamp when this question was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
