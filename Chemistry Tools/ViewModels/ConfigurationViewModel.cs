using System;

using Chemistry_Tools.UserSettings;

using ReactiveUI;

namespace Chemistry_Tools.ViewModels;
public class ConfigurationViewModel : ViewModelBase, IRoutableViewModel
{
    private Theme[] _themes = Array.Empty<Theme>();
    private Language[] _languages = Array.Empty<Language>();

    public ConfigurationViewModel(IUserSettings appSettings, IScreen hostScreen) : base(appSettings)
    {
        HostScreen = hostScreen;
        ReloadData();
    }

    public void ReloadData()
    {
        foreach (var theme in Themes)
            theme?.Dispose();
        Themes = UserSettings.GetThemes();
        Languages = UserSettings.GetLanguages();
    }

    public Theme[] Themes
    {
        get => _themes;
        private set => this.RaiseAndSetIfChanged(ref _themes, value);
    }

    internal void SaveData() => UserSettings.Save().Wait();

    public Language[] Languages
    {
        get => _languages;
        private set => this.RaiseAndSetIfChanged(ref _languages, value);
    }

    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);
    public IScreen HostScreen { get; }
}
