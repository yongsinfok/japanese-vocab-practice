using System.Windows.Input;
using JapanesePracticeApp.Core.Models;
using JapanesePracticeApp.Core.Services;

namespace JapanesePracticeApp.UI.ViewModels;

/// <summary>
/// View model for the main window.
/// </summary>
public class MainViewModel : ViewModelBase
{
    private readonly QuestionGenerationService _questionService;
    private GrammarQuestion? _currentQuestion;
    private string _userAnswer = string.Empty;
    private string _feedbackMessage = string.Empty;
    private bool _isLoading;
    private bool _isInitialized;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainViewModel"/> class.
    /// </summary>
    /// <param name="questionService">The question generation service.</param>
    public MainViewModel(QuestionGenerationService questionService)
    {
        _questionService = questionService ?? throw new ArgumentNullException(nameof(questionService));

        LoadQuestionCommand = new RelayCommand(async () => await LoadNewQuestionAsync(), () => _isInitialized && !_isLoading);
        SubmitAnswerCommand = new RelayCommand(SubmitAnswer, () => !string.IsNullOrWhiteSpace(_userAnswer) && !_isLoading);
    }

    /// <summary>
    /// Gets the current question being displayed.
    /// </summary>
    public GrammarQuestion? CurrentQuestion
    {
        get => _currentQuestion;
        private set => SetProperty(ref _currentQuestion, value);
    }

    /// <summary>
    /// Gets or sets the user's answer input.
    /// </summary>
    public string UserAnswer
    {
        get => _userAnswer;
        set
        {
            if (SetProperty(ref _userAnswer, value))
            {
                ((RelayCommand)SubmitAnswerCommand).RaiseCanExecuteChanged();
            }
        }
    }

    /// <summary>
    /// Gets the feedback message to display to the user.
    /// </summary>
    public string FeedbackMessage
    {
        get => _feedbackMessage;
        private set => SetProperty(ref _feedbackMessage, value);
    }

    /// <summary>
    /// Gets a value indicating whether a question is being loaded.
    /// </summary>
    public bool IsLoading
    {
        get => _isLoading;
        private set
        {
            if (SetProperty(ref _isLoading, value))
            {
                ((RelayCommand)LoadQuestionCommand).RaiseCanExecuteChanged();
                ((RelayCommand)SubmitAnswerCommand).RaiseCanExecuteChanged();
            }
        }
    }

    /// <summary>
    /// Gets the command to load a new question.
    /// </summary>
    public ICommand LoadQuestionCommand { get; }

    /// <summary>
    /// Gets the command to submit the user's answer.
    /// </summary>
    public ICommand SubmitAnswerCommand { get; }

    /// <summary>
    /// Initializes the view model by setting up the question service.
    /// </summary>
    /// <param name="modelPath">The path to the LLM model file.</param>
    public async Task InitializeAsync(string modelPath)
    {
        try
        {
            IsLoading = true;
            FeedbackMessage = "Initializing AI model...";

            await _questionService.InitializeAsync(modelPath);
            _isInitialized = true;

            FeedbackMessage = "Ready! Click 'Load Question' to start.";
            ((RelayCommand)LoadQuestionCommand).RaiseCanExecuteChanged();
        }
        catch (Exception ex)
        {
            FeedbackMessage = $"Error initializing: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// Loads a new grammar question.
    /// </summary>
    private async Task LoadNewQuestionAsync()
    {
        try
        {
            IsLoading = true;
            FeedbackMessage = "Generating question...";
            UserAnswer = string.Empty;

            CurrentQuestion = await _questionService.GenerateQuestionAsync();
            FeedbackMessage = string.Empty;
        }
        catch (Exception ex)
        {
            FeedbackMessage = $"Error loading question: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// Submits and checks the user's answer.
    /// </summary>
    private void SubmitAnswer()
    {
        if (CurrentQuestion == null)
        {
            return;
        }

        var isCorrect = string.Equals(
            UserAnswer.Trim(),
            CurrentQuestion.CorrectAnswer.Trim(),
            StringComparison.OrdinalIgnoreCase);

        if (isCorrect)
        {
            FeedbackMessage = $"Correct! {CurrentQuestion.Explanation ?? ""}";
        }
        else
        {
            FeedbackMessage = $"Incorrect. The correct answer is: {CurrentQuestion.CorrectAnswer}. {CurrentQuestion.Explanation ?? ""}";
        }
    }
}

/// <summary>
/// Simple implementation of ICommand for use in view models.
/// </summary>
public class RelayCommand : ICommand
{
    private readonly Action _execute;
    private readonly Func<bool>? _canExecute;

    public RelayCommand(Action execute, Func<bool>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

    public void Execute(object? parameter) => _execute();

    public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}
