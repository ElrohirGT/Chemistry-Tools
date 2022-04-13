using System;
using System.Linq;

using Chemistry_Tools.Core.Services.PeriodicTableService;
using Chemistry_Tools.Infrastructure.Services;

using Xunit;

namespace Chemistry_Tools.Infrastructure.Tests;
public class PeriodicTableServiceTests
{
    readonly IPeriodicTableService _table;

    public PeriodicTableServiceTests() => _table = new PeriodicTableService();

    [Theory]
    [InlineData("NaOH", 40)]
    [InlineData("H2O", 18.02)]
    [InlineData("W(H2O)2", 219.87)]
    [InlineData("Fe4(Fe(CN)6)3", 859.22)]
    public void CanCalculateMolarMassAccurately(string moleculeInText, decimal shouldBe)
    {
        var couldParseIt = _table.TryGetElementsOfMolecule(moleculeInText, out ChemistryElement[] elements);
        Assert.True(couldParseIt);

        decimal total = Math.Round(elements.Sum(el => el.MolarMass * el.Quantity), 2);
        Assert.InRange(total, shouldBe-0.02M, shouldBe+0.02M);
    }

    [Theory]
    [InlineData("NaOH", 3)]
    [InlineData("H2O", 2)]
    [InlineData("W(H2O)2", 3)]
    [InlineData("Fe4(Fe(CN)6)3", 3)]
    public void CanParseMoleculeElements(string moleculeInText, int elementCount)
    {
        var couldParseIt = _table.TryGetElementsOfMolecule(moleculeInText, out var elements);
        Assert.True(couldParseIt);

        Assert.Equal(elementCount, elements.Length);
    }

    [Theory]
    [InlineData("5Ca+V2O5->5CaO+2V")]
    [InlineData("5Ca + V2O5 -> 5CaO + 2V")]
    public void CanParseEquation(string equation)
    {
        var couldParseIt = _table.TryGetChemistryEquation(equation, out var reaction);
        Assert.True(couldParseIt);

        Assert.Equal(2, reaction.Reactants.Count);
        Assert.Equal(5, reaction.Reactants[0].MolQuantity);
        Assert.Equal(1, reaction.Reactants[1].MolQuantity);

        Assert.Equal(2, reaction.Products.Count);
        Assert.Equal(5, reaction.Products[0].MolQuantity);
        Assert.Equal(2, reaction.Products[1].MolQuantity);
    }
}