using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Microsoft.Extensions.Configuration;

namespace GCore.AppSystem;

public interface IAppSystemExtension : IDisposable
{
    void ConfigBuild(IConfigurationBuilder builder, IEnumerable<Assembly> assemblies);

    void ServiceBuild(IConfiguration config, ContainerBuilder builder, IEnumerable<Assembly> assemblies);
}