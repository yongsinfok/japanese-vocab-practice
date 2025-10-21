using System.Text.Json;
using JapanesePracticeApp.Core.Models;
using JapanesePracticeApp.Core.Services;

namespace JapanesePracticeApp.Infrastructure.Services;

/// <summary>
/// Service for loading Chinese-Japanese vocabulary items from JSON files.
/// </summary>
public class VocabularyService : IVocabularyService
{
    private readonly string _dataDirectory;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly Random _random;

    /// <summary>
    /// Initializes a new instance of the <see cref="VocabularyService"/> class.
    /// </summary>
    /// <param name="dataDirectory">The directory where vocabulary JSON files are stored.</param>
    public VocabularyService(string? dataDirectory = null)
    {
        // Default to the Data folder in the application directory
        _dataDirectory = dataDirectory ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip
        };

        _random = new Random();
    }

    /// <inheritdoc/>
    public async Task<List<VocabularyItem>> LoadVocabularyByWeekAsync(int weekNumber)
    {
        var filePath = Path.Combine(_dataDirectory, $"week{weekNumber}.json");

        if (!File.Exists(filePath))
        {
            return new List<VocabularyItem>();
        }

        try
        {
            var json = await File.ReadAllTextAsync(filePath);
            var items = JsonSerializer.Deserialize<List<VocabularyItem>>(json, _jsonOptions);
            return items ?? new List<VocabularyItem>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading vocabulary from {filePath}: {ex.Message}");
            return new List<VocabularyItem>();
        }
    }

    /// <inheritdoc/>
    public async Task<List<VocabularyItem>> LoadAllVocabularyAsync()
    {
        var allItems = new List<VocabularyItem>();

        if (!Directory.Exists(_dataDirectory))
        {
            return allItems;
        }

        // Find all week*.json files
        var weekFiles = Directory.GetFiles(_dataDirectory, "week*.json");

        foreach (var file in weekFiles)
        {
            try
            {
                var json = await File.ReadAllTextAsync(file);
                var items = JsonSerializer.Deserialize<List<VocabularyItem>>(json, _jsonOptions);
                if (items != null)
                {
                    allItems.AddRange(items);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading vocabulary from {file}: {ex.Message}");
            }
        }

        return allItems;
    }

    /// <inheritdoc/>
    public async Task<VocabularyItem?> GetRandomVocabularyItemAsync(int weekNumber)
    {
        var items = await LoadVocabularyByWeekAsync(weekNumber);

        if (items.Count == 0)
        {
            return null;
        }

        var randomIndex = _random.Next(items.Count);
        return items[randomIndex];
    }

    /// <inheritdoc/>
    public Task<List<int>> GetAvailableWeeksAsync()
    {
        var weeks = new List<int>();

        if (!Directory.Exists(_dataDirectory))
        {
            return Task.FromResult(weeks);
        }

        var weekFiles = Directory.GetFiles(_dataDirectory, "week*.json");

        foreach (var file in weekFiles)
        {
            var fileName = Path.GetFileNameWithoutExtension(file);
            // Extract week number from filename like "week1", "week2", etc.
            if (fileName.StartsWith("week") && int.TryParse(fileName.Substring(4), out var weekNumber))
            {
                weeks.Add(weekNumber);
            }
        }

        weeks.Sort();
        return Task.FromResult(weeks);
    }
}
