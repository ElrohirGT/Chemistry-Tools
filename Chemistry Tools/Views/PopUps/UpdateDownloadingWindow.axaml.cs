using System;
using System.ComponentModel;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using Chemistry_Tools.ViewModels;

using ReactiveUI;

namespace Chemistry_Tools.Views.PopUps;
public partial class UpdateDownloadingWindow : ReactiveWindow<UpdateDownloadingViewModel>
{
    public UpdateDownloadingWindow()
    {
        InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        DataContext = ViewModel = new UpdateDownloadingViewModel();
        this.WhenActivated(d => d(ViewModel.CancelDownloadCommand.Subscribe(v => Close(!v))));
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
