using System.Windows;
using JapanesePracticeApp.UI.ViewModels;

namespace JapanesePracticeApp.UI.Views;

/// <summary>
/// Interaction logic for DatasetManagerWindow.xaml
/// </summary>
public partial class DatasetManagerWindow : Window
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DatasetManagerWindow"/> class.
    /// </summary>
    public DatasetManagerWindow(DatasetManagerViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;

        // Initialize the view model
        Loaded += async (s, e) => await viewModel.InitializeAsync();
    }
}
