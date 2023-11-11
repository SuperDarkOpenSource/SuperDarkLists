using FluentAssertions;

namespace SuperdarkLists.Test.UnitTests.Controllers.ItemCategories;

[TestFixture]
public class ItemCategoriesGetTests : ItemCategoryControllerTestBase
{
    [Test]
    public async Task ReturnsEmptyListWhenTableIsEmpty()
    {
        // Empty repository of ItemCategories
        
        var result = await UnitUnderTest.GetAsync();

        result.Should().NotBeNull();
        result.Result.Should().BeNull();
        result.Value.Should().NotBeNull("Result should be a default 200.")
            .And.BeEmpty("There should be nothing in the repo");
    }

    [TestCase(1)]
    [TestCase(10)]
    [TestCase(100)]
    public async Task ReturnsOnlyEntitiesInDatabase(int amountToAdd)
    {
        // Add some stuff to the database
        GenerateFakeItemCategoryData(amountToAdd);

        var result = await UnitUnderTest.GetAsync();

        // Was this ASP.NET controller binding a success?
        result.Should().NotBeNull();
        result.Result.Should().BeNull();
        
        // Did it return a list?
        result.Value.Should().NotBeNullOrEmpty();
        result.Value.Should().OnlyContain(e => IsRestModelItemCategoryInDatabase(e));
    }
}