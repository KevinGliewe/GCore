using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;

namespace GCore.AppSystem;

public class AppSystemManagerBuilder : IAppSystemManagerBuilder
{
    protected List<Assembly> _assemblyList = new List<Assembly>();
    protected List<IAppSystemExtension> _extensions = new List<IAppSystemExtension>();

    public void AddScannableAssemblies(params Assembly[] assembly) => _assemblyList.AddRange(assembly);

    public void AddScannableAssemblies(IEnumerable<Assembly> assembly) => _assemblyList.AddRange(assembly);

    public void AddExtensions(params IAppSystemExtension[] extensions) => _extensions.AddRange(extensions);

    public void AddExtensions(IEnumerable<IAppSystemExtension> extensions) => _extensions.AddRange(extensions);

    public IAppSystemManager Build()
    {
        var appExtensions = _assemblyList.Concat(new[] { typeof(AppSystemManager).Assembly }).Distinct().Select(ass =>
            ass.GetTypes()
                .Where(ty => ty.IsAssignableTo<IAppSystemExtension>() && !ty.IsInterface && !ty.IsAbstract))
            .SelectMany(x => x).Distinct()
                .Where(ty => !_extensions.Select(e => e.GetType()).Contains(ty))
            .Select(ty => Activator.CreateInstance(ty) as IAppSystemExtension ?? throw new Exception("Error while activating AppExtension " + ty)); 
        
        var sys = new AppSystemManager(appExtensions, _assemblyList.Concat(new[] { typeof(AppSystemManager).Assembly }).Distinct());

        foreach (var appSystemExtension in appExtensions)
        {
            appSystemExtension.Dispose();
        }

        return sys;
    }  
}