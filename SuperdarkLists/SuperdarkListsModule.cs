using Autofac;
using Microsoft.EntityFrameworkCore;
using SuperdarkLists.DomainModel.Database;

namespace SuperdarkLists;

public class SuperdarkListsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.Register<ListDatabaseContext>(context =>
            {
                var config = context.Resolve<IConfiguration>();

                var optionsBuilder = new DbContextOptionsBuilder<ListDatabaseContext>();
                optionsBuilder.UseNpgsql(config.GetConnectionString("Database"));

                return new ListDatabaseContext(optionsBuilder.Options);
            })
            .AsSelf()
            .InstancePerLifetimeScope();
    }
}