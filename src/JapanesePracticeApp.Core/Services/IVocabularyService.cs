using JapanesePracticeApp.Core.Models;

namespace JapanesePracticeApp.Core.Services;

/// <summary>
/// Interface for vocabulary service that loads Chinese-Japanese vocabulary items.
/// </summary>
public interface IVocabularyService
{
    /// <summary>
    /// Loads all vocabulary items from a specific week.
    /// </summary>
    /// <param name="weekNumber">The week number to load.</param>
    /// <returns>A list of vocabulary items for the specified week.</returns>
    Task<List<VocabularyItem>> LoadVocabularyByWeekAsync(int weekNumber);

    /// <summary>
    /// Loads all available vocabulary items.
    /// </summary>
    /// <returns>A list of all vocabulary items.</returns>
    Task<List<VocabularyItem>> LoadAllVocabularyAsync();

    /// <summary>
    /// Gets a random vocabulary item from a specific week.
    /// </summary>
    /// <param name="weekNumber">The week number to get a random item from.</param>
    /// <returns>A random vocabulary item.</returns>
    Task<VocabularyItem?> GetRandomVocabularyItemAsync(int weekNumber);

    /// <summary>
    /// Gets all available week numbers.
    /// </summary>
    /// <returns>A list of available week numbers.</returns>
    Task<List<int>> GetAvailableWeeksAsync();
}
