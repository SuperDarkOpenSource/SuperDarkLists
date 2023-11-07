using SuperdarkLists.DomainModel.Database.Providers;

namespace SuperdarkLists.Providers;

public class DatabaseConnectionStringProvider : IDatabaseConnectionStringProvider
{
    public DatabaseConnectionStringProvider(IConfiguration config)
    {
        string? connectionString = config["ConnectionStrings:SQL"];
        ArgumentNullException.ThrowIfNull(connectionString);

        DatabaseConnectionString = connectionString;
    }
    
    public string DatabaseConnectionString { get; }
}