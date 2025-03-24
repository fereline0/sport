using System;
using System.IO;
using System.Net.Http;
using System.Windows;
using frontend.Services;
using frontend.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace frontend
{
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;
        private IConfiguration _configuration;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var environment =
                Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile(
                    $"appsettings.{environment}.json",
                    optional: true,
                    reloadOnChange: true
                )
                .Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            _serviceProvider = serviceCollection.BuildServiceProvider();

            var mainView = _serviceProvider.GetRequiredService<MainView>();
            mainView.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            var apiURI = _configuration["API:URI"];

            if (string.IsNullOrEmpty(apiURI))
            {
                throw new InvalidOperationException("API base URL is not configured.");
            }

            services.AddHttpClient(
                "DefaultAPIClient",
                client =>
                {
                    client.BaseAddress = new Uri(apiURI);
                }
            );

            services.AddTransient<AuthService>();
            services.AddTransient<UserService>();
            services.AddTransient<ProductService>();
            services.AddTransient<MainView>();
            services.AddTransient<LoginView>();
        }
    }
}
