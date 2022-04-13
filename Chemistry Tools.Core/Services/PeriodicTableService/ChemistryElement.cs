namespace Chemistry_Tools.Core.Services.PeriodicTableService;

public struct ChemistryElement : IChemistryElement
{
    public string ElementName { get; set; }
    public decimal MolarMass { get; set; }
    public decimal Quantity { get; set; }

}
