using System;
using System.Reflection;

namespace GCore.AppSystem.Handler;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = true, AllowMultiple = false)]
public class LifetimeAttribute : Attribute
{
    public static Lifetime DEFAULT = Lifetime.Singleton;

    public Lifetime Attribute { get; private set; }

    public LifetimeAttribute(Lifetime attribute)
    {
        Attribute = attribute;
    }

    public static Lifetime GetLifetime(Type type) =>
        type.GetCustomAttribute<LifetimeAttribute>()?.Attribute ?? DEFAULT;
}