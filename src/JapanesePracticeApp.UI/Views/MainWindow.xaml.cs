using System.IO;
using System.Windows;
using JapanesePracticeApp.UI.ViewModels;
using JapanesePracticeApp.Core.Services;
using JapanesePracticeApp.Infrastructure.Services;

namespace JapanesePracticeApp.UI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly MainViewModel _viewModel;
    private readonly IVocabularyService _vocabularyService;

    public MainWindow()
    {
        InitializeComponent();

        // Set up dependency injection manually (in a larger app, use a DI container)
        _vocabularyService = new VocabularyService();
        _viewModel = new MainViewModel(_vocabularyService);

        DataContext = _viewModel;

        // Initialize the vocabulary service after the window is loaded
        Loaded += async (s, e) => await InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        await _viewModel.InitializeAsync();
    }

    private void ManageDatasets_Click(object sender, RoutedEventArgs e)
    {
        var datasetManagerViewModel = new DatasetManagerViewModel(_vocabularyService);
        var datasetManagerWindow = new Views.DatasetManagerWindow(datasetManagerViewModel);
        datasetManagerWindow.ShowDialog();

        // Refresh the main window after closing the dataset manager
        _ = _viewModel.InitializeAsync();
    }
}