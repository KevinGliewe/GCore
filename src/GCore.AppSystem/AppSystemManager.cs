using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

using System.Reflection;
using Autofac;
using GCore.Logging;
using GCore.AppSystem.Extensions;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using GCore.AppSystem.Config;
using GCore.AppSystem.Handler;
using GCore.Extensions.AssemblyEx;
using GCore.Messaging.TinyMessenger;
using Zio;
using Zio.FileSystems;

namespace GCore.AppSystem {
    public class AppSystemManager : IAppSystemManager {
        public IConfiguration Config { get; private set; }
        public IContainer Services { get; private set; }
        public IFileSystem FileSystem { get; private set; }

        internal AppSystemManager(IEnumerable<IAppSystemExtension> appExtensions, IEnumerable<Assembly> assemblies)
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, args) => Log.Exception("Unhandled Exception", (Exception)args.ExceptionObject);

            // Initialize Configuration
            var configurationBuilder = new ConfigurationBuilder();
            ConfigBuild(configurationBuilder);
            foreach (var appSystemExtension in appExtensions)
                appSystemExtension.ConfigBuild(configurationBuilder, assemblies);
            Config = configurationBuilder.Build();

            Log.Debug($"Configuration:");
            foreach (var entry in Config.AsEnumerable())
                Log.Debug($"  {entry.Key} = {entry.Value}");


            {
                // Initialize FileSystem
                var mfs = new MountFileSystem();
                var pfs = new PhysicalFileSystem();
                mfs.Mount("/cd", new SubFileSystem(pfs, pfs.ConvertPathFromInternal(Directory.GetCurrentDirectory())));
                mfs.Mount("/exe", new SubFileSystem(pfs, pfs.ConvertPathFromInternal(Assembly.GetExecutingAssembly().GetBetterLocationDir())));
                FileSystem = mfs;
            }

            // Initialize Services
            var serviceCollection = new ContainerBuilder();
            ServiceBuild(serviceCollection);
            ConfigOptionAttribute.BuildServiceOptions(serviceCollection, Config, assemblies);
            HandlerAttribute.BuildServiceHandlers(serviceCollection, Config, assemblies);
            foreach (var appSystemExtension in appExtensions)
                appSystemExtension.ServiceBuild(Config, serviceCollection, assemblies);
            Services = serviceCollection.Build();

            Log.Debug($"Services:");
            foreach (var registeredService in Services.GetRegisteredServiceImplementations())
                Log.Debug($"  {registeredService.Service.FullName} => {registeredService.Implementation.FullName}");
        }

        private void ConfigBuild(IConfigurationBuilder builder) {

        }

        private void ServiceBuild(ContainerBuilder builder) {
            builder
                .AddSingleton(Config)
                .AddSingleton(this)
                .AddSingleton<IFileSystem>(FileSystem)
                .AddSingleton<ITinyMessengerHub, TinyMessengerHub>()
                .Register(c => this.Services).As<IContainer>();
        }
    }
}