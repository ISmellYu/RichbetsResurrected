using Autofac;
using AutoMapper;
using RichbetsResurrected.Entities.Profiles;

namespace RichbetsResurrected.Entities;

public class DefaultEntityModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(context => new MapperConfiguration(cfg =>
            {
                //Register Mapper Profile
                cfg.AddProfile<AutoMapperProfile>();
            }
        )).AsSelf().SingleInstance();

        builder.Register(c =>
            {
                //This resolves a new context that can be used later.
                var context = c.Resolve<IComponentContext>();
                var config = context.Resolve<MapperConfiguration>();
                return config.CreateMapper(context.Resolve);
            })
            .As<IMapper>()
            .InstancePerLifetimeScope();
    }
}
