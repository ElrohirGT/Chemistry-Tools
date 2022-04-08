using Chemistry_Tools.UserSettings;

using ReactiveUI;

namespace Chemistry_Tools.ViewModels;
public abstract class BaseViewModelWithResources<WindowLanguage, WindowResources> : ViewModelBase
{
    private WindowLanguage? _currentWindowLanguage;
    private WindowResources? _currentWindowResources;

    protected override void OnGlobalUserSettingsChanged(IUserSettings? settings)
    {
        RegisterForLanguageOrThemeChange(settings);
        CurrentWindowLanguage = GetCurrentWindowLanguage(settings?.CurrentLanguage);
        CurrentWindowResources = GetCurrentWindowResources(settings?.CurrentTheme?.Resources);
    }

    protected override void OnGlobalUserSettingsChanging(IUserSettings? settings) => UnregisterForLanguageOrThemeChange(settings);

    private void UnregisterForLanguageOrThemeChange(IUserSettings? settings)
    {
        if (settings is null)
            return;
        settings.ThemeChanged -= OnThemeChanged;
        settings.LanguageChanged -= OnLanguageChanged;
    }

    private void RegisterForLanguageOrThemeChange(IUserSettings? settings)
    {
        if (settings is null)
            return;
        settings.ThemeChanged -= OnThemeChanged;
        settings.ThemeChanged += OnThemeChanged;

        settings.LanguageChanged -= OnLanguageChanged;
        settings.LanguageChanged += OnLanguageChanged;
    }

    private void OnLanguageChanged(Language? languagedChangedTo) => CurrentWindowLanguage = GetCurrentWindowLanguage(languagedChangedTo);
    private void OnThemeChanged(Theme? themeChangedTo) => CurrentWindowResources = GetCurrentWindowResources(themeChangedTo?.Resources);

    public WindowLanguage? CurrentWindowLanguage
    {
        get => _currentWindowLanguage;
        set => this.RaiseAndSetIfChanged(ref _currentWindowLanguage, value);
    }
    public WindowResources? CurrentWindowResources
    {
        get => _currentWindowResources;
        set => this.RaiseAndSetIfChanged(ref _currentWindowResources, value);
    }

    /// <summary>
    /// Helps in the construction of a viewmodel. 
    /// </summary>
    /// <param name="appSettings">The gloabl instance of app settings that is used.</param>
    protected BaseViewModelWithResources(IUserSettings appSettings) : base(appSettings) 
        => RegisterForLanguageOrThemeChange(UserSettings);

    protected abstract WindowLanguage? GetCurrentWindowLanguage(Language? currentLanguage);
    protected abstract WindowResources? GetCurrentWindowResources(Resources? currentResources);
}
