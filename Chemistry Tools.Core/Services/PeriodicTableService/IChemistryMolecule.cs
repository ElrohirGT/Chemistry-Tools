
namespace Chemistry_Tools.Core.Services.PeriodicTableService;

public interface IChemistryMolecule
{
    string ChemicalComposition { get; set; }
    IDictionary<string, IChemistryElement> Elements { get; set; }
    decimal MolQuantity { get; set; }
    double Grams { get; set; }
    decimal GramsByMol { get; }

    decimal GramsProducedInAReaction(decimal reactantGrams, IChemistryMolecule targetProduct);
}