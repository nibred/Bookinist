using Bookinist.Data;
using Bookinist.Services;
using Bookinist.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Bookinist
{
    public partial class App : Application
    {
        private static IHost _host;

        private static IHostBuilder CreateHostBuilder(string[] args) => Host
            .CreateDefaultBuilder(args)
            .ConfigureServices(ConfigureServices);

        public static bool IsDesignTime { get; private set; } = true;

        public static IServiceProvider Services => MainHost.Services;
        public static IHost MainHost => _host ??= CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

        internal static void ConfigureServices(HostBuilderContext host, IServiceCollection services) => services
            .AddDatabase(host.Configuration)
            .AddViewModels()
            .AddServices();

        protected override async void OnStartup(StartupEventArgs e)
        {
            IsDesignTime = false;

            var host = MainHost;
            base.OnStartup(e);
            await host.StartAsync();

            using var scope = Services.CreateScope();
            await scope.ServiceProvider.GetRequiredService<DbInitializer>().InitializeAsync();

        }
        protected override async void OnExit(ExitEventArgs e)
        {
            using var host = MainHost;
            base.OnExit(e);
            await host.StopAsync();
        }
    }
}
