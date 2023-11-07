using Microsoft.Extensions.Configuration;
using SuperdarkLists.DomainModel.Database.Model;
using SuperdarkLists.DomainModel.Database.Providers;

namespace SuperdarkLists.DomainModel.Database;

public class DatabaseContext : DbContext
{
    private IDatabaseConnectionStringProvider _connectionStringProvider;
    
    public DatabaseContext(IDatabaseConnectionStringProvider connectionStringProvider)
    {
        _connectionStringProvider = connectionStringProvider;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionStringProvider.DatabaseConnectionString);
    }

    public virtual DbSet<Item> Items { get; set; } = null!;

    public virtual DbSet<ItemCategory> Categories { get; set; } = null!;

    public virtual DbSet<ShoppingList> ShoppingLists { get; set; } = null!;

    public virtual DbSet<ShoppingListItem> ShoppingListItems { get; set; } = null!;
}