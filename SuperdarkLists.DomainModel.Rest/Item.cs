namespace SuperdarkLists.DomainModel.Rest;

[JsonObject(ItemRequired = Required.Always)]
public class Item
{
    public Guid Id { get; set; }
}