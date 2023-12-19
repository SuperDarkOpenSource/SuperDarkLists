namespace SuperdarkLists.DomainModel.Rest.Model;

[JsonObject]
public class ItemCategoryRequest
{
    public Guid? Id { get; set; }
    
    public string? Name { get; set; }
}