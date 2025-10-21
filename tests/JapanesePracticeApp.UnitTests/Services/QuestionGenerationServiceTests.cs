using JapanesePracticeApp.Core.Models;
using JapanesePracticeApp.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace JapanesePracticeApp.UnitTests.Services;

[TestClass]
public class QuestionGenerationServiceTests
{
    private Mock<ILlmService> _mockLlmService = null!;
    private QuestionGenerationService _questionService = null!;

    [TestInitialize]
    public void Setup()
    {
        _mockLlmService = new Mock<ILlmService>();
        _questionService = new QuestionGenerationService(_mockLlmService.Object);
    }

    [TestMethod]
    public void Constructor_WithNullLlmService_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.ThrowsException<ArgumentNullException>(() => new QuestionGenerationService(null!));
    }

    [TestMethod]
    public async Task GenerateQuestionAsync_WhenLlmServiceNotInitialized_ThrowsInvalidOperationException()
    {
        // Arrange
        _mockLlmService.Setup(x => x.IsInitialized).Returns(false);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<InvalidOperationException>(
            () => _questionService.GenerateQuestionAsync());
    }

    [TestMethod]
    public async Task GenerateQuestionAsync_WhenLlmServiceInitialized_CallsLlmService()
    {
        // Arrange
        var expectedQuestion = new GrammarQuestion
        {
            Id = Guid.NewGuid(),
            QuestionText = "私は毎日学校___行きます。",
            CorrectAnswer = "に",
            DifficultyLevel = "Beginner"
        };

        _mockLlmService.Setup(x => x.IsInitialized).Returns(true);
        _mockLlmService.Setup(x => x.GenerateQuestionAsync("Beginner", It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedQuestion);

        // Act
        var result = await _questionService.GenerateQuestionAsync("Beginner");

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expectedQuestion.Id, result.Id);
        Assert.AreEqual(expectedQuestion.QuestionText, result.QuestionText);
        _mockLlmService.Verify(x => x.GenerateQuestionAsync("Beginner", It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task InitializeAsync_CallsLlmServiceInitialize()
    {
        // Arrange
        var modelPath = "path/to/model.gguf";

        // Act
        await _questionService.InitializeAsync(modelPath);

        // Assert
        _mockLlmService.Verify(x => x.InitializeAsync(modelPath), Times.Once);
    }

    [TestMethod]
    public void IsReady_ReturnsLlmServiceIsInitialized()
    {
        // Arrange
        _mockLlmService.Setup(x => x.IsInitialized).Returns(true);

        // Act & Assert
        Assert.IsTrue(_questionService.IsReady);
        _mockLlmService.Setup(x => x.IsInitialized).Returns(false);
        Assert.IsFalse(_questionService.IsReady);
    }
}
