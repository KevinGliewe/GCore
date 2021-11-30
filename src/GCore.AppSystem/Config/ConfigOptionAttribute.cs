using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using GCore.Logging;
using GCore.AppSystem.Extensions;
using Microsoft.Extensions.Configuration;

namespace GCore.AppSystem.Config;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class ConfigOptionAttribute : Attribute
{
    public String ConfigName { get; private set; }

    public ConfigOptionAttribute(string configName)
    {
        ConfigName = configName;
    }

    private static ConfigOptionAttribute? GetAttributeFromType(Type type) =>
        type.GetCustomAttribute<ConfigOptionAttribute>();

    public static void BuildServiceOptions(ContainerBuilder builder, IConfiguration config,
        IEnumerable<Assembly> assemblies)
    {
        foreach (var ass in assemblies)
        {
            foreach (var ia in ass.GetTypes().Select(t => new {T = t, A = GetAttributeFromType(t)})
                         .Where(t => t.A is not null))
            {
                var sectionName = ia.A?.ConfigName ?? throw new Exception();
                var optionType = ia.T;

                object? option = null;

                try
                {
                    var section = config.GetSection(sectionName);

                    if (section.Exists())
                    {
                        option = section.Get(optionType) ??
                                 throw new ArgumentNullException(
                                     $"Option '{sectionName}' cant be casted to '{optionType}'");
                    }
                    else
                    {
                        Log.Warn($"Config section '{sectionName}' not found for {optionType}");

                        option = Activator.CreateInstance(optionType);
                    }


                    builder.AddSingleton(option ?? throw new Exception("Option in null"), optionType);

                }
                catch (Exception ex)
                {
                    Log.Exception(
                        $"Exception while building ConfigOption '{sectionName}' for type {optionType}", ex);
                }
            }
        }
    }
}