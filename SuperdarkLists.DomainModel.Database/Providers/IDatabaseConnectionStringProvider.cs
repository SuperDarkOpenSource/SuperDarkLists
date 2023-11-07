namespace SuperdarkLists.DomainModel.Database.Providers;

public interface IDatabaseConnectionStringProvider
{
    string DatabaseConnectionString { get; }
}