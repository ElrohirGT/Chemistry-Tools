using System.Reactive.Disposables;

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
        this.WhenActivated(disposables =>
        {
            ViewModel?.ReloadData();
            Disposable.Create(() => ViewModel?.SaveData()).DisposeWith(disposables);
        });
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
