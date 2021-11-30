using Autofac;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Reflection;
using GCore.AppSystem.WinForms.MainFormPlugins;

namespace GCore.AppSystem.WinForms;

public class AppSystemWinFormsExtension : IAppSystemExtension
{
    public void Dispose() { }

    public void ConfigBuild(IConfigurationBuilder builder, IEnumerable<Assembly> assemblies) { }

    public void ServiceBuild(IConfiguration config, ContainerBuilder builder, IEnumerable<Assembly> assemblies)
    {
        IMainFormPlugin.BuildServicePlugins(builder, config, assemblies);
    }
}