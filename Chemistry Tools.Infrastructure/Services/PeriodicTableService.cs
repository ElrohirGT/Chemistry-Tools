using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.RegularExpressions;

using Chemistry_Tools.Core.Services.PeriodicTableService;

namespace Chemistry_Tools.Infrastructure.Services;
public class PeriodicTableService : IPeriodicTableService
{
    private const string EQUATION_SEPARATOR = "->";
    private readonly Regex MOLECULE_SPLITTER = new(@"\(|\)|([A-Z][a-z]*)|(\d+\/\d+)|\d*", RegexOptions.Compiled | RegexOptions.ExplicitCapture);
    private readonly Regex VALID_MOLECULE_CHECKER = new(@"(?<molQuantity>(\d+\/\d+)|\d*)(?<molecule>(\(*[A-Z][a-z]*((\d+\/\d+)|\d*)\)*((\d+\/\d+)|\d*))+)", RegexOptions.Compiled | RegexOptions.ExplicitCapture);

    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
    };

    public Dictionary<string, ChemistryElement> PeriodicTable { get; set; }

    public PeriodicTableService(string fileName = "periodicTable.json") => PeriodicTable = ParseJson(fileName);

    private Dictionary<string, ChemistryElement> ParseJson(string fileName)
    {
        using var fileStream = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.Read);
        return JsonSerializer.Deserialize<Dictionary<string, ChemistryElement>>(fileStream, _options);
    }

    private bool TryGetElementsOfMolecule(string textMolecule, out ChemistryElement[] elements)
    {
        elements = Array.Empty<ChemistryElement>();
        if (!TryGetElementsAndQuantitites(textMolecule, out Dictionary<string, decimal> elementsInText))
            return false;
        if (elementsInText.Count == 0)
            return false;

        List<ChemistryElement> chemistryElements = new();
        foreach (var elementName in elementsInText.Keys)
        {
            if (!PeriodicTable.TryGetValue(elementName, out ChemistryElement chemistryElement))
                return false;
            chemistryElement.Quantity = elementsInText[elementName];
            chemistryElement.ElementName = elementName;
            chemistryElements.Add(chemistryElement);
        }

        elements = chemistryElements.ToArray();
        return true;
    }

    private bool TryGetElementsAndQuantitites(string textMolecule, out Dictionary<string, decimal> elementsInText)
    {
        elementsInText = new Dictionary<string, decimal>();
        string[] moleculeParts = MOLECULE_SPLITTER.Matches(textMolecule).Select(m => m.Value).ToArray()[..^1];

        var parenthesisScale = new Stack<decimal>(new decimal[] { 1M });
        var moleculeNumber = 1M;
        var previousNumber = 1M;
        var parenthesisInBalance = true;

        for (int i = moleculeParts.Length-1; i > -1; i--)
        {
            var part = moleculeParts[i];
            decimal currentNumber;
            if (TryParseNumberPart(part, out currentNumber))
                previousNumber *= currentNumber * parenthesisScale.Peek();
            else if (part == ")")
            {
                parenthesisInBalance = !parenthesisInBalance;
                parenthesisScale.Push(previousNumber == 1 ? parenthesisScale.Peek() : previousNumber);
                previousNumber = 1M;
            }
            else if (part == "(")
            {
                parenthesisInBalance = !parenthesisInBalance;
                parenthesisScale.Pop();
                previousNumber = 1M;
            }
            else
            {
                moleculeNumber = previousNumber == 1 ? parenthesisScale.Peek() : previousNumber;
                elementsInText.TryAdd(part, 0);
                elementsInText[part] += moleculeNumber;

                moleculeNumber = 1M;
                previousNumber = 1M;
            }
        }
        return parenthesisInBalance;
    }

    private static bool TryParseNumberPart(string part, out decimal currentNumber)
    {
        if (decimal.TryParse(part, out currentNumber))
            return true;

        string[] fractionParts = part.Split('/');
        if (!fractionParts.All((s) => decimal.TryParse(s, out _)))
            return false;

        var numerator = decimal.Parse(fractionParts[0]);
        var denominator = decimal.Parse(fractionParts[1]);
        if (denominator == 0)
            return false;

        currentNumber =  numerator / denominator;
        return true;
    }

    public bool TryGetChemistryEquation(string reactionInText, [NotNullWhen(true)] out ChemistryEquation? reaction)
    {
        reaction = null;
        if (!reactionInText.Contains(EQUATION_SEPARATOR))
            return false;

        var reactionParts = reactionInText.Split(EQUATION_SEPARATOR, StringSplitOptions.TrimEntries|StringSplitOptions.RemoveEmptyEntries);
        if (reactionParts.Length != 2)
            return false;

        if (!TryGetMolecules(reactionParts[0], out IList<IChemistryMolecule> reactants))
            return false;

        if (!TryGetMolecules(reactionParts[1], out IList<IChemistryMolecule> products))
            return false;

        reaction = new ChemistryEquation
        {
            Reactants = reactants,
            Products = products
        };
        return true;
    }

    private bool TryGetMolecules(string equationSide, out IList<IChemistryMolecule> molecules)
    {
        molecules = new List<IChemistryMolecule>();
        var moleculesInText = equationSide.Split('+');

        foreach (var possibleMolecule in moleculesInText)
        {
            if (!TryGetMolecule(possibleMolecule, out ChemistryMolecule? molecule))
                return false;
            molecules.Add(molecule);
        }

        return true;
    }

    public bool TryGetMolecule(string textMolecule, [NotNullWhen(true)] out ChemistryMolecule? molecule)
    {
        molecule = null;

        if (!VALID_MOLECULE_CHECKER.IsMatch(textMolecule))
            return false;

        var match = VALID_MOLECULE_CHECKER.Match(textMolecule);

        string molQuantityInText = match.Groups["molQuantity"].Value;
        molQuantityInText = string.IsNullOrEmpty(molQuantityInText) ? "1" : molQuantityInText;
        if (!TryParseNumberPart(molQuantityInText, out decimal molQuantity))
            return false;

        string chemicalComposition = match.Groups["molecule"].Value;
        if (!TryGetElementsOfMolecule(chemicalComposition, out ChemistryElement[] elements))
            return false;

        molecule = new ChemistryMolecule
        {
            Elements = new Dictionary<string, IChemistryElement>(),
            ChemicalComposition = chemicalComposition,
            MolQuantity = molQuantity
        };
        foreach (var element in elements)
            molecule.Elements.TryAdd(element.ElementName, element);
        return true;
    }
}
