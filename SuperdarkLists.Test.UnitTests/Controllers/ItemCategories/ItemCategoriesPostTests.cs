using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SuperdarkLists.Api.v1;
using SuperdarkLists.DomainModel.Database.Model;
using SuperdarkLists.DomainModel.Rest.ErrorModel;

namespace SuperdarkLists.Test.UnitTests.Controllers.ItemCategories;

[TestFixture]
[Category(TestCategories.UnitTests)]
public class ItemCategoriesPostTests : ItemCategoryControllerTestBase
{
    [Test]
    public async Task EmptyNameInBodyReturnsBadRequest()
    {
        // Passing in string.Empty
        ItemCategoryFormData form = new()
        {
            Name = string.Empty
        };

        var result = await UnitUnderTest.PostAsync(form);

        // Should give us a result
        result.Should().NotBeNull();
        
        // And the result should be a BadRequest with an Error object and StatusCode of InvalidOrMissingParameter
        result.Should().BeOfType<BadRequestObjectResult>()
            .Subject.Value.Should().BeOfType<Error>()
            .Subject.ErrorCode.Should().Be(ErrorCode.InvalidOrMissingParameter);
        
        // Also the database shouldn't have been saved
        _mockDatabaseContext.SaveChangesCalled.Should().BeFalse();
    }
    
    [TestCase(null)]
    [TestCase("")]
    public async Task MissingNameInBodyReturnsBadRequest(string name)
    {
        // Null name or empty string
        ItemCategoryFormData form = new()
        {
            Name = name
        };

        var result = await UnitUnderTest.PostAsync(form);

        // Should yield an result
        result.Should().NotBeNull();
        
        // And a BadRequest of our Error object
        result.Should().BeOfType<BadRequestObjectResult>()
            .Subject.Value.Should().BeOfType<Error>()
            .Subject.ErrorCode.Should().Be(ErrorCode.InvalidOrMissingParameter);
        
        // Also the database shouldn't have been saved
        _mockDatabaseContext.SaveChangesCalled.Should().BeFalse();
    }

    [Test]
    public async Task MatchingNameInDatabaseYieldsBadRequest()
    {
        const string existingName = "TestItemCategory";
        
        // Insert an object into our "Database", Id doesn't matter but name should match our test
        _mockDatabaseContext.BackingItemCategories.Add(new ItemCategory()
        {
            CategoryId = Guid.Empty,
            Name = existingName
        });

        // Same name here
        ItemCategoryFormData form = new()
        {
            Name = existingName
        };

        var result = await UnitUnderTest.PostAsync(form);

        // Should return an BadRequest with our Error result and a code of EntityAlreadyExists
        result.Should().BeOfType<BadRequestObjectResult>()
            .Subject.Value.Should().BeOfType<Error>()
            .Subject.ErrorCode.Should().Be(ErrorCode.EntityAlreadyExists);
        
        // Also the database shouldn't have been saved
        _mockDatabaseContext.SaveChangesCalled.Should().BeFalse();
    }

    [Test]
    public async Task Works()
    {
        // We are going to create a new ItemCategory with this name
        const string testName = "TestName";

        ItemCategoryFormData form = new()
        {
            Name = testName
        };

        var result = await UnitUnderTest.PostAsync(form);

        // We should have created something
        result.Should().BeOfType<CreatedResult>();
        
        // Database should have been saved
        _mockDatabaseContext.SaveChangesCalled.Should().BeTrue();
        
        // Inside our Database we should have our new item
        _mockDatabaseContext.BackingItemCategories.Should().HaveCount(1)
            .And.OnlyContain(e => e.Name == testName);
    }
    
}