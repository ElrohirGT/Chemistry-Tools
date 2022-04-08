
using Chemistry_Tools.Core.Services.PeriodicTableService;

namespace Chemistry_Tools.Infrastructure.Services;
public class PeriodicTable : IPeriodicTableService
{
    public bool TryParseMolecule(string textMolecule, out ChemistryElement[] elements) => throw new NotImplementedException();
}
