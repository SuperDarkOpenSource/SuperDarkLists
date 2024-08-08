using Autofac;
using SuperdarkLists.Services;

namespace SuperdarkLists;

public class SuperdarkListsModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterModule<ServicesModule>();
    }
}