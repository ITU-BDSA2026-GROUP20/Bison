using Bison.Core.models;

namespace Bison.Razor.Tests;

public class FilterTest
{
    private static readonly Taxon Animal = new() { TaxonId = 1, VernacularName = "Animal" };
    private static readonly Taxon Tiger  = new() { TaxonId = 2, VernacularName = "Tiger", Parent = Animal };
    private static readonly Taxon Cat    = new() { TaxonId = 3, VernacularName = "Cat",   Parent = Tiger };
    private static readonly Taxon Bird   = new() { TaxonId = 4, VernacularName = "Bird",  Parent = Animal };
    
    [Fact]
    public void TaxonIsSubTaxonOfItself()
    {
        Assert.True(Cat.IsSubTaxonOf(Cat));
    }

    [Fact]
    public void TaxonIsSubTaxonOfParent()
    {
        Assert.True(Cat.IsSubTaxonOf(Tiger));
    }

    [Fact]
    public void TaxonIsSubTaxonOfGrandparent()
    {
        Assert.True(Cat.IsSubTaxonOf(Animal));
    }

    [Fact]
    public void ParentIsNotSubTaxonOfChild()
    {
        Assert.False(Tiger.IsSubTaxonOf(Cat));
    }

    [Fact]
    public void SiblingIsNotSubTaxon()
    {
        Assert.False(Bird.IsSubTaxonOf(Tiger));
    }
}