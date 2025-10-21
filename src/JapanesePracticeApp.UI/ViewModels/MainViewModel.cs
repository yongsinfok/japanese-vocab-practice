using System.Windows.Input;
using JapanesePracticeApp.Core.Models;
using JapanesePracticeApp.Core.Services;

namespace JapanesePracticeApp.UI.ViewModels;

/// <summary>
/// Practice mode enum - Only Vocabulary mode supported.
/// </summary>
public enum PracticeMode
{
    Vocabulary
}

/// <summary>
/// View model for the main window.
/// </summary>
public class MainViewModel : ViewModelBase
{
    private readonly IVocabularyService _vocabularyService;
    private VocabularyItem? _currentVocabulary;
    private string _userAnswer = string.Empty;
    private string _feedbackMessage = string.Empty;
    private bool _isLoading;
    private PracticeMode _practiceMode = PracticeMode.Vocabulary;
    private int _selectedWeek = 1;
    private List<int> _availableWeeks = new List<int>();
    private string _selectedDifficulty = "All";
    private readonly List<string> _difficultyLevels = new List<string> { "All", "Beginner", "Intermediate", "Advanced" };

    /// <summary>
    /// Initializes a new instance of the <see cref="MainViewModel"/> class.
    /// </summary>
    /// <param name="vocabularyService">The vocabulary service.</param>
    public MainViewModel(IVocabularyService vocabularyService)
    {
        _vocabularyService = vocabularyService ?? throw new ArgumentNullException(nameof(vocabularyService));

        LoadQuestionCommand = new RelayCommand(async () => await LoadNewQuestionAsync(), () => !_isLoading);
        SubmitAnswerCommand = new RelayCommand(SubmitAnswer, () => !string.IsNullOrWhiteSpace(_userAnswer) && !_isLoading);
    }

    /// <summary>
    /// Initializes the view model by loading available weeks and loads the first question.
    /// </summary>
    public async Task InitializeAsync()
    {
        try
        {
            IsLoading = true;
            FeedbackMessage = "Loading vocabulary data...";

            AvailableWeeks = await _vocabularyService.GetAvailableWeeksAsync();
            if (AvailableWeeks.Count > 0)
            {
                // Set the field directly to avoid triggering the property setter
                _selectedWeek = AvailableWeeks[0];
                OnPropertyChanged(nameof(SelectedWeek));

                // Automatically load the first question on startup
                await LoadNewQuestionAsync();
            }
            else
            {
                FeedbackMessage = "No vocabulary data available.";
            }
        }
        catch (Exception ex)
        {
            FeedbackMessage = $"Error loading vocabulary: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>
    /// Gets the current vocabulary item being displayed.
    /// </summary>
    public VocabularyItem? CurrentVocabulary
    {
        get => _currentVocabulary;
        private set => SetProperty(ref _currentVocabulary, value);
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
    /// Gets or sets the current practice mode.
    /// </summary>
    public PracticeMode PracticeMode
    {
        get => _practiceMode;
        private set => SetProperty(ref _practiceMode, value);
    }

    /// <summary>
    /// Gets or sets the selected week number for vocabulary practice.
    /// </summary>
    public int SelectedWeek
    {
        get => _selectedWeek;
        set
        {
            if (SetProperty(ref _selectedWeek, value))
            {
                // Automatically load a new question when week changes
                _ = LoadNewQuestionAsync();
            }
        }
    }

    /// <summary>
    /// Gets or sets the list of available weeks.
    /// </summary>
    public List<int> AvailableWeeks
    {
        get => _availableWeeks;
        private set => SetProperty(ref _availableWeeks, value);
    }

    /// <summary>
    /// Gets the list of available difficulty levels.
    /// </summary>
    public List<string> DifficultyLevels => _difficultyLevels;

    /// <summary>
    /// Gets or sets the selected difficulty level.
    /// </summary>
    public string SelectedDifficulty
    {
        get => _selectedDifficulty;
        set
        {
            if (SetProperty(ref _selectedDifficulty, value))
            {
                // Automatically load a new question when difficulty changes
                _ = LoadNewQuestionAsync();
            }
        }
    }

    /// <summary>
    /// Gets the display text for the current question.
    /// </summary>
    public string QuestionDisplay
    {
        get
        {
            if (CurrentVocabulary != null)
            {
                return CurrentVocabulary.ChineseText;
            }
            return "Click 'Load Question' to start";
        }
    }

    /// <summary>
    /// Gets the hint text for the current question.
    /// </summary>
    public string HintDisplay
    {
        get
        {
            if (CurrentVocabulary != null && !string.IsNullOrEmpty(CurrentVocabulary.Hint))
            {
                return $"Hint: {CurrentVocabulary.Hint}";
            }
            return string.Empty;
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
    /// Loads a new vocabulary question.
    /// </summary>
    private async Task LoadNewQuestionAsync()
    {
        try
        {
            IsLoading = true;
            UserAnswer = string.Empty;
            FeedbackMessage = "Loading vocabulary...";

            CurrentVocabulary = await _vocabularyService.GetRandomVocabularyItemAsync(SelectedWeek, SelectedDifficulty);

            if (CurrentVocabulary == null)
            {
                var difficultyMsg = SelectedDifficulty == "All" ? "" : $" with difficulty '{SelectedDifficulty}'";
                FeedbackMessage = $"No vocabulary found for week {SelectedWeek}{difficultyMsg}. Please check your data files or select a different filter.";
                return;
            }

            FeedbackMessage = string.Empty;
            OnPropertyChanged(nameof(QuestionDisplay));
            OnPropertyChanged(nameof(HintDisplay));
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
    /// Submits and checks the vocabulary answer.
    /// </summary>
    private async void SubmitAnswer()
    {
        if (CurrentVocabulary == null)
        {
            return;
        }

        var userAnswerTrimmed = UserAnswer.Trim();

        // Check if the answer matches the main answer or any alternative answers
        var isCorrect = string.Equals(userAnswerTrimmed, CurrentVocabulary.JapaneseAnswer, StringComparison.OrdinalIgnoreCase) ||
                        CurrentVocabulary.AlternativeAnswers.Any(alt =>
                            string.Equals(userAnswerTrimmed, alt, StringComparison.OrdinalIgnoreCase));

        if (isCorrect)
        {
            FeedbackMessage = $"Correct! The answer is: {CurrentVocabulary.JapaneseAnswer}";
            if (CurrentVocabulary.AlternativeAnswers.Count > 0)
            {
                FeedbackMessage += $" (Also acceptable: {string.Join(", ", CurrentVocabulary.AlternativeAnswers)})";
            }

            // Auto-load next question after a short delay to show the feedback
            await Task.Delay(1500);
            await LoadNewQuestionAsync();
        }
        else
        {
            FeedbackMessage = $"Incorrect. The correct answer is: {CurrentVocabulary.JapaneseAnswer}";
            if (CurrentVocabulary.AlternativeAnswers.Count > 0)
            {
                FeedbackMessage += $" (Also acceptable: {string.Join(", ", CurrentVocabulary.AlternativeAnswers)})";
            }
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
