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

    public MainWindow()
    {
        InitializeComponent();

        // Set up dependency injection manually (in a larger app, use a DI container)
        var llmService = new LlamaSharpService();
        var questionService = new QuestionGenerationService(llmService);
        _viewModel = new MainViewModel(questionService);

        DataContext = _viewModel;

        // Initialize the LLM after the window is loaded
        Loaded += async (s, e) => await InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        // Look for the model file in the models directory
        var modelDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "models");
        var modelPath = Directory.Exists(modelDirectory)
            ? Directory.GetFiles(modelDirectory, "*.gguf").FirstOrDefault()
            : null;

        if (string.IsNullOrEmpty(modelPath))
        {
            MessageBox.Show(
                "No GGUF model file found in the 'models' directory.\n\n" +
                "Please download a model (e.g., Mistral 7B) and place it in the 'models' folder at the root of the project.",
                "Model Not Found",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
            return;
        }

        await _viewModel.InitializeAsync(modelPath);
    }
}