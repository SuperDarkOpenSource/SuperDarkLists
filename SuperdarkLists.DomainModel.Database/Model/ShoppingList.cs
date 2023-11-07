using System.Collections.Generic;

namespace SuperdarkLists.DomainModel.Database.Model;

[Table("shopping_lists")]
[PrimaryKey(nameof(ShoppingListId))]
public class ShoppingList
{
    [Column("shopping_list_id")]
    public Guid ShoppingListId { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;
    
    [Column("created_on")]
    public DateTime CreatedOn { get; set; }
    
    [Column("last_updated")]
    public DateTime LastUpdated { get; set; }

    public ICollection<ShoppingListItem> Items { get; set; } = new List<ShoppingListItem>();
}