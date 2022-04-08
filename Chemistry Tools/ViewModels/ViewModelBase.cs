using System;
using System.Threading.Tasks;

using Chemistry_Tools.UserSettings;

using ReactiveUI;

namespace Chemistry_Tools.ViewModels;
public abstract class ViewModelBase : ReactiveObject
{
    private IUserSettings? _userSettings;

    /// <summary>
    /// Get's the current user settings used in the app.
    /// </summary>
    public IUserSettings? UserSettings
    {
        get => _userSettings;
        set
        {
            if (_userSettings == value)
                return;
            this.RaisePropertyChanging();
            OnGlobalUserSettingsChanging(_userSettings);
            _userSettings = value;
            this.RaisePropertyChanged();
            OnGlobalUserSettingsChanged(value);
        }
    }

    protected abstract void OnGlobalUserSettingsChanging(IUserSettings? settings);
    protected abstract void OnGlobalUserSettingsChanged(IUserSettings? settings);

    /// <summary>
    /// Helps in the construction of a viewmodel. 
    /// </summary>
    /// <param name="appSettings">The gloabl instance of app settings that is used.</param>
    protected ViewModelBase(IUserSettings appSettings)
    {
        UserSettings = appSettings;
    }

    public virtual Task OnOpened(EventArgs e) => Task.CompletedTask;

    public virtual Task OnClosed(EventArgs e) => Task.CompletedTask;
}
