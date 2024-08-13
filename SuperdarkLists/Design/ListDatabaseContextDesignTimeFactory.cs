using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SuperdarkLists.DomainModel.Database;

namespace SuperdarkLists.Design;

public class ListDatabaseContextDesignTimeFactory : IDesignTimeDbContextFactory<ListDatabaseContext>
{
    public ListDatabaseContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ListDatabaseContext>();
        optionsBuilder.UseNpgsql();

        return new ListDatabaseContext(false, optionsBuilder.Options);
    }
}