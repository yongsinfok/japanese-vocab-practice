namespace JapanesePracticeApp.Core.Models;

/// <summary>
/// Represents the user's progress and performance statistics.
/// </summary>
public class UserProgress
{
    /// <summary>
    /// Gets or sets the total number of questions answered.
    /// </summary>
    public int TotalQuestionsAnswered { get; set; }

    /// <summary>
    /// Gets or sets the number of questions answered correctly.
    /// </summary>
    public int CorrectAnswers { get; set; }

    /// <summary>
    /// Gets or sets the number of questions answered incorrectly.
    /// </summary>
    public int IncorrectAnswers { get; set; }

    /// <summary>
    /// Gets or sets the list of question IDs that have been answered.
    /// </summary>
    public List<Guid> AnsweredQuestionIds { get; set; } = new();

    /// <summary>
    /// Gets or sets the timestamp of the last activity.
    /// </summary>
    public DateTime LastActivityAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets the accuracy rate as a percentage.
    /// </summary>
    public double AccuracyRate => TotalQuestionsAnswered > 0
        ? (double)CorrectAnswers / TotalQuestionsAnswered * 100
        : 0;
}
