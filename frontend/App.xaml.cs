﻿using System;
using System.IO;
using System.Net.Http;
using System.Windows;
using frontend.Services;
using frontend.Utils;
using frontend.Utils.frontend;
using frontend.ViewModels;
using frontend.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace frontend
{
    public partial class App : Application
    {
        private IServiceProvider? ServiceProvider;
        private IConfiguration? Configuration;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            string? environment =
                Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile(
                    $"appsettings.{environment}.json",
                    optional: true,
                    reloadOnChange: true
                )
                .Build();

            ServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            Current.ShutdownMode = ShutdownMode.OnLastWindowClose;

            WindowManager windowManager = ServiceProvider.GetRequiredService<WindowManager>();
            windowManager.ShowWindow<MainWindow>();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration!);
            services.AddSingleton<TokenStorage>();

            string? apiURI = Configuration!["API:URI"];
            if (string.IsNullOrEmpty(apiURI))
            {
                throw new InvalidOperationException("API base URL is not configured.");
            }

            services.AddTransient<TokenHandler>();

            services
                .AddHttpClient("DefaultAPIClient")
                .AddHttpMessageHandler<TokenHandler>()
                .ConfigureHttpClient(client =>
                {
                    client.BaseAddress = new Uri(apiURI);
                });

            services.AddTransient<AuthService>();
            services.AddTransient<UserService>();
            services.AddTransient<OrderService>();
            services.AddTransient<OrderItemsService>();
            services.AddTransient<ProductService>();
            services.AddSingleton<NavigationService>();
            services.AddTransient<MainWindow>();
            services.AddTransient<HomePage>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<HomeViewModel>();
            services.AddTransient<ProductViewModel>();
            services.AddTransient<ProductPage>();
            services.AddTransient<ProductFormViewModel>();
            services.AddTransient<ProductFormPage>();
            services.AddTransient<OrderPage>();
            services.AddTransient<OrderPage>();
            services.AddTransient<PickupPointService>();
            services.AddTransient<OrderViewModel>();
            services.AddTransient<LoginWindow>();
            services.AddSingleton<WindowManager>();
        }
    }
}
