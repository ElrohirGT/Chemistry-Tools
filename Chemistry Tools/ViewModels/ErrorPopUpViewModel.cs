using System;
using System.Reactive;

using ReactiveUI;

namespace Chemistry_Tools.ViewModels;
public class ErrorPopUpViewModel : ViewModelBase
{
    public event Action<Unit>? Close;

    private Exception _error;

    public Exception Error
    {
        get => _error;
        set
        {
            this.RaiseAndSetIfChanged(ref _error, value);
            var ex = value;
            while (ex.InnerException is not null)
                ex = ex.InnerException;
            FullErrorMessage = ex.ToString();
        }
    }

    private string _fullErrorMessage;

    public string FullErrorMessage
    {
        get => _fullErrorMessage;
        set => this.RaiseAndSetIfChanged(ref _fullErrorMessage, value);
    }

    public ErrorPopUpViewModel()
    {
        CloseDialogCommand = ReactiveCommand.Create(OnWindowClosing);
        CopyErrorCommand = ReactiveCommand.Create<string>(s => TextCopy.ClipboardService.SetText(s));
    }

    private void OnWindowClosing() => Close?.Invoke(Unit.Default);

    public ReactiveCommand<Unit, Unit> CloseDialogCommand { get; }
    public ReactiveCommand<string, Unit> CopyErrorCommand { get; set; }
}
