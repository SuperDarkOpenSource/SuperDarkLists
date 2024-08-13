using Microsoft.EntityFrameworkCore;

namespace SuperdarkLists.DomainModel.Database;

public interface IListDatabaseContext
{
    public DbSet<ShoppingList> ShoppingLists { get; set; }

    public DbSet<ListItem> ListItems { get; set; }
    
    public bool IsReadOnly { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}