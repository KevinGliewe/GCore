using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

using System.Reflection;
using Autofac;
using GCore.Logging;
using GCore.AppSystem.Extensions;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using GCore.AppSystem.Config;
using GCore.AppSystem.Handler;
using GCore.Messaging.TinyMessenger;

namespace GCore.AppSystem {
    public class AppSystemManager : IAppSystemManager {
        public IConfiguration Config { get; private set; }
        public IContainer Services { get; private set; }

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
                .AddSingleton<ITinyMessengerHub, TinyMessengerHub>()
                .Register(c => this.Services).As<IContainer>();
        }
    }
}