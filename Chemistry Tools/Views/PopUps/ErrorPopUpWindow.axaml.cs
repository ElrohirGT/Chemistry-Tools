using System.Reactive;

using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using Chemistry_Tools.ViewModels;

namespace Chemistry_Tools.Views.PopUps;
public partial class ErrorPopUpWindow : ReactiveWindow<ErrorPopUpViewModel>
{
    public ErrorPopUpWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        DataContext = ViewModel = new ErrorPopUpViewModel();
        ViewModel.Close -= CloseWindow;
        ViewModel.Close += CloseWindow;
    }

    private void CloseWindow(Unit obj) => Close(obj);

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
