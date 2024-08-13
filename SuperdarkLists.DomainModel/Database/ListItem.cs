using Microsoft.EntityFrameworkCore;

namespace SuperdarkLists.DomainModel.Database;

[PrimaryKey(nameof(Id))]
public class ListItem
{
    public Guid Id { get; set; }
    
    public string DisplayName { get; set; }
    
    public ShoppingList ShoppingList { get; set; }
}