using System;
using System.Threading.Tasks;

using Chemistry_Tools.UserSettings;

using ReactiveUI;

namespace Chemistry_Tools.ViewModels;
public abstract class ViewModelBase : ReactiveObject
{
    private IUserSettings _appSettings;

    public IUserSettings UserSettings
    {
        get => _appSettings;
        set => this.RaiseAndSetIfChanged(ref _appSettings, value);
    }
    /// <summary>
    /// Helps in the construction of a viewmodel. 
    /// </summary>
    /// <param name="appSettings">The gloabl instance of app settings that is used.</param>
    protected ViewModelBase(IUserSettings appSettings) => UserSettings = appSettings;

    public virtual Task OnOpened(EventArgs e) => Task.CompletedTask;
}
