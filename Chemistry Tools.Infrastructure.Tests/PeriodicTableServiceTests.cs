using System;
using System.Linq;

using Chemistry_Tools.Core.Services.PeriodicTableService;
using Chemistry_Tools.Infrastructure.Services;

using Xunit;

namespace Chemistry_Tools.Infrastructure.Tests;
public class PeriodicTableServiceTests
{
    [Theory]
    [InlineData("NaOH", 40)]
    [InlineData("H2O", 18.02)]
    [InlineData("W(H2O)2", 219.87)]
    [InlineData("Fe4(Fe(CN)6)3", 859.22)]
    public void CanParseMolecule(string moleculeInText, decimal shouldBe)
    {
        var table = new PeriodicTableService();
        var couldParseIt = table.TryGetElementsOfMolecule(moleculeInText, out ChemistryElement[] elements);
        Assert.True(couldParseIt);

        decimal total = Math.Round(elements.Sum(el => el.MolarMass * el.Quantity), 2);
        Assert.InRange(total, shouldBe-0.2M, shouldBe+0.2M);
    }
}