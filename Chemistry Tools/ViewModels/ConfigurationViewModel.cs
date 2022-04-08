using System;

using Chemistry_Tools.UserSettings;
using Chemistry_Tools.UserSettings.WindowsLanguage;

using ReactiveUI;

namespace Chemistry_Tools.ViewModels;
public class ConfigurationViewModel : BaseViewModelWithResources<ConfigurationWindowLanguage, object>, IRoutableViewModel
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

    internal void SaveData()
    {
        UserSettings.Save().Wait();
        foreach (var theme in Themes)
            if (theme != UserSettings.CurrentTheme)
                theme.Dispose();
    }

    protected override ConfigurationWindowLanguage? GetCurrentWindowLanguage(Language? currentLanguage) => currentLanguage?.ConfigurationWindow;
    protected override object? GetCurrentWindowResources(Resources? currentResources) => null;

    public Language[] Languages
    {
        get => _languages;
        private set => this.RaiseAndSetIfChanged(ref _languages, value);
    }

    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString()[..5];
    public IScreen HostScreen { get; }
}
