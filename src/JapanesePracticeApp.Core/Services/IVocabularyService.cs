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
    /// Gets a random vocabulary item from a specific week filtered by difficulty level.
    /// </summary>
    /// <param name="weekNumber">The week number to get a random item from.</param>
    /// <param name="difficultyLevel">The difficulty level to filter by. Use "All" or empty string for no filtering.</param>
    /// <returns>A random vocabulary item matching the criteria.</returns>
    Task<VocabularyItem?> GetRandomVocabularyItemAsync(int weekNumber, string? difficultyLevel);

    /// <summary>
    /// Gets all available week numbers.
    /// </summary>
    /// <returns>A list of available week numbers.</returns>
    Task<List<int>> GetAvailableWeeksAsync();

    /// <summary>
    /// Saves vocabulary items for a specific week to a JSON file.
    /// </summary>
    /// <param name="weekNumber">The week number to save.</param>
    /// <param name="items">The vocabulary items to save.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SaveVocabularyByWeekAsync(int weekNumber, List<VocabularyItem> items);

    /// <summary>
    /// Deletes a vocabulary item from a specific week.
    /// </summary>
    /// <param name="weekNumber">The week number.</param>
    /// <param name="item">The vocabulary item to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteVocabularyItemAsync(int weekNumber, VocabularyItem item);

    /// <summary>
    /// Creates a new week with empty vocabulary list.
    /// </summary>
    /// <param name="weekNumber">The week number to create.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CreateNewWeekAsync(int weekNumber);
}
