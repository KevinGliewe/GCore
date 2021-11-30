using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Core;
using Autofac.Features.Decorators;

namespace GCore.AppSystem.Extensions;

public static class IContainerExtensions
{
    public static T GetService<T>(this IContainer self) where T: class
    {
        return self.Resolve<T>();
    }

    public static IEnumerable<Type> GetRegisteredImplementations(this IContainer self) 
        => self.ComponentRegistry.Registrations
            .Select(r => r.Activator.LimitType);

    public static IEnumerable<Type> GetRegisteredServices(this IContainer self)
    {
        foreach (var reg in self.ComponentRegistry.Registrations)
        {
            foreach (var service in reg.Services)
            {
                if (service is TypedService ds)
                    yield return ds.ServiceType;
            }
        }
    }

    public static IEnumerable<(Type Service, Type Implementation)> GetRegisteredServiceImplementations(this IContainer self)
    {
        foreach (var reg in self.ComponentRegistry.Registrations)
        {
            foreach (var service in reg.Services)
            {
                if (service is TypedService ds)
                    yield return new ValueTuple<Type, Type>{ Item1 = ds.ServiceType , Item2 = reg.Activator.LimitType};
            }
        }
    }


    public static object ResolveUnregistered(this IComponentContext context, Type serviceType, IEnumerable<Parameter> parameters)
    {
        var scope = context.Resolve<ILifetimeScope>();
        using (var innerScope = scope.BeginLifetimeScope(b => b.RegisterType(serviceType)))
        {
            var service = new TypedService(serviceType);

            IComponentRegistration? creg;
            innerScope.ComponentRegistry.TryGetRegistration(service, out creg);

            ServiceRegistration sreg;
            innerScope.ComponentRegistry.TryGetServiceRegistration(service, out sreg);

            var req = new ResolveRequest(service, sreg, parameters, creg);

            return context.ResolveComponent(req);
        }
    }

    public static object ResolveUnregistered(this IComponentContext context, Type serviceType)
    {
        return ResolveUnregistered(context, serviceType, Enumerable.Empty<Parameter>());
    }

    public static object ResolveUnregistered(this IComponentContext context, Type serviceType, params Parameter[] parameters)
    {
        return ResolveUnregistered(context, serviceType, (IEnumerable<Parameter>)parameters);
    }

    public static TService ResolveUnregistered<TService>(this IComponentContext context)
    {
        return (TService)ResolveUnregistered(context, typeof(TService), Enumerable.Empty<Parameter>());
    }

    public static TService ResolveUnregistered<TService>(this IComponentContext context, params Parameter[] parameters)
    {
        return (TService)ResolveUnregistered(context, typeof(TService), (IEnumerable<Parameter>)parameters);
    }
}