namespace Chemistry_Tools.Core.Services.PeriodicTableService;
public class ChemistryEquation : IChemistryEquation
{
    public IList<IChemistryMolecule> Products { get; set; }
    public IList<IChemistryMolecule> Reactants { get; set; }
}
