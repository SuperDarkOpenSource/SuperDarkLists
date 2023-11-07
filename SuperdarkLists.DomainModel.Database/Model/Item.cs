namespace SuperdarkLists.DomainModel.Database.Model;

[Table("items")]
[PrimaryKey(nameof(ItemId))]
public class Item
{
    [Column("item_id")]
    public Guid ItemId { get; set; }

    [Column("name")] 
    public string Name { get; set; } = string.Empty;

    [ForeignKey(nameof(ItemCategory))]
    [Column("category_id")]
    public Guid CategoryId { get; set; }
}