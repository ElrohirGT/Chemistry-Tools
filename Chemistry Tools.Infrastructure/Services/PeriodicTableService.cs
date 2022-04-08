
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

using Chemistry_Tools.Core.Services.PeriodicTableService;

namespace Chemistry_Tools.Infrastructure.Services;
public class PeriodicTableService : IPeriodicTableService
{
    private readonly Regex ELEMENT_SPLITTER = new(@"[A-Z][a-z]*\d*", RegexOptions.Compiled);
    private readonly Regex ELEMENT_COMPONENTS = new(@"\d+", RegexOptions.Compiled);
    private readonly Regex MOLECULE_SPLITTER = new(@"([A-Z][a-z]*)|\d*|\(|\)", RegexOptions.Compiled|RegexOptions.ExplicitCapture);

    private JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
    };

    public Dictionary<string, ChemistryElement> PeriodicTable { get; set; }

    public PeriodicTableService() => PeriodicTable = ParseJson("periodicTable.json");

    private Dictionary<string, ChemistryElement> ParseJson(string fileName)
    {
        using var fileStream = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.Read);
        return JsonSerializer.Deserialize<Dictionary<string, ChemistryElement>>(fileStream, _options);
    }

    public bool TryGetElementsOfMolecule(string textMolecule, out ChemistryElement[] elements)
    {
        elements = Array.Empty<ChemistryElement>();
        if (!TryGetElementsAndQuantitites(textMolecule, out Dictionary<string, decimal> elementsInText))
            return false;
        if (elementsInText.Count == 0)
            return false;

        List<ChemistryElement> chemistryElements = new();
        foreach (var elementInText in elementsInText.Keys)
        {
            if (!PeriodicTable.TryGetValue(elementInText, out ChemistryElement? chemistryElement))
                return false;
            chemistryElement.Quantity = elementsInText[elementInText];
            chemistryElements.Add(chemistryElement);
        }

        elements = chemistryElements.ToArray();
        return true;
    }

    private bool TryGetElementsAndQuantitites(string textMolecule, out Dictionary<string, decimal> elementsInText)
    {
        elementsInText = new Dictionary<string, decimal>();

        if (!TryGetExpandedTextMolecule(textMolecule, out string expandedTextMolecule))
            return false;

        string[] elements = ELEMENT_SPLITTER.Matches(expandedTextMolecule).Select(m => m.Value).ToArray();

        foreach (var element in elements)
        {
            string capturedQuantity = ELEMENT_COMPONENTS.Match(element).Value;
            decimal quantity = string.IsNullOrEmpty(capturedQuantity) ? 1 : decimal.Parse(capturedQuantity);
            string elementName = element;
            if (!string.IsNullOrEmpty(capturedQuantity))
                elementName = element[..element.IndexOf(capturedQuantity)];

            elementsInText.TryAdd(elementName, 0);
            elementsInText[elementName] += quantity;
        }

        return true;
    }

    private bool TryGetExpandedTextMolecule(string textMolecule, out string expandedTextMolecule)
    {
        expandedTextMolecule = string.Empty;
        string[] moleculeParts = MOLECULE_SPLITTER.Matches(textMolecule).Select(m => m.Value).ToArray()[..^1];

        var parenthesisScale = 1M;
        var moleculeNumber = 1M;
        var previousNumber = 1M;
        var parenthesisInBalance = true;

        for (int i = moleculeParts.Length-1; i > -1; i--)
        {
            var part = moleculeParts[i];
            if (decimal.TryParse(part, out decimal currentNumber))
                previousNumber *= currentNumber;
            else if (string.IsNullOrEmpty(part))
            {
                parenthesisInBalance = !parenthesisInBalance;
                if (!parenthesisInBalance)
                    parenthesisScale = previousNumber;
                else
                    parenthesisScale = 1M;
                previousNumber = 1M;
            }
            else
            {
                moleculeNumber = previousNumber;
                moleculeParts[i] = $"{part}{parenthesisScale * moleculeNumber}";
                previousNumber = 1M;
            }
        }

        IEnumerable<string> values = moleculeParts.Where(s => !decimal.TryParse(s, out _)).Where((s)=>!string.IsNullOrEmpty(s));
        expandedTextMolecule = string.Join(string.Empty, values);
        return true;
    }
}
