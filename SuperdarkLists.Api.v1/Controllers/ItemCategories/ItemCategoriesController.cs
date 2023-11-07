using System.Net;
using Microsoft.EntityFrameworkCore;
using SuperdarkLists.Common.Extensions;
using SuperdarkLists.DomainModel.Converters.ToRest;
using SuperdarkLists.DomainModel.Database.Model;
using SuperdarkLists.DomainModel.Rest.ErrorModel;

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
    public async Task<ActionResult> PostAsync([FromForm] ItemCategoryFormData form)
    {
        string? name = form.Name;
        
        // Make sure our input is good
        if (name.IsNullOrEmpty())
        {
            return BadRequest(new Error(ErrorCode.InvalidOrMissingParameter));
        }
        
        // Try to find one in the database already
        ItemCategory? itemCategory = await _db.Categories.FirstOrDefaultAsync(e => e.Name == name);
        if (itemCategory is not null)
        {
            return BadRequest(new Error(ErrorCode.EntityAlreadyExists));
        }
        
        // Time to create one
        itemCategory = new()
        {
            CategoryId = Guid.NewGuid(),
            Name = name
        };

        await _db.AddAsync(itemCategory);

        await _db.SaveChangesAsync();

        return Created(itemCategory);
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