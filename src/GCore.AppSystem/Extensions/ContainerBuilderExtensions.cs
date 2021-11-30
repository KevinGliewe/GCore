using System;
using Autofac;
using GCore.AppSystem.Handler;

namespace GCore.AppSystem.Extensions;

public static class ContainerBuilderExtensions
{
    public static ContainerBuilder Add(this ContainerBuilder self, Type service, Type implementation, Lifetime lifetime)
    {
        switch (lifetime)
        {
            case Lifetime.Singleton:
                self.AddSingleton(service, implementation);
                break;
            case Lifetime.Transient:
                self.AddTransient(service, implementation);
                break;
            case Lifetime.Scoped:
                self.AddScoped(service, implementation);
                break;
            case Lifetime.Default:
                self.Add(service, implementation, LifetimeAttribute.DEFAULT);
                break;
        }
        return self;
    }

    public static ContainerBuilder AddSingleton<T>(this ContainerBuilder self, T instance) where T : class
    {
        self.RegisterInstance(instance).As<T>().SingleInstance();
        return self;
    }

    public static ContainerBuilder AddSingleton(this ContainerBuilder self, object instance, Type service)
    {
        self.RegisterInstance(instance).As(service).SingleInstance();
        return self;
    }

    public static ContainerBuilder AddSingleton<T>(this ContainerBuilder self) where T : class
    {
        self.RegisterType<T>().SingleInstance();
        return self;
    }

    public static ContainerBuilder AddSingleton<TService, TImpl>(this ContainerBuilder self) where TService : class where TImpl : class
    {
        self.RegisterType<TImpl>().As<TService>().SingleInstance();
        return self;
    }

    public static ContainerBuilder AddSingleton(this ContainerBuilder self, Type service, Type implementation)
    {
        self.RegisterType(implementation).As(service).SingleInstance();
        return self;
    }

    public static ContainerBuilder AddTransient(this ContainerBuilder self, Type service, Type implementation)
    {
        self.RegisterType(implementation).As(service).InstancePerDependency();
        return self;
    }

    public static ContainerBuilder AddScoped(this ContainerBuilder self, Type service, Type implementation)
    {
        self.RegisterType(implementation).As(service).InstancePerLifetimeScope();
        return self;
    }
}