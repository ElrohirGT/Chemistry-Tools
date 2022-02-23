using NetSparkleUpdater;

using ReactiveUI;

namespace Chemistry_Tools.ViewModels;
public class UpdatePopUpViewModel : ViewModelBase
{
    private AppCastItem _appCastItem;

    public ReactiveCommand<bool, bool> CloseDialogCommand { get; }
    public AppCastItem AppCastItem
    {
        get => _appCastItem;
        set => this.RaiseAndSetIfChanged(ref _appCastItem, value);
    }

    public UpdatePopUpViewModel()
    {
        CloseDialogCommand = ReactiveCommand.Create<bool, bool>(v => v);
    }
}
