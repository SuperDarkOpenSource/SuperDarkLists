using Autofac;
using SuperdarkLists.DomainModel.Database;
using SuperdarkLists.DomainModel.Database.Providers;
using SuperdarkLists.Providers;

namespace SuperdarkLists;

public class SuperdarkListsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<DatabaseConnectionStringProvider>().As<IDatabaseConnectionStringProvider>()
            .SingleInstance();
        
        builder.RegisterModule(new DatabaseModule());
    }
}