using FluentValidation;
using Prism.Ioc;
using System.Windows;
using WPFInvoiceSystem.Domain;
using WPFInvoiceSystem.Domain.Entities;
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
                .Register<IValidator<Invoice>, InvoiceValidator>();
            containerRegistry
                .Register<IValidator<Customer>, CustomerValidator>();

            containerRegistry
                .RegisterDialog<CustomerFormView, CustomerFormViewModel>(name: DialogNames.CustomersFormDialog);
            containerRegistry
                .RegisterDialog<CustomersSearchView, CustomersSearchViewModel>(name: DialogNames.CustomersSearchDialog);

            containerRegistry
                .RegisterForNavigation<InvoicesListView, InvoicesListViewModel>(name: ViewNames.InvoicesListView);
            containerRegistry
                .RegisterForNavigation<InvoiceFormView, InvoiceFormViewModel>(name: ViewNames.InvoiceFormView);
            containerRegistry
                .RegisterForNavigation<InvoiceMetadataSubformView, InvoiceMetadataSubformViewModel>(name: ViewNames.InvoiceMetadataSubform);
            containerRegistry
                .RegisterForNavigation<CustomersSubformView, CustomersSubformViewModel>(name: ViewNames.CustomerSubformView);
            
            containerRegistry
                .RegisterSingleton<IUnitOfWork, UnitOfWork>();
        }
    }
}
