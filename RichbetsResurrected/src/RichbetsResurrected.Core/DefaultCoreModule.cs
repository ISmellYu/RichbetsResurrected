﻿using Autofac;
using RichbetsResurrected.Core.Interfaces;
using RichbetsResurrected.Core.Services;

namespace RichbetsResurrected.Core;

public class DefaultCoreModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ToDoItemSearchService>()
            .As<IToDoItemSearchService>().InstancePerLifetimeScope();
        
    }
}