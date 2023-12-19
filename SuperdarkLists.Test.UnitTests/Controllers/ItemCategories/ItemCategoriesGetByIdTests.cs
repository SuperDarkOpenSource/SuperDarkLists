using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SuperdarkLists.DomainModel.Database.Model;
using SuperdarkLists.DomainModel.Rest.ErrorModel;

namespace SuperdarkLists.Test.UnitTests.Controllers.ItemCategories;

[TestFixture]
[Category(TestCategories.UnitTests)]
public class ItemCategoriesGetByIdTests : ItemCategoryControllerTestBase
{
    [Test]
    public async Task NullIdParmaterReturnsBadRequest()
    {
        Guid? testId = null;

        var result = await UnitUnderTest.GetByIdAsync(testId);

        result.Value.Should().BeNull();
        
        result.Result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().BeOfType<Error>()
            .Which.ErrorCode.Should().Be(ErrorCode.InvalidOrMissingParameter);

        _mockDatabaseContext.SaveChangesCalled.Should().BeFalse();
    }

    [Test]
    public async Task ItemCategoryMustExist()
    {
        _mockDatabaseContext.BackingItemCategories.Add(new ItemCategory()
        {
            CategoryId = Guid.NewGuid(),
            Name = "TestCategory"
        });

        var result = await UnitUnderTest.GetByIdAsync(Guid.Empty);

        result.Value.Should().BeNull();

        result.Result.Should().BeOfType<NotFoundObjectResult>()
            .Which.Value.Should().BeOfType<Error>()
            .Which.ErrorCode.Should().Be(ErrorCode.EntityDoesNotExist);

        _mockDatabaseContext.SaveChangesCalled.Should().BeFalse();
    }
}