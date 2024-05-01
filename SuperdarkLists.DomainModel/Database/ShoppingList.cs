using Microsoft.EntityFrameworkCore;

namespace SuperdarkLists.DomainModel.Database;

[PrimaryKey(nameof(Id))]
public class ShoppingList
{
    public Guid Id { get; }
    
    public string Name { get; set; }
    
    public List<ListItem> Items { get; set; }
}