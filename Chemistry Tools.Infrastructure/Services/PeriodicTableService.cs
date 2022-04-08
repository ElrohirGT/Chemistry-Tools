
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

using Chemistry_Tools.Core.Services.PeriodicTableService;

namespace Chemistry_Tools.Infrastructure.Services;
public class PeriodicTableService : IPeriodicTableService
{
    private readonly Regex MOLECULE_SPLIT_PATTERN = new(@"[A-Z][a-z]*\d*", RegexOptions.Compiled);
    private readonly Regex ELEMENT_SPLIT_PATTERN = new(@"\d+", RegexOptions.Compiled);
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

    public bool TryParseMolecule(string textMolecule, out ChemistryElement[] elements)
    {
        Dictionary<string, decimal> elementsInText = GetElementsAndQuantities(textMolecule);
        elements = Array.Empty<ChemistryElement>();
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

    private Dictionary<string, decimal> GetElementsAndQuantities(string textMolecule)
    {
        var quantityByElement = new Dictionary<string, decimal>();
        string[] elements = MOLECULE_SPLIT_PATTERN.Matches(textMolecule).Select(m=>m.Value).ToArray();

        foreach(var element in elements)
        {
            string capturedQuantity = ELEMENT_SPLIT_PATTERN.Match(element).Value;
            decimal quantity = string.IsNullOrEmpty(capturedQuantity) ? 1 : decimal.Parse(capturedQuantity);
            string elementName = element;
            if (!string.IsNullOrEmpty(capturedQuantity))
                elementName = element[..element.IndexOf(capturedQuantity)];

            if (!quantityByElement.ContainsKey(element))
                quantityByElement.Add(elementName, 0);
            quantityByElement[elementName] += quantity;
        }

        return quantityByElement;
    }
}
