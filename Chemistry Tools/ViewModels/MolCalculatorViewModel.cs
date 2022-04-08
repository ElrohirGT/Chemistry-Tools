using System;
using System.Reactive;

using Chemistry_Tools.UserSettings;
using Chemistry_Tools.UserSettings.WindowsLanguage;

using ReactiveUI;

namespace Chemistry_Tools.ViewModels;
public class MolCalculatorViewModel : BaseViewModelWithResources<MolCalculatorWindowLanguage, object>, IRoutableViewModel
{
    private string? _textBoxText;

    public ReactiveCommand<string, Unit> Calculate { get; }
    public string? TextBoxText
    {
        get => _textBoxText;
        set => this.RaiseAndSetIfChanged(ref _textBoxText, value);
    }
    public MolCalculatorViewModel(IUserSettings appSettings, IScreen hostScreen) : base(appSettings)
    {
        HostScreen = hostScreen;
        var canExecute = this.WhenAnyValue(x => x.TextBoxText, (s) => !string.IsNullOrEmpty(s));
        Calculate = ReactiveCommand.Create<string>(CalculateMolOf, canExecute);
    }

    private void CalculateMolOf(string textMolecule)
    {

    }

    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString()[..5];
    public IScreen HostScreen { get; }

    protected override MolCalculatorWindowLanguage? GetCurrentWindowLanguage(Language? currentLanguage) => currentLanguage?.MolCalculatorWindow;
    protected override object? GetCurrentWindowResources(Resources? currentResources) => null;
}
