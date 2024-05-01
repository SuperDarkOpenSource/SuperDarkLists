using Microsoft.EntityFrameworkCore;

namespace SuperdarkLists.DomainModel.Database;

public class ListDatabaseContext : DbContext
{
    public ListDatabaseContext(DbContextOptions<ListDatabaseContext> options) :
        base(options)
    {
    }

    public DbSet<ShoppingList> ShoppingLists = null!;

    public DbSet<ListItem> ListItems = null!;
}