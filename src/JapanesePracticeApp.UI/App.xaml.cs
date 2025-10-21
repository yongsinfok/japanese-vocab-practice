using System.Configuration;
using System.Data;
using System.Windows;
using JapanesePracticeApp.UI.Services;

namespace JapanesePracticeApp.UI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Initialize ThemeManager
        _ = ThemeManager.Instance;
    }
}

