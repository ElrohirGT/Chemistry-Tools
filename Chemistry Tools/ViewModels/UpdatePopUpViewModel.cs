using Chemistry_Tools.Core.Updaters;
using Chemistry_Tools.UserSettings;
using Chemistry_Tools.UserSettings.WindowsLanguage;

using ReactiveUI;

namespace Chemistry_Tools.ViewModels;
public class UpdatePopUpViewModel : BaseViewModelWithResources<UpdatePopUpWindowLanguage, object>
{
    private UpdateItem _updateItem;

    public ReactiveCommand<bool, bool> CloseDialogCommand { get; }
    public UpdateItem UpdateItem
    {
        get => _updateItem;
        set => this.RaiseAndSetIfChanged(ref _updateItem, value);
    }

    public UpdatePopUpViewModel(IUserSettings userSettings) : base(userSettings) => CloseDialogCommand = ReactiveCommand.Create<bool, bool>(v => v);

    protected override UpdatePopUpWindowLanguage? GetCurrentWindowLanguage(Language? currentLanguage) => currentLanguage?.UpdatePopUpWindow;
    protected override object? GetCurrentWindowResources(Resources? currentResources) => null;
}
