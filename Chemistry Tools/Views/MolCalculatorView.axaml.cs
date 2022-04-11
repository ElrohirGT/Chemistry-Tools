using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using Chemistry_Tools.ViewModels;

using ReactiveUI;

namespace Chemistry_Tools.Views;
public partial class MolCalculatorView : ReactiveUserControl<MolCalculatorViewModel>
{
    public TextBox MoleculeTextBox { get; private set; }

    public MolCalculatorView()
    {
        InitializeComponent();
        this.WhenActivated(disposables => disposables(MoleculeTextBox.GetObservable(TextBox.TextProperty).InvokeCommand(ViewModel.Calculate)));
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        MoleculeTextBox = this.FindControl<TextBox>("MoleculeTextBlock");
    }
}
