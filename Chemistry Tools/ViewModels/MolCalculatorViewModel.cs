using System;
using System.Linq;
using System.Reactive;

using Chemistry_Tools.Core.Services.PeriodicTableService;
using Chemistry_Tools.UserSettings;
using Chemistry_Tools.UserSettings.WindowsLanguage;

using ReactiveUI;

namespace Chemistry_Tools.ViewModels;
public class MolCalculatorViewModel : BaseViewModelWithResources<MolCalculatorWindowLanguage, object>, IRoutableViewModel
{
    private string? _textBoxText;
    private readonly IPeriodicTableService _periodicTable;
    private string? _errorMessage;
    private string? _successMessage;

    public ReactiveCommand<string, Unit> Calculate { get; }
    public string? ErrorMessage
    {
        get => _errorMessage;
        set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
    }
    public string? SuccessMessage
    {
        get => _successMessage;
        set => this.RaiseAndSetIfChanged(ref _successMessage, value);
    }
    public string? TextBoxText
    {
        get => _textBoxText;
        set => this.RaiseAndSetIfChanged(ref _textBoxText, value);
    }
    public MolCalculatorViewModel(IUserSettings appSettings, IScreen hostScreen, IPeriodicTableService periodicTable) : base(appSettings)
    {
        HostScreen = hostScreen;
        IObservable<bool>? canExecute = this.WhenAnyValue(x => x.TextBoxText, (s) => !string.IsNullOrEmpty(s));
        Calculate = ReactiveCommand.Create<string>(CalculateMolOf, canExecute);
        _periodicTable = periodicTable;
    }

    public void CalculateMolOf(string textMolecule)
    {
        if (!_periodicTable.TryGetElementsOfMolecule(textMolecule, out ChemistryElement[] elements))
        {
            SuccessMessage = null;
            ErrorMessage = CurrentWindowLanguage?.ParseMoleculeErrorMessage;
        }
        else
        {
            ErrorMessage = null;
            decimal sum = elements.Sum((e) => e.MolarMass * e.Quantity);
            SuccessMessage = string.Format(CurrentWindowLanguage?.SuccessMessageFormat ?? string.Empty, sum);
        }
    }

    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString()[..5];
    public IScreen HostScreen { get; }

    protected override MolCalculatorWindowLanguage? GetCurrentWindowLanguage(Language? currentLanguage) => currentLanguage?.MolCalculatorWindow;
    protected override object? GetCurrentWindowResources(Resources? currentResources) => null;
}
