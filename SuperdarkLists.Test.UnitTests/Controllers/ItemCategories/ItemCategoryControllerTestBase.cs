using HttpContextMoq;
using HttpContextMoq.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SuperdarkLists.Api.v1;
using SuperdarkLists.DomainModel.Database.Model;
using SuperdarkLists.Test.Common.Database;

namespace SuperdarkLists.Test.UnitTests.Controllers.ItemCategories;

[Category(TestCategories.UnitTests)]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class ItemCategoryControllerTestBase
{
    protected string Url { get; set; } = "https://example.com/api/v1/item-category";
    protected MockDatabaseContext _mockDatabaseContext = new();
    protected ItemCategoriesController UnitUnderTest { get; private set; }
    
    [SetUp]
    public void Setup()
    {
        var mockHttpContext = new HttpContextMock();
        mockHttpContext.SetupUrl(Url);
        
        UnitUnderTest = new ItemCategoriesController(_mockDatabaseContext)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = mockHttpContext
            },
        };
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