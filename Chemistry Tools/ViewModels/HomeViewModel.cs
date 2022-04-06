using System;

using Chemistry_Tools.UserSettings;

using ReactiveUI;

namespace Chemistry_Tools.ViewModels;
public class HomeViewModel : ViewModelBase, IRoutableViewModel
{
    public HomeViewModel(IUserSettings appSettings, IScreen hostScreen) : base(appSettings)
    {
        HostScreen = hostScreen;
    }

    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);
    public IScreen HostScreen { get; }
}
