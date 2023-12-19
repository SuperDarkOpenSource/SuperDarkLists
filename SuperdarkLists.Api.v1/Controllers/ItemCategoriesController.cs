using System.Net;
using Microsoft.EntityFrameworkCore;
using SuperdarkLists.Common.Extensions;
using SuperdarkLists.DomainModel.Converters.ToRest;
using SuperdarkLists.DomainModel.Rest.ErrorModel;
using SuperdarkLists.DomainModel.Rest.Model;
using ItemCategory = SuperdarkLists.DomainModel.Database.Model.ItemCategory;

namespace SuperdarkLists.Api.v1;

public class ItemCategoriesController : BaseV1ApiController
{
    private DatabaseContext _db;

    public ItemCategoriesController(DatabaseContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DomainModel.Rest.Model.ItemCategory>>> GetAsync()
    {
        var list = await _db.Categories.Select(e => e.ToRestModel()).ToListAsync();

        return list;
    }

    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] ItemCategoryRequest body)
    {
        string? name = body.Name;
        
        // Make sure our input is good
        if (name.IsNullOrEmpty())
        {
            return BadRequest(new Error(ErrorCode.InvalidOrMissingParameter));
        }
        
        if (body.Id is not null)
        {
            // Ok they are trying to update the object, not create a new one
            ItemCategory? itemCategory = await _db.Categories.FirstOrDefaultAsync(e => e.CategoryId == body.Id);

            if (itemCategory is null)
            {
                return NotFound(new Error(ErrorCode.EntityDoesNotExist));
            }
            
            // When updating the name, verify it doesn't already exist
            if (await _db.Categories.AnyAsync(e => e.Name == body.Name))
            {
                return BadRequest(new Error(ErrorCode.InvalidOrMissingParameter));
            }
            
            // Update the entity
            itemCategory.Name = name;

            await _db.SaveChangesAsync();

            return Ok(itemCategory.ToRestModel());
        }
        else
        {
            // Try to find one in the database already
            if (await _db.Categories.AnyAsync(e => e.Name == name))
            {
                return BadRequest(new Error(ErrorCode.EntityAlreadyExists));
            }
        
            // Time to create one
            ItemCategory itemCategory = new()
            {
                CategoryId = Guid.NewGuid(),
                Name = name,
            };

            await _db.AddAsync(itemCategory);

            await _db.SaveChangesAsync();

            return Created(itemCategory.ToRestModel());
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DomainModel.Rest.Model.ItemCategory>> GetByIdAsync(Guid? id)
    {
        if (id is null)
        {
            return BadRequest(new Error(ErrorCode.InvalidOrMissingParameter));
        }

        ItemCategory? itemCategory = await _db.Categories.FirstOrDefaultAsync(e => e.CategoryId == id);
        if (itemCategory is null)
        {
            return NotFound(new Error(ErrorCode.EntityDoesNotExist));
        }

        return itemCategory.ToRestModel();
    }
}