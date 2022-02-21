using ReactiveUI;

namespace Chemistry_Tools.ViewModels;
public class UpdatePopUpViewModel : ViewModelBase
{
    public ReactiveCommand<bool, bool> CloseDialogCommand { get; }

    public UpdatePopUpViewModel()
    {
        CloseDialogCommand = ReactiveCommand.Create<bool, bool>(v => v);
    }
}
