using System;

using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using Chemistry_Tools.ViewModels;

using ReactiveUI;

using Splat;

namespace Chemistry_Tools.Views.PopUps;
public partial class UpdateDownloadingWindow : ReactiveWindow<UpdateDownloadingViewModel>
{
    private readonly UpdateDownloadingViewModel _videmodel;

    public UpdateDownloadingWindow()
    {
        InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        DataContext = _videmodel = Locator.Current.GetService<UpdateDownloadingViewModel>();
        this.WhenActivated(d => d(_videmodel.CancelDownloadCommand.Subscribe(v => Close(!v))));
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
