namespace SuperdarkLists.DomainModel.Rest.Model;

[JsonObject]
public class ItemCategory
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
}