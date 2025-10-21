using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using JapanesePracticeApp.Core.Models;
using JapanesePracticeApp.Core.Services;

namespace JapanesePracticeApp.UI.ViewModels;

/// <summary>
/// View model for the Dataset Manager window.
/// </summary>
public class DatasetManagerViewModel : ViewModelBase
{
    private readonly IVocabularyService _vocabularyService;
    private int _selectedWeek = 1;
    private VocabularyItem? _selectedItem;
    private string _chineseText = string.Empty;
    private string _japaneseAnswer = string.Empty;
    private string _alternativeAnswers = string.Empty;
    private string _difficultyLevel = "Beginner";
    private string _hint = string.Empty;
    private string _statusMessage = string.Empty;
    private bool _isEditing;

    /// <summary>
    /// Initializes a new instance of the <see cref="DatasetManagerViewModel"/> class.
    /// </summary>
    public DatasetManagerViewModel(IVocabularyService vocabularyService)
    {
        _vocabularyService = vocabularyService ?? throw new ArgumentNullException(nameof(vocabularyService));

        AvailableWeeks = new ObservableCollection<int>();
        VocabularyItems = new ObservableCollection<VocabularyItem>();

        AddItemCommand = new RelayCommand(async () => await AddOrUpdateItemAsync(), () => CanSaveItem());
        DeleteItemCommand = new RelayCommand(async () => await DeleteItemAsync(), () => SelectedItem != null);
        CreateWeekCommand = new RelayCommand(async () => await CreateNewWeekAsync());
        ClearFormCommand = new RelayCommand(ClearForm);
        RefreshCommand = new RelayCommand(async () => await LoadDataAsync());
    }

    /// <summary>
    /// Gets the available weeks.
    /// </summary>
    public ObservableCollection<int> AvailableWeeks { get; }

    /// <summary>
    /// Gets the vocabulary items for the selected week.
    /// </summary>
    public ObservableCollection<VocabularyItem> VocabularyItems { get; }

    /// <summary>
    /// Gets or sets the selected week.
    /// </summary>
    public int SelectedWeek
    {
        get => _selectedWeek;
        set
        {
            if (SetProperty(ref _selectedWeek, value))
            {
                _ = LoadVocabularyForWeekAsync(value);
            }
        }
    }

    /// <summary>
    /// Gets or sets the selected vocabulary item.
    /// </summary>
    public VocabularyItem? SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (SetProperty(ref _selectedItem, value))
            {
                if (value != null)
                {
                    LoadItemToForm(value);
                }
                ((RelayCommand)DeleteItemCommand).RaiseCanExecuteChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets the Chinese text input.
    /// </summary>
    public string ChineseText
    {
        get => _chineseText;
        set
        {
            if (SetProperty(ref _chineseText, value))
            {
                ((RelayCommand)AddItemCommand).RaiseCanExecuteChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets the Japanese answer input.
    /// </summary>
    public string JapaneseAnswer
    {
        get => _japaneseAnswer;
        set
        {
            if (SetProperty(ref _japaneseAnswer, value))
            {
                ((RelayCommand)AddItemCommand).RaiseCanExecuteChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets the alternative answers (comma-separated).
    /// </summary>
    public string AlternativeAnswers
    {
        get => _alternativeAnswers;
        set => SetProperty(ref _alternativeAnswers, value);
    }

    /// <summary>
    /// Gets or sets the difficulty level.
    /// </summary>
    public string DifficultyLevel
    {
        get => _difficultyLevel;
        set => SetProperty(ref _difficultyLevel, value);
    }

    /// <summary>
    /// Gets or sets the hint.
    /// </summary>
    public string Hint
    {
        get => _hint;
        set => SetProperty(ref _hint, value);
    }

    /// <summary>
    /// Gets or sets the status message.
    /// </summary>
    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether we're editing an existing item.
    /// </summary>
    public bool IsEditing
    {
        get => _isEditing;
        set => SetProperty(ref _isEditing, value);
    }

    /// <summary>
    /// Gets the button text for add/update.
    /// </summary>
    public string AddButtonText => IsEditing ? "Update Item" : "Add Item";

    /// <summary>
    /// Gets the command to add or update an item.
    /// </summary>
    public ICommand AddItemCommand { get; }

    /// <summary>
    /// Gets the command to delete an item.
    /// </summary>
    public ICommand DeleteItemCommand { get; }

    /// <summary>
    /// Gets the command to create a new week.
    /// </summary>
    public ICommand CreateWeekCommand { get; }

    /// <summary>
    /// Gets the command to clear the form.
    /// </summary>
    public ICommand ClearFormCommand { get; }

    /// <summary>
    /// Gets the command to refresh data.
    /// </summary>
    public ICommand RefreshCommand { get; }

    /// <summary>
    /// Initializes the view model by loading available weeks.
    /// </summary>
    public async Task InitializeAsync()
    {
        await LoadDataAsync();
    }

    /// <summary>
    /// Loads all data (weeks and items).
    /// </summary>
    private async Task LoadDataAsync()
    {
        try
        {
            StatusMessage = "Loading data...";

            var weeks = await _vocabularyService.GetAvailableWeeksAsync();
            AvailableWeeks.Clear();
            foreach (var week in weeks)
            {
                AvailableWeeks.Add(week);
            }

            if (AvailableWeeks.Count > 0)
            {
                SelectedWeek = AvailableWeeks[0];
            }

            StatusMessage = "Data loaded successfully.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error loading data: {ex.Message}";
        }
    }

    /// <summary>
    /// Loads vocabulary items for the specified week.
    /// </summary>
    private async Task LoadVocabularyForWeekAsync(int weekNumber)
    {
        try
        {
            StatusMessage = $"Loading week {weekNumber}...";

            var items = await _vocabularyService.LoadVocabularyByWeekAsync(weekNumber);
            VocabularyItems.Clear();
            foreach (var item in items)
            {
                VocabularyItems.Add(item);
            }

            StatusMessage = $"Loaded {items.Count} items for week {weekNumber}.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error loading vocabulary: {ex.Message}";
        }
    }

    /// <summary>
    /// Adds or updates a vocabulary item.
    /// </summary>
    private async Task AddOrUpdateItemAsync()
    {
        try
        {
            var item = IsEditing && SelectedItem != null
                ? SelectedItem
                : new VocabularyItem();

            item.ChineseText = ChineseText.Trim();
            item.JapaneseAnswer = JapaneseAnswer.Trim();
            item.AlternativeAnswers = AlternativeAnswers
                .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(a => a.Trim())
                .ToList();
            item.DifficultyLevel = DifficultyLevel;
            item.Hint = Hint.Trim();
            item.WeekNumber = SelectedWeek;

            if (!IsEditing)
            {
                VocabularyItems.Add(item);
            }

            // Save to file
            await _vocabularyService.SaveVocabularyByWeekAsync(SelectedWeek, VocabularyItems.ToList());

            StatusMessage = IsEditing
                ? "Item updated successfully!"
                : "Item added successfully!";

            ClearForm();
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error saving item: {ex.Message}";
        }
    }

    /// <summary>
    /// Deletes the selected vocabulary item.
    /// </summary>
    private async Task DeleteItemAsync()
    {
        if (SelectedItem == null)
        {
            return;
        }

        try
        {
            var result = MessageBox.Show(
                $"Are you sure you want to delete '{SelectedItem.ChineseText}'?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                VocabularyItems.Remove(SelectedItem);
                await _vocabularyService.SaveVocabularyByWeekAsync(SelectedWeek, VocabularyItems.ToList());

                StatusMessage = "Item deleted successfully!";
                ClearForm();
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error deleting item: {ex.Message}";
        }
    }

    /// <summary>
    /// Creates a new week.
    /// </summary>
    private async Task CreateNewWeekAsync()
    {
        try
        {
            // Find the next available week number
            var nextWeek = AvailableWeeks.Count > 0 ? AvailableWeeks.Max() + 1 : 1;

            await _vocabularyService.CreateNewWeekAsync(nextWeek);
            AvailableWeeks.Add(nextWeek);
            SelectedWeek = nextWeek;

            StatusMessage = $"Week {nextWeek} created successfully!";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error creating week: {ex.Message}";
        }
    }

    /// <summary>
    /// Loads an item into the form for editing.
    /// </summary>
    private void LoadItemToForm(VocabularyItem item)
    {
        IsEditing = true;
        ChineseText = item.ChineseText;
        JapaneseAnswer = item.JapaneseAnswer;
        AlternativeAnswers = string.Join(", ", item.AlternativeAnswers);
        DifficultyLevel = item.DifficultyLevel;
        Hint = item.Hint ?? string.Empty;

        OnPropertyChanged(nameof(AddButtonText));
    }

    /// <summary>
    /// Clears the form.
    /// </summary>
    private void ClearForm()
    {
        IsEditing = false;
        SelectedItem = null;
        ChineseText = string.Empty;
        JapaneseAnswer = string.Empty;
        AlternativeAnswers = string.Empty;
        DifficultyLevel = "Beginner";
        Hint = string.Empty;

        OnPropertyChanged(nameof(AddButtonText));
    }

    /// <summary>
    /// Determines whether the item can be saved.
    /// </summary>
    private bool CanSaveItem()
    {
        return !string.IsNullOrWhiteSpace(ChineseText) &&
               !string.IsNullOrWhiteSpace(JapaneseAnswer);
    }
}
