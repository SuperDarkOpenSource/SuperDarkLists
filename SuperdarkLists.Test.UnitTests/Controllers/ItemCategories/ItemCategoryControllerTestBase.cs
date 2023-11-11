using SuperdarkLists.Api.v1;
using SuperdarkLists.DomainModel.Database.Model;
using SuperdarkLists.Test.Common.Database;

namespace SuperdarkLists.Test.UnitTests.Controllers.ItemCategories;

[Category(TestCategories.UnitTests)]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class ItemCategoryControllerTestBase
{
    protected MockDatabaseContext _mockDatabaseContext = new();

    protected ItemCategoriesController UnitUnderTest;
    
    [SetUp]
    public void Setup()
    {
        UnitUnderTest = new ItemCategoriesController(_mockDatabaseContext);
    }

    protected void GenerateFakeItemCategoryData(int amount)
    {
        for (var i = 0; i < amount; i++)
        {
            ItemCategory itemCategory = new()
            {
                CategoryId = Guid.NewGuid(),
                Name = $"Test_Category_{i}"
            };
            
            _mockDatabaseContext.BackingItemCategories.Add(itemCategory);
        }
    }

    protected bool IsRestModelItemCategoryInDatabase(DomainModel.Rest.Model.ItemCategory restModel)
    {
        ItemCategory? item =
            _mockDatabaseContext.BackingItemCategories.FirstOrDefault(e => e.CategoryId.Equals(restModel.Id));

        if (item is null)
        {
            return false;
        }

        return item.Name.Equals(restModel.Name);
    }
}