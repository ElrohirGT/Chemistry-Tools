using System;
using System.Collections.Generic;

using Chemistry_Tools.Core.Services.PeriodicTableService;

using Xunit;

namespace Chemistry_Tools.Infrastructure.Tests;
public class ChemistryMoleculeTests
{
    [Fact]
    public void CanCalculateMolarMass()
    {
        var molecule = new ChemistryMolecule
        {
            Elements = new Dictionary<string, IChemistryElement>
            {
                {"H", new ChemistryElement{MolarMass = 2, Quantity = 2} },
                {"O", new ChemistryElement{MolarMass = 3, Quantity = 3} }
            }
        };

        Assert.Equal(2 * 2 + 3 * 3, molecule.GramsByMol);
    }

    [Fact]
    public void CanCalculateGramsProducedInAReaction()
    {
        var reactant = new ChemistryMolecule
        {
            Elements = new Dictionary<string, IChemistryElement>
            {
                {"V", new ChemistryElement{MolarMass=50.942M, Quantity =2M} },
                {"O", new ChemistryElement{MolarMass=15.999M, Quantity=5M} }
            },
            MolQuantity=1
        };
        var product = new ChemistryMolecule
        {
            Elements = new Dictionary<string, IChemistryElement>
            {
                {"V", new ChemistryElement{MolarMass=50.942M, Quantity=1M} }
            },
            MolQuantity = 2
        };

        var total = reactant.GramsProducedInAReaction(1540M, product);
        total = Math.Round(total, 4);
        Assert.Equal(862.6689M, total);
    }
}
