using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SuperdarkLists.Api.v1;
using SuperdarkLists.DomainModel.Rest.ErrorModel;
using SuperdarkLists.DomainModel.Rest.Model;
using ItemCategory = SuperdarkLists.DomainModel.Database.Model.ItemCategory;

namespace SuperdarkLists.Test.UnitTests.Controllers.ItemCategories;

[TestFixture]
[Category(TestCategories.UnitTests)]
public class ItemCategoriesPostTests : ItemCategoryControllerTestBase
{
    [Test]
    public async Task EmptyNameInBodyReturnsBadRequestWhenCreating()
    {
        // Passing in string.Empty
        ItemCategoryRequest request = new()
        {
            Name = string.Empty
        };

        var result = await UnitUnderTest.PostAsync(request);

        // Should give us a result
        result.Should().NotBeNull();
        
        // And the result should be a BadRequest with an Error object and StatusCode of InvalidOrMissingParameter
        result.Should().BeOfType<BadRequestObjectResult>()
            .Subject.Value.Should().BeOfType<Error>()
            .Subject.ErrorCode.Should().Be(ErrorCode.InvalidOrMissingParameter);
        
        // Also the database shouldn't have been saved
        _mockDatabaseContext.SaveChangesCalled.Should().BeFalse();
    }

    [Test]
    public async Task EmptyNameReturnsBadRequestWhenUpdating()
    {
        Guid existingId = Guid.NewGuid();
        const string existingName = "ItemCategory A";
        
        // Add an existing item into our Database
        _mockDatabaseContext.BackingItemCategories.Add(new ItemCategory()
        {
            CategoryId = existingId,
            Name = existingName
        });
        
        // Try to update with an empty name
        ItemCategoryRequest request = new()
        {
            Id = existingId,
            Name = string.Empty,
        };

        var result = await UnitUnderTest.PostAsync(request);

        result.Should().BeOfType<BadRequestObjectResult>()
            .Subject.Value.Should().BeOfType<Error>()
            .Subject.ErrorCode.Should().Be(ErrorCode.InvalidOrMissingParameter);
    }

    [TestCase(null)]
    [TestCase("")]
    public async Task MissingNameInUpdateRequestReturnsBadRequest(string name)
    {
        Guid existingId = Guid.NewGuid();
        const string existingName = "ItemCategory A";
        
        // Add an existing item into our Database
        _mockDatabaseContext.BackingItemCategories.Add(new ItemCategory()
        {
            CategoryId = existingId,
            Name = existingName
        });
        
        // Try to update with an empty name
        ItemCategoryRequest request = new()
        {
            Id = existingId,
            Name = name,
        };

        var result = await UnitUnderTest.PostAsync(request);

        result.Should().BeOfType<BadRequestObjectResult>()
            .Subject.Value.Should().BeOfType<Error>()
            .Subject.ErrorCode.Should().Be(ErrorCode.InvalidOrMissingParameter);
    }
    
    [TestCase(null)]
    [TestCase("")]
    public async Task MissingNameInBodyReturnsBadRequestWhenCreating(string name)
    {
        // Null name or empty string
        ItemCategoryRequest request = new()
        {
            Name = name
        };

        var result = await UnitUnderTest.PostAsync(request);

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
    public async Task MatchingNameInDatabaseYieldsBadRequestWhenCreating()
    {
        const string existingName = "TestItemCategory";
        
        // Insert an object into our "Database", Id doesn't matter but name should match our test
        _mockDatabaseContext.BackingItemCategories.Add(new ItemCategory()
        {
            CategoryId = Guid.Empty,
            Name = existingName
        });

        // Same name here
        ItemCategoryRequest request = new()
        {
            Name = existingName
        };

        var result = await UnitUnderTest.PostAsync(request);

        // Should return an BadRequest with our Error result and a code of EntityAlreadyExists
        result.Should().BeOfType<BadRequestObjectResult>()
            .Subject.Value.Should().BeOfType<Error>()
            .Subject.ErrorCode.Should().Be(ErrorCode.EntityAlreadyExists);
        
        // Also the database shouldn't have been saved
        _mockDatabaseContext.SaveChangesCalled.Should().BeFalse();
    }

    [Test]
    public async Task CreatingNewItemCategoryWorks()
    {
        // We are going to create a new ItemCategory with this name
        const string testName = "TestName";

        ItemCategoryRequest request = new()
        {
            Name = testName
        };

        var result = await UnitUnderTest.PostAsync(request);

        // We should have created something
        result.Should().BeOfType<CreatedResult>();
        
        // Database should have been saved
        _mockDatabaseContext.SaveChangesCalled.Should().BeTrue();
        
        // Inside our Database we should have our new item
        _mockDatabaseContext.BackingItemCategories.Should().HaveCount(1)
            .And.OnlyContain(e => e.Name == testName);
    }
    
    [Test]
    public async Task ExistingNameInDatabasePreventsUpdate()
    {
        Guid existingCategoryId = Guid.NewGuid();
        const string existingName = "TestCategory";
        
        // Add an ItemCategory to our database
        _mockDatabaseContext.BackingItemCategories.Add(new ItemCategory()
        {
            CategoryId = existingCategoryId,
            Name = existingName
        });
        
        // We want to update an item using the existing name
        const string newName = existingName;
        
        // Create the request using the existing Id which will update our ItemCategory
        ItemCategoryRequest request = new()
        {
            Id = existingCategoryId,
            Name = newName
        };

        var result = await UnitUnderTest.PostAsync(request);

        // Result should be correct
        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().BeOfType<Error>()
            .Which.ErrorCode.Should().Be(ErrorCode.InvalidOrMissingParameter);
        
        // Our Database should not be modified
        _mockDatabaseContext.SaveChangesCalled.Should().BeFalse();
    }

    [Test]
    public async Task UpdatingExitingItemCategoryWorks()
    {
        Guid existingCategoryId = Guid.NewGuid();
        const string existingName = "TestCategory";
        
        // Add an ItemCategory to our database
        _mockDatabaseContext.BackingItemCategories.Add(new ItemCategory()
        {
            CategoryId = existingCategoryId,
            Name = existingName
        });
        
        // We want to update the item to a new name
        const string newName = "New TestCategory Name";
        
        // Create the request using the existing Id which will update our ItemCateogry
        ItemCategoryRequest request = new()
        {
            Id = existingCategoryId,
            Name = newName
        };

        var result = await UnitUnderTest.PostAsync(request);

        // Result should be correct
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<DomainModel.Rest.Model.ItemCategory>()
            .Subject.Should().BeEquivalentTo(new DomainModel.Rest.Model.ItemCategory()
            {
                Id = existingCategoryId,
                Name = newName
            });
        
        // Our database should be updated
        _mockDatabaseContext.BackingItemCategories.Should()
            .OnlyContain(e => e.CategoryId == existingCategoryId && e.Name == newName);
        _mockDatabaseContext.SaveChangesCalled.Should().BeTrue();
    }
    
}