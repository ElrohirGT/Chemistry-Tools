using System;

using Chemistry_Tools.UserSettings;
using Chemistry_Tools.UserSettings.WindowsLanguage;

using ReactiveUI;

namespace Chemistry_Tools.ViewModels;
public class ReactionStoichiometryViewModel : BaseViewModelWithResources<ReactionStoichiometryWindowLanguage, object>, IRoutableViewModel
{
    public ReactionStoichiometryViewModel(IUserSettings appSettings, IScreen host) : base(appSettings)
    {
        HostScreen = host;
    }

    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString()[..5];
    public IScreen HostScreen { get; }

    protected override ReactionStoichiometryWindowLanguage? GetCurrentWindowLanguage(Language? currentLanguage) => currentLanguage?.ReactionStoichiometryWindow;
    protected override object? GetCurrentWindowResources(Resources? currentResources) => null;
}
