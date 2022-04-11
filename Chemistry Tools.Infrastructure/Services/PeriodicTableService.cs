using System.Text.Json;
using System.Text.RegularExpressions;

using Chemistry_Tools.Core.Services.PeriodicTableService;

namespace Chemistry_Tools.Infrastructure.Services;
public class PeriodicTableService : IPeriodicTableService
{
    private readonly Regex MOLECULE_SPLITTER = new(@"\(|\)|([A-Z][a-z]*)|(\d+/\d+)|\d*", RegexOptions.Compiled|RegexOptions.ExplicitCapture);

    private JsonSerializerOptions _options = new()
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

        currentNumber = decimal.Parse(fractionParts[0]) / decimal.Parse(fractionParts[1]);
        return true;
    }
}
