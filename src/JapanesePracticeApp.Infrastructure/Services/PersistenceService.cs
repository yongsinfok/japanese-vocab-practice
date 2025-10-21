using System.Text.Json;
using JapanesePracticeApp.Core.Models;

namespace JapanesePracticeApp.Infrastructure.Services;

/// <summary>
/// Service for persisting and loading data to/from JSON files.
/// </summary>
public class PersistenceService
{
    private readonly string _dataDirectory;
    private readonly JsonSerializerOptions _jsonOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="PersistenceService"/> class.
    /// </summary>
    /// <param name="dataDirectory">The directory where data files will be stored.</param>
    public PersistenceService(string? dataDirectory = null)
    {
        _dataDirectory = dataDirectory ?? Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "JapanesePracticeApp");

        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        // Ensure data directory exists
        Directory.CreateDirectory(_dataDirectory);
    }

    /// <summary>
    /// Saves user progress to a JSON file.
    /// </summary>
    /// <param name="progress">The user progress to save.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SaveProgressAsync(UserProgress progress)
    {
        var filePath = Path.Combine(_dataDirectory, "progress.json");
        var json = JsonSerializer.Serialize(progress, _jsonOptions);
        await File.WriteAllTextAsync(filePath, json);
    }

    /// <summary>
    /// Loads user progress from a JSON file.
    /// </summary>
    /// <returns>The loaded user progress, or a new instance if the file doesn't exist.</returns>
    public async Task<UserProgress> LoadProgressAsync()
    {
        var filePath = Path.Combine(_dataDirectory, "progress.json");

        if (!File.Exists(filePath))
        {
            return new UserProgress();
        }

        try
        {
            var json = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<UserProgress>(json, _jsonOptions) ?? new UserProgress();
        }
        catch
        {
            // If deserialization fails, return a new instance
            return new UserProgress();
        }
    }

    /// <summary>
    /// Saves a list of grammar questions to a JSON file.
    /// </summary>
    /// <param name="questions">The list of questions to save.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SaveQuestionsAsync(List<GrammarQuestion> questions)
    {
        var filePath = Path.Combine(_dataDirectory, "questions.json");
        var json = JsonSerializer.Serialize(questions, _jsonOptions);
        await File.WriteAllTextAsync(filePath, json);
    }

    /// <summary>
    /// Loads a list of grammar questions from a JSON file.
    /// </summary>
    /// <returns>The loaded list of questions, or an empty list if the file doesn't exist.</returns>
    public async Task<List<GrammarQuestion>> LoadQuestionsAsync()
    {
        var filePath = Path.Combine(_dataDirectory, "questions.json");

        if (!File.Exists(filePath))
        {
            return new List<GrammarQuestion>();
        }

        try
        {
            var json = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<List<GrammarQuestion>>(json, _jsonOptions) ?? new List<GrammarQuestion>();
        }
        catch
        {
            // If deserialization fails, return an empty list
            return new List<GrammarQuestion>();
        }
    }
}
