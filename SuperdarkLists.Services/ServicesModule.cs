using Autofac;
using SuperdarkLists.DomainModel.Services;
using SuperdarkLists.Services.Database;

namespace SuperdarkLists.Services;

public class ServicesModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ListDatabaseContextFactory>()
            .As<IListDatabaseContextFactory>()
            .SingleInstance()
            .WithParameter(new TypedParameter(typeof(string), "postgresConnectionString"));
    }
}