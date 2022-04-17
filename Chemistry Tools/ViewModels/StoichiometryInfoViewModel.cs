using System;
using System.Reactive;

using Chemistry_Tools.UserSettings;
using Chemistry_Tools.UserSettings.WindowsLanguage;
using Chemistry_Tools.UserSettings.WindowsResources;

using ReactiveUI;

namespace Chemistry_Tools.ViewModels;
public class StoichiometryInfoViewModel : BaseViewModelWithResources<StoichiometryInfoWindowLanguage, StoichiometryInfoWindowResources>, IRoutableViewModel
{
    public StoichiometryInfoViewModel(IUserSettings appSettings, IScreen host) : base(appSettings)
    {
        HostScreen = host;
        GoBack = host.Router.NavigateBack;
    }

    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString()[..5];
    public IScreen HostScreen { get; }
    public ReactiveCommand<Unit, Unit> GoBack { get; }

    protected override StoichiometryInfoWindowLanguage? GetCurrentWindowLanguage(Language? currentLanguage) => currentLanguage?.StoichiometryInfoWindow;
    protected override StoichiometryInfoWindowResources? GetCurrentWindowResources(Resources? currentResources) => currentResources?.StoichiometryInfoWindow;
}
