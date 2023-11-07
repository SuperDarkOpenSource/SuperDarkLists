namespace SuperdarkLists.DomainModel.Database.Model;

[Table("shopping_list_items")]
[PrimaryKey(nameof(ShoppingListItemId))]
[Index(nameof(ShoppingListId))]
public class ShoppingListItem
{
    [Column("shopping_list_item_id")]
    public Guid ShoppingListItemId { get; set; }
    
    [Column("shopping_list_id")]
    [ForeignKey(nameof(ShoppingList))]
    public Guid ShoppingListId { get; set; }
    
    public ShoppingList ShoppingList { get; set; } = null!;
    
    [Column("item_id")]
    [ForeignKey(nameof(Item))]
    public Guid ItemId { get; set; }

    [Column("amount")]
    public ulong Amount { get; set; }
    
    [Column("added_on")]
    public DateTime AddedOn { get; set; }
}