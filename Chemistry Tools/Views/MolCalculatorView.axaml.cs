using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using Chemistry_Tools.ViewModels;

namespace Chemistry_Tools.Views;
public partial class MolCalculatorView : ReactiveUserControl<MolCalculatorViewModel>
{
    public MolCalculatorView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
