using System.Reactive;

using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using Chemistry_Tools.ViewModels;

using Splat;

namespace Chemistry_Tools.Views.PopUps;
public partial class ErrorPopUpWindow : ReactiveWindow<ErrorPopUpViewModel>
{
    private readonly ErrorPopUpViewModel _viewmodel;

    public ErrorPopUpWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        DataContext = _viewmodel = Locator.Current.GetService<ErrorPopUpViewModel>();
        _viewmodel.Close -= CloseWindow;
        _viewmodel.Close += CloseWindow;
    }

    private void CloseWindow(Unit obj) => Close(obj);

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
