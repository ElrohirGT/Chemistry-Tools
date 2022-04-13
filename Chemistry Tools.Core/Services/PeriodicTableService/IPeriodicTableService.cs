using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chemistry_Tools.Core.Services.PeriodicTableService;
public interface IPeriodicTableService
{
    bool TryGetElementsOfMolecule(string textMolecule, out ChemistryElement[] elements);
    bool TryGetChemistryEquation(string reactionInText, out ChemistryEquation reaction);
}
