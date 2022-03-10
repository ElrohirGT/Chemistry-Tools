using Chemistry_Tools.Core.Updaters;

using ReactiveUI;

namespace Chemistry_Tools.ViewModels;
public class UpdatePopUpViewModel : ViewModelBase
{
    private UpdateItem _updateItem;

    public ReactiveCommand<bool, bool> CloseDialogCommand { get; }
    public UpdateItem UpdateItem
    {
        get => _updateItem;
        set => this.RaiseAndSetIfChanged(ref _updateItem, value);
    }

    public UpdatePopUpViewModel() => CloseDialogCommand = ReactiveCommand.Create<bool, bool>(v => v);
}
