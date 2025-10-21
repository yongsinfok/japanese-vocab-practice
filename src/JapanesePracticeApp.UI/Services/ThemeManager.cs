using System;
using System.Windows;

namespace JapanesePracticeApp.UI.Services;

public class ThemeManager
{
    private static ThemeManager? _instance;
    private const string LightThemeUri = "Themes/LightTheme.xaml";
    private const string DarkThemeUri = "Themes/DarkTheme.xaml";
    private Theme _currentTheme = Theme.Light;

    public static ThemeManager Instance => _instance ??= new ThemeManager();

    public Theme CurrentTheme
    {
        get => _currentTheme;
        private set
        {
            if (_currentTheme != value)
            {
                _currentTheme = value;
                ThemeChanged?.Invoke(this, _currentTheme);
            }
        }
    }

    public event EventHandler<Theme>? ThemeChanged;

    private ThemeManager()
    {
        // Initialize with light theme
        ApplyTheme(Theme.Light);
    }

    public void SetTheme(Theme theme)
    {
        ApplyTheme(theme);
        CurrentTheme = theme;
    }

    public void ToggleTheme()
    {
        var newTheme = CurrentTheme == Theme.Light ? Theme.Dark : Theme.Light;
        SetTheme(newTheme);
    }

    private void ApplyTheme(Theme theme)
    {
        var themeUri = theme == Theme.Light ? LightThemeUri : DarkThemeUri;
        var newTheme = new ResourceDictionary { Source = new Uri(themeUri, UriKind.Relative) };

        var app = Application.Current;
        if (app.Resources.MergedDictionaries.Count > 0)
        {
            // Replace the first dictionary (theme dictionary)
            app.Resources.MergedDictionaries[0] = newTheme;
        }
        else
        {
            // Add as first dictionary
            app.Resources.MergedDictionaries.Insert(0, newTheme);
        }
    }
}

public enum Theme
{
    Light,
    Dark
}
