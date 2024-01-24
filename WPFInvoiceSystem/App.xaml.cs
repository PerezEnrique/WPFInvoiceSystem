using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Prism.Ioc;
using System;
using System.IO;
using System.Text;
using System.Windows;
using WPFInvoiceSystem.Domain;
using WPFInvoiceSystem.Domain.Modals;
using WPFInvoiceSystem.Libraries;
using WPFInvoiceSystem.Persistance;
using WPFInvoiceSystem.Services;
using WPFInvoiceSystem.Utils.Constants;
using WPFInvoiceSystem.Validation;
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
            using (var dbContext = new AppDbContext())
            {
                dbContext.Database.Migrate();
            }

            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry
                .Register<IValidator<Customer>, CustomerValidator>();
            containerRegistry
                .Register<IValidator<Invoice>, InvoiceValidator>();
            containerRegistry
                .Register<IValidator<Service>, ServiceValidator>();

            containerRegistry
                .RegisterDialog<ConfirmOperationView, ConfirmOperationViewModel>(name: DialogNames.ConfirmOperationDialog);
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
                .RegisterSingleton<IReportsGenerator, ReportsGenerator>();
            containerRegistry
                .RegisterSingleton<IUnitOfWork, UnitOfWork>();
        }
    }
}
