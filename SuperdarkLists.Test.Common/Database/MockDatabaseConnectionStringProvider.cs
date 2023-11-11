using SuperdarkLists.DomainModel.Database.Providers;

namespace SuperdarkLists.Test.Common.Database;

public class MockDatabaseConnectionStringProvider : IDatabaseConnectionStringProvider
{
    public string DatabaseConnectionString => "";
}