using SuperdarkLists.DomainModel.Database;

namespace SuperdarkLists.DomainModel.Services;

public interface IListDatabaseContextFactory
{
    IListDatabaseContext GetDatabaseContext(bool isReadOnly = false);
}