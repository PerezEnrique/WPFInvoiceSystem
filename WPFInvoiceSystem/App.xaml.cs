using FluentValidation;
using Prism.Ioc;
using System.Windows;
using WPFInvoiceSystem.Domain;
using WPFInvoiceSystem.Domain.Modals;
using WPFInvoiceSystem.Persistance.Repositories;
using WPFInvoiceSystem.Utils.Constants;
using WPFInvoiceSystem.Utils.Validation;
using WPFInvoiceSystem.ViewModels;
using WPFInvoiceSystem.Views;

namespace WPFInvoiceSystem
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
            containerRegistry
                .Register<IValidator<Customer>, CustomerValidator>();
            containerRegistry
                .Register<IValidator<Invoice>, InvoiceValidator>();

            containerRegistry
                .RegisterDialog<CustomerFormView, CustomerFormViewModel>(name: DialogNames.CustomersFormDialog);
            containerRegistry
                .RegisterDialog<CustomersSearchView, CustomersSearchViewModel>(name: DialogNames.CustomersSearchDialog);
            containerRegistry
                .RegisterDialog<ServiceFormView, ServiceFormViewModel>(name: DialogNames.ServiceFormDialog);
            containerRegistry
                .RegisterDialog<ServiceSearchView, ServiceSearchViewModel>(name: DialogNames.ServiceSearchDialog);

            containerRegistry
                .RegisterForNavigation<CustomersSubformView, CustomersSubformViewModel>(name: ViewNames.CustomerSubformView);
            containerRegistry
                .RegisterForNavigation<InvoiceFormView, InvoiceFormViewModel>(name: ViewNames.InvoiceFormView);
            containerRegistry
                .RegisterForNavigation<InvoiceMetadataSubformView, InvoiceMetadataSubformViewModel>(name: ViewNames.InvoiceMetadataSubform);
            containerRegistry
                .RegisterForNavigation<InvoiceSearchView, InvoiceSearchViewModel>(name: ViewNames.InvoiceSearchView);
            containerRegistry
                .RegisterForNavigation<InvoicesListView, InvoicesListViewModel>(name: ViewNames.InvoicesListView);
            containerRegistry
                .RegisterForNavigation<ServiceSubformView, ServicesSubformViewModel>(name: ViewNames.ServiceSubformView);
            
            containerRegistry
                .RegisterSingleton<IUnitOfWork, UnitOfWork>();
        }
    }
}
