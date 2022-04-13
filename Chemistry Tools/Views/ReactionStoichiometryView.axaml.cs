using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using Chemistry_Tools.ViewModels;

using ReactiveUI;

namespace Chemistry_Tools.Views;
public partial class ReactionStoichiometryView : ReactiveUserControl<ReactionStoichiometryViewModel>
{
    public ReactionStoichiometryView()
    {
        InitializeComponent();
        this.WhenActivated(disposables => disposables(InputTextBox.GetObservable(TextBox.TextProperty).InvokeCommand(ViewModel.ParseEquation)));
        this.WhenActivated(disposables => disposables(SecondaryInput.GetObservable(NumericUpDown.ValueProperty).InvokeCommand(ViewModel.CalculateEfficiency)));
    }

    public TextBox InputTextBox { get; private set; }
    public NumericUpDown SecondaryInput { get; private set; }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        InputTextBox = this.FindControl<TextBox>("InputTextBox");
        SecondaryInput = this.FindControl<NumericUpDown>("SecondaryInput");
    }
}
