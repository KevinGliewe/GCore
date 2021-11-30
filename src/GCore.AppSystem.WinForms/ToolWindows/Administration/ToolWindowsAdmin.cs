using System;
using GCore.Data;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Autofac;
using Autofac.Core;
using GCore.AppSystem.Extensions;

namespace GCore.AppSystem.WinForms.ToolWindows.Administration;

public class ToolWindowsAdmin : IToolWindowsAdmin
{
    private IContainer _serviceProvider { get; set; }

    private IConfiguration _configuration { get; set; }

    public IReadOnlyDictionary<String, Type> ToolWindowImplementations { get; private set; }



    public ToolWindowsAdmin(IContainer serviceProvider, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;

        var twi = new Dictionary<String, Type>();

        foreach (var service in _serviceProvider.GetRegisteredServices())
        {
            if (service.IsAssignableTo(typeof(IToolWindowBase)) && service.GetCustomAttribute<ToolWindowAttribute>() is not null)
            {
                twi.Add(service.GetCustomAttribute<ToolWindowAttribute>()?.Name ?? throw new Exception(), service);
            }
        }

        ToolWindowImplementations = twi;
    }

    #region IReadOnlyDictionary<String, Type> -----------------------------------------------------------------------------------------

    public IEnumerable<string> Keys => ToolWindowImplementations.Keys;

    public IEnumerable<Type> Values => ToolWindowImplementations.Values;

    public int Count => ToolWindowImplementations.Count;

    public Type this[string key] => ToolWindowImplementations[key];

    public bool ContainsKey(string key) => ToolWindowImplementations.ContainsKey(key);

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out Type value) =>
        ToolWindowImplementations.TryGetValue(key, out value);

    public IEnumerator<KeyValuePair<string, Type>> GetEnumerator() => ToolWindowImplementations.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ToolWindowImplementations.GetEnumerator();
    #endregion

    #region IToolWindowsAdmin ---------------------------------------------------------------------------------------------------------
    public ToolWindowBase GetToolWindow(string name) 
        => _serviceProvider.Resolve(this[name]) as ToolWindowBase ?? throw new Exception();

    public ToolWindowBase GetToolWindow(string name, IEnumerable<Parameter> parameters)
        => _serviceProvider.Resolve(this[name], parameters) as ToolWindowBase ?? throw new Exception();
    public ToolWindowBase GetToolWindow(string name, params Parameter[] parameters)
        => GetToolWindow(name, (IEnumerable<Parameter>)parameters);

    #endregion
}