using System.Reactive.Disposables;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using Chemistry_Tools.ViewModels;

using ReactiveUI;

namespace Chemistry_Tools.Views;
public partial class ConfigurationView : ReactiveUserControl<ConfigurationViewModel>
{
    public ConfigurationView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.WhenActivated(disposables =>
        {
            ViewModel?.ReloadData();
            Disposable.Create(() => ViewModel?.SaveData()).DisposeWith(disposables);
        });
        AvaloniaXamlLoader.Load(this);
    }
}
