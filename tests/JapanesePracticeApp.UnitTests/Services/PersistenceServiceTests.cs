using JapanesePracticeApp.Core.Models;
using JapanesePracticeApp.Infrastructure.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JapanesePracticeApp.UnitTests.Services;

[TestClass]
public class PersistenceServiceTests
{
    private string _tempDirectory = null!;
    private PersistenceService _persistenceService = null!;

    [TestInitialize]
    public void Setup()
    {
        _tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_tempDirectory);
        _persistenceService = new PersistenceService(_tempDirectory);
    }

    [TestCleanup]
    public void Cleanup()
    {
        if (Directory.Exists(_tempDirectory))
        {
            Directory.Delete(_tempDirectory, true);
        }
    }

    [TestMethod]
    public async Task SaveProgressAsync_ThenLoadProgressAsync_ReturnsSameProgress()
    {
        // Arrange
        var progress = new UserProgress
        {
            TotalQuestionsAnswered = 10,
            CorrectAnswers = 7,
            IncorrectAnswers = 3,
            LastActivityAt = DateTime.UtcNow
        };

        // Act
        await _persistenceService.SaveProgressAsync(progress);
        var loadedProgress = await _persistenceService.LoadProgressAsync();

        // Assert
        Assert.AreEqual(progress.TotalQuestionsAnswered, loadedProgress.TotalQuestionsAnswered);
        Assert.AreEqual(progress.CorrectAnswers, loadedProgress.CorrectAnswers);
        Assert.AreEqual(progress.IncorrectAnswers, loadedProgress.IncorrectAnswers);
        Assert.AreEqual(progress.AccuracyRate, loadedProgress.AccuracyRate);
    }

    [TestMethod]
    public async Task LoadProgressAsync_WhenNoFileExists_ReturnsNewUserProgress()
    {
        // Act
        var progress = await _persistenceService.LoadProgressAsync();

        // Assert
        Assert.AreEqual(0, progress.TotalQuestionsAnswered);
        Assert.AreEqual(0, progress.CorrectAnswers);
        Assert.AreEqual(0, progress.IncorrectAnswers);
        Assert.AreEqual(0, progress.AccuracyRate);
    }

    [TestMethod]
    public async Task SaveQuestionsAsync_ThenLoadQuestionsAsync_ReturnsSameQuestions()
    {
        // Arrange
        var questions = new List<GrammarQuestion>
        {
            new GrammarQuestion
            {
                Id = Guid.NewGuid(),
                QuestionText = "私は毎日学校___行きます。",
                CorrectAnswer = "に",
                DifficultyLevel = "Beginner"
            },
            new GrammarQuestion
            {
                Id = Guid.NewGuid(),
                QuestionText = "彼は本___読みます。",
                CorrectAnswer = "を",
                DifficultyLevel = "Intermediate"
            }
        };

        // Act
        await _persistenceService.SaveQuestionsAsync(questions);
        var loadedQuestions = await _persistenceService.LoadQuestionsAsync();

        // Assert
        Assert.AreEqual(2, loadedQuestions.Count);
        Assert.AreEqual(questions[0].QuestionText, loadedQuestions[0].QuestionText);
        Assert.AreEqual(questions[1].QuestionText, loadedQuestions[1].QuestionText);
    }

    [TestMethod]
    public async Task LoadQuestionsAsync_WhenNoFileExists_ReturnsEmptyList()
    {
        // Act
        var questions = await _persistenceService.LoadQuestionsAsync();

        // Assert
        Assert.AreEqual(0, questions.Count);
    }

    [TestMethod]
    public void UserProgress_AccuracyRate_CalculatedCorrectly()
    {
        // Arrange
        var progress = new UserProgress
        {
            TotalQuestionsAnswered = 10,
            CorrectAnswers = 7,
            IncorrectAnswers = 3
        };

        // Act
        var accuracyRate = progress.AccuracyRate;

        // Assert
        Assert.AreEqual(70.0, accuracyRate);
    }

    [TestMethod]
    public void UserProgress_AccuracyRate_WhenNoQuestions_ReturnsZero()
    {
        // Arrange
        var progress = new UserProgress();

        // Act
        var accuracyRate = progress.AccuracyRate;

        // Assert
        Assert.AreEqual(0, accuracyRate);
    }
}
