using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using GCore.Logging;
using GCore.AppSystem.Extensions;
using Microsoft.Extensions.Configuration;

namespace GCore.AppSystem.Handler;

[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class HandlerAttribute : Attribute
{
    public String ConfigName { get; private set; }
    public String DefaultImplementation { get;  private set; }

    public HandlerAttribute(string configName, string defaultImplementation)
    {
        ConfigName = configName;
        DefaultImplementation = defaultImplementation;
    }

    private static HandlerAttribute? GetAttributeFromType(Type type) => type.GetCustomAttribute<HandlerAttribute>();

    public static void BuildServiceHandlers(ContainerBuilder builder, IConfiguration config,
        IEnumerable<Assembly> assemblies)
    {
        foreach (var ass in assemblies)
        {
            foreach (var ia in ass.GetTypes().Select(t => new {T = t, A = GetAttributeFromType(t)})
                         .Where(t => t.A is not null))
            {
                var implementationName = config[ia.A?.ConfigName ?? ""] ?? 
                                         (ia.A?.DefaultImplementation.Contains('.') ?? true 
                                             ? ia.A?.DefaultImplementation 
                                             : ia.T.Namespace + "." + ia.A?.DefaultImplementation ) ?? "";

                Type? implementationType = null;

                foreach (var ass1 in assemblies)
                {
                    implementationType = ass1.GetTypes().FirstOrDefault(t =>
                        t.FullName == implementationName && !t.IsInterface && !t.IsAbstract);
                    if(implementationType is not null)
                        break;
                }

                if (implementationType is null)
                {
                    Log.Error($"Implementation '{implementationName}' for interface '{ia.T.FullName}' not found!");
                    continue;
                }

                var lifetime = LifetimeAttribute.GetLifetime(implementationType);

                if (lifetime == Lifetime.Default)
                    lifetime = LifetimeAttribute.GetLifetime(ia.T);

                switch (lifetime)
                {
                    case Lifetime.Singleton:
                        builder.AddSingleton(ia.T, implementationType);
                        break;
                    case Lifetime.Transient:
                        builder.AddTransient(ia.T, implementationType);
                        break;
                    case Lifetime.Scoped:
                        builder.AddScoped(ia.T, implementationType);
                        break;
                    case Lifetime.Default:
                        builder.Add(ia.T, implementationType, lifetime);
                        break;
                }

                Log.Info($"Added implementation '{implementationName}' as {lifetime} for interface '{ia.T.FullName}'");
            }
        }
    }
}