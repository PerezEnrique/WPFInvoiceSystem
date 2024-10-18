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
            containerRegistry.Register<IReportsProvider, ReportsProvider>();
            containerRegistry.Register<IServicesProvider, ServicesProvider>();
            containerRegistry.Register<IServiceTypesProvider, ServiceTypesProvider>();
            containerRegistry.RegisterForNavigation<CustomersFormView>(name: ViewNames.CustomersFormView);
            containerRegistry.RegisterForNavigation<CustomersSearchView>(name: ViewNames.CustomersSearchView);
            containerRegistry.RegisterForNavigation<HomeView>(name: ViewNames.HomeView);
            containerRegistry.RegisterForNavigation<InvoiceDetailsView>(name: ViewNames.InvoiceDetailsView);
            containerRegistry.RegisterForNavigation<InvoicesFormView>(name: ViewNames.InvoicesFormView);
            containerRegistry.RegisterForNavigation<InvoicesView>(name: ViewNames.InvoicesView);
            containerRegistry.RegisterForNavigation<ServicesFormView>(name: ViewNames.ServicesFormView);
            containerRegistry.RegisterForNavigation<ServicesManagementView>(name: ViewNames.ServicesManagementView);
            containerRegistry.RegisterForNavigation<ServicesSearchView>(name: ViewNames.ServicesSearchView);
            containerRegistry.RegisterForNavigation<ServiceTypesManagementView>(name: ViewNames.ServiceTypesManagementView);
            containerRegistry.RegisterForNavigation<ServiceTypesFormView>(name: ViewNames.ServiceTypesFormView);
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
