using Autofac;

namespace SuperdarkLists.DomainModel.Database;

public class DatabaseModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<DatabaseContext>().InstancePerLifetimeScope();
    }
}