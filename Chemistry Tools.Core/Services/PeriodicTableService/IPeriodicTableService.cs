using System.Diagnostics.CodeAnalysis;

namespace Chemistry_Tools.Core.Services.PeriodicTableService;
public interface IPeriodicTableService
{
    bool TryGetChemistryEquation(string reactionInText, [NotNullWhen(true)] out ChemistryEquation? reaction);
    bool TryGetMolecule(string textMolecule, [NotNullWhen(true)] out ChemistryMolecule? molecule);
}
