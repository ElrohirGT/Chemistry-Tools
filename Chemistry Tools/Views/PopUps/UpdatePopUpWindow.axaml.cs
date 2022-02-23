using System;

using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using Chemistry_Tools.ViewModels;

using ReactiveUI;

namespace Chemistry_Tools.Views.PopUps;
public partial class UpdatePopUpWindow : ReactiveWindow<UpdatePopUpViewModel>
{
    readonly UpdatePopUpViewModel _viewModel = new();
    public UpdatePopUpWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        DataContext = _viewModel;
        this.WhenActivated(d => d(_viewModel!.CloseDialogCommand.Subscribe(v => Close(v))));
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
