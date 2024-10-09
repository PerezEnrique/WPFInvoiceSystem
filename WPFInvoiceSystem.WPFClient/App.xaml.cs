using Flurl.Http.Configuration;
using Microsoft.Extensions.Configuration;
using Prism.Ioc;
using System;
using System.Windows;
using WPFInvoiceSystem.WPFClient.Abstractions;
using WPFInvoiceSystem.WPFClient.DataProviders;
using WPFInvoiceSystem.WPFClient.Utils.Constants;
using WPFInvoiceSystem.WPFClient.Views;

namespace WPFInvoiceSystem.WPFClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //IConfiguration
            IConfiguration configuration = GetConfigurationService();
            containerRegistry.RegisterInstance<IConfiguration>(configuration);

            //Http client
            IFlurlClientCache flurlClientCache = GetFlurlClientCacheInstance(configuration);
            containerRegistry.RegisterInstance<IFlurlClientCache>(flurlClientCache);

            //Rest of services
            containerRegistry.Register<ICustomersProvider, CustomersProvider>();
            containerRegistry.Register<IInvoicesProvider, InvoicesProvider>();
            containerRegistry.Register<IServicesProvider, ServicesProvider>();
            containerRegistry.Register<IServiceTypesProvider, ServiceTypesProvider>();

            containerRegistry.RegisterForNavigation<HomeView>(name: ViewNames.HomeView);
        }

        private static IConfiguration GetConfigurationService()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        private static IFlurlClientCache GetFlurlClientCacheInstance(IConfiguration configuration)
        {
            string? baseUrl = configuration
                .GetValue<string>("ApiBaseUrl");

            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new Exception("Fatal Error: Base url is null or white space");

            return new FlurlClientCache()
                .Add(AppConstants.DefaultHttpClientName, baseUrl);
        }
    }
}
