using System;
using System.Linq;
using System.Reactive;

using Avalonia.Collections;

using Chemistry_Tools.Core.Services.PeriodicTableService;
using Chemistry_Tools.UserSettings;
using Chemistry_Tools.UserSettings.WindowsLanguage;

using ReactiveUI;

namespace Chemistry_Tools.ViewModels;
public class ReactionStoichiometryViewModel : BaseViewModelWithResources<ReactionStoichiometryWindowLanguage, object>, IRoutableViewModel
{
    public ReactionStoichiometryViewModel(IUserSettings appSettings, IScreen host, IPeriodicTableService periodicTable) : base(appSettings)
    {
        HostScreen = host;
        _periodicTable = periodicTable;

        var canParse = this.WhenAnyValue(x => x.TextBoxText, (s) => !string.IsNullOrEmpty(s));
        var canExecute = this.WhenAnyValue(x => x.SelectedProduct, x => x.ChemistryReactants, (p, ps) => p is not null && ps.Count != 0);
        var canCalculate = this.WhenAnyValue(x => x.LimitingReactant, x => x.SelectedProduct, (l, p) => l is not null && p is not null);

        ParseEquation = ReactiveCommand.Create<string>(ParseInputtedEquation, canParse);
        Calculate = ReactiveCommand.Create<IChemistryMolecule>(CalculateLimitingReactant, canExecute);
        CalculateEfficiency = ReactiveCommand.Create<double>(CalculateEfficiencyOfReaction, canCalculate);
    }

    private void CalculateEfficiencyOfReaction(double experimentMass)
    {
        decimal mass = Convert.ToDecimal(experimentMass);
        SecondarySuccessMessage = string.Format(CurrentWindowLanguage?.SecondarySuccessMessageFormat, mass / LimitingReactant.Value.AmountProduced, SelectedProduct.ChemicalComposition);
    }

    private void CalculateLimitingReactant(IChemistryMolecule targetProduct)
    {
        SecondarySuccessMessage = null;
        LimitingReactant = null;
        ExperimentalMass = 0;

        (IChemistryMolecule? Molecule, decimal AmountProduced) limitingReactant = (null, decimal.MaxValue);
        foreach (var reactant in ChemistryReactants)
        {
            decimal reactantGrams = Convert.ToDecimal(reactant.Grams);
            decimal total = reactant.GramsProducedInAReaction(reactantGrams, targetProduct);
            if (total < limitingReactant.AmountProduced)
                limitingReactant = (reactant, total);
        }

        SuccessMessage = string.Format(CurrentWindowLanguage?.SuccessMessageFormat, limitingReactant.Molecule.ChemicalComposition, targetProduct.ChemicalComposition, limitingReactant.AmountProduced);
        LimitingReactant = limitingReactant;
    }

    private void ParseInputtedEquation(string reactionInText)
    {
        ChemistryProducts.Clear();
        ChemistryReactants.Clear();
        LimitingReactant = null;
        SecondarySuccessMessage = null;
        SelectedProduct = null;
        ExperimentalMass = 0;

        if (!_periodicTable.TryGetChemistryEquation(reactionInText, out ChemistryEquation equation))
        {
            SuccessMessage = null;
            ErrorMessage = CurrentWindowLanguage?.ErrorMessageFormat;
            return;
        }

        ErrorMessage = null;
        ChemistryProducts.AddRange(equation.Products);
        ChemistryReactants.AddRange(equation.Reactants);
        SelectedProduct = equation.Products.FirstOrDefault();
    }

    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString()[..5];
    public IScreen HostScreen { get; }

    private readonly IPeriodicTableService _periodicTable;
    private readonly AvaloniaList<IChemistryMolecule> _chemistryProducts = new();
    private readonly AvaloniaList<IChemistryMolecule> _chemistryReactants = new();
    private IChemistryMolecule? _selectedProduct;
    private string? _textBoxText;
    private string? _successMessage;
    private string? _errorMessage;
    private (IChemistryMolecule? Molecule, decimal AmountProduced)? _limitingReactant;
    private string? _secondarySuccessMessage;
    private decimal _experimentalMass;

    public string? SuccessMessage
    {
        get => _successMessage;
        set => this.RaiseAndSetIfChanged(ref _successMessage, value);
    }
    public string? SecondarySuccessMessage
    {
        get => _secondarySuccessMessage;
        set => this.RaiseAndSetIfChanged(ref _secondarySuccessMessage, value);
    }
    public string? ErrorMessage
    {
        get => _errorMessage;
        set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
    }
    public string? TextBoxText
    {
        get => _textBoxText;
        set => this.RaiseAndSetIfChanged(ref _textBoxText, value);
    }
    public decimal ExperimentalMass
    {
        get => _experimentalMass;
        set => this.RaiseAndSetIfChanged(ref _experimentalMass, value);
    }
    public AvaloniaList<IChemistryMolecule> ChemistryProducts => _chemistryProducts;
    public AvaloniaList<IChemistryMolecule> ChemistryReactants => _chemistryReactants;
    public IChemistryMolecule? SelectedProduct
    {
        get => _selectedProduct;
        set => this.RaiseAndSetIfChanged(ref _selectedProduct, value);
    }
    public ReactiveCommand<string, Unit> ParseEquation { get; }
    public ReactiveCommand<double, Unit> CalculateEfficiency { get; set; }
    public ReactiveCommand<IChemistryMolecule, Unit> Calculate { get; }
    public (IChemistryMolecule? Molecule, decimal AmountProduced)? LimitingReactant
    {
        get => _limitingReactant;
        private set => this.RaiseAndSetIfChanged(ref _limitingReactant, value);
    }

    protected override ReactionStoichiometryWindowLanguage? GetCurrentWindowLanguage(Language? currentLanguage) => currentLanguage?.ReactionStoichiometryWindow;
    protected override object? GetCurrentWindowResources(Resources? currentResources) => null;
}
