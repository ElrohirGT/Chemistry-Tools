namespace Chemistry_Tools.Core.Services.PeriodicTableService;
public class ChemistryMolecule : IChemistryMolecule
{
    public string ChemicalComposition { get; set; }
    public IDictionary<string, IChemistryElement> Elements { get; set; }
    public double Grams { get; set; }
    public decimal GramsByMol => Elements.Values.Sum(el => el.MolarMass * el.Quantity);
    public decimal MolQuantity { get; set; }

    public decimal GramsProducedInAReaction(decimal reactantGrams, IChemistryMolecule targetProduct)
        => reactantGrams * targetProduct.MolQuantity * targetProduct.GramsByMol / (GramsByMol * MolQuantity);
}
