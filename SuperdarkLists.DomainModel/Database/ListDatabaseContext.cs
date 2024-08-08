using Microsoft.EntityFrameworkCore;

namespace SuperdarkLists.DomainModel.Database;

public class ListDatabaseContext : DbContext, IListDatabaseContext
{
    public ListDatabaseContext(bool isReadOnly, DbContextOptions<ListDatabaseContext> options) :
        base(options)
    {
    }

    public DbSet<ShoppingList> ShoppingLists { get; set; } = null!;

    public DbSet<ListItem> ListItems { get; set; } = null!;
    
    public bool IsReadOnly { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (IsReadOnly)
        {
            throw new InvalidOperationException("Cannot save readonly DatabaseContexts");
        }
        
        return base.SaveChangesAsync(cancellationToken);
    }
}