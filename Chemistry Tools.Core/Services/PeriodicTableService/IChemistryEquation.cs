
namespace Chemistry_Tools.Core.Services.PeriodicTableService;

public interface IChemistryEquation
{
    IList<IChemistryMolecule> Products { get; set; }
    IList<IChemistryMolecule> Reactants { get; set; }
}