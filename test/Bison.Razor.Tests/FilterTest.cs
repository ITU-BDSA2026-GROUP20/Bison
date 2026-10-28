using Bison.Core.models;

namespace Bison.Razor.Tests;
/*
When the filter service has been implemented in MyChat.Razor service, we need to refactor the test.
This is bare minimum test to ensure that the IsSubTaxonOf method works as expected.
    */
public class FilterTest
{
    private static readonly Taxon Animal = new() { TaxonId = 1, VernacularName = "Animal" };
    private static readonly Taxon Tiger  = new() { TaxonId = 2, VernacularName = "Tiger", Parent = Cat };
    private static readonly Taxon Cat    = new() { TaxonId = 3, VernacularName = "Cat",   Parent = Animal };
    private static readonly Taxon Bird   = new() { TaxonId = 4, VernacularName = "Bird",  Parent = Animal };

    [Fact]
    public void TaxonIsSubTaxonOfItself()
    {
        Assert.True(Cat.IsSubTaxonOf(Cat));
    }

    [Fact]
    public void TaxonIsSubTaxonOfParent()
    {
        Assert.True(Tiger.IsSubTaxonOf(Cat));
    }

    [Fact]
    public void TaxonIsSubTaxonOfGrandparent()
    {
        Assert.True(Cat.IsSubTaxonOf(Animal));
    }

    [Fact]
    public void ParentIsNotSubTaxonOfChild()
    {
        Assert.False(Cat.IsSubTaxonOf(Tiger));
    }

    [Fact]
    public void SiblingIsNotSubTaxon()
    {
        Assert.False(Bird.IsSubTaxonOf(Tiger));
    }
}