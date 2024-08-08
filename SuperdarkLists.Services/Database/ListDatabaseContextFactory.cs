using Microsoft.EntityFrameworkCore;
using SuperdarkLists.DomainModel.Database;
using SuperdarkLists.DomainModel.Services;

namespace SuperdarkLists.Services.Database;

public class ListDatabaseContextFactory : IListDatabaseContextFactory
{
    private readonly string _postgresConnectionString;

    public ListDatabaseContextFactory(string postgresConnectionString)
    {
        _postgresConnectionString = postgresConnectionString;
    }
    
    public IListDatabaseContext GetDatabaseContext(bool isReadOnly = false)
    {
        var builder = new DbContextOptionsBuilder<ListDatabaseContext>();
        builder.UseNpgsql(_postgresConnectionString);
        
        return new ListDatabaseContext(isReadOnly, builder.Options);
    }
}