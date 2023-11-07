namespace SuperdarkLists.DomainModel.Database.Model;

[Table("item_categories")]
[PrimaryKey(nameof(CategoryId))]
public class ItemCategory
{
    [Column("category_id")]
    public Guid CategoryId { get; set; }
    
    [Column("name")]
    public string Name { get; set; } = String.Empty;
}