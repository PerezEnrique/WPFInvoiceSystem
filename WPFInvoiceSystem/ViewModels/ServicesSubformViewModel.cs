using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Domain.Modals;
using WPFInvoiceSystem.EventClasses;
using WPFInvoiceSystem.Utils.Constants;

namespace WPFInvoiceSystem.ViewModels
{
    public class ServicesSubformViewModel : BindableBase, INavigationAware
    {
        public readonly IDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;
        public DelegateCommand AddServiceCommand { get; }
        public DelegateCommand RemoveServiceCommand { get; }
        public DelegateCommand ShowCreateServiceFormCommand { get; }
        public DelegateCommand ShowServiceSearchCommand { get; }

        private Service? _createdOrSearchedService;
        public Service? CreatedOrSearchedService
        {
            get { return _createdOrSearchedService; }
            set { SetProperty(ref _createdOrSearchedService, value); }
        }

        private Invoice? _invoice;
        public Invoice? Invoice
        {
            get { return _invoice; }
            set { SetProperty(ref _invoice, value); }
        }

        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set { SetProperty(ref _quantity, value); }
        }

        private InvoiceService? _selectedInvoiceService;
        public InvoiceService? SelectedInvoiceService
        {
            get { return _selectedInvoiceService; }
            set { SetProperty(ref _selectedInvoiceService, value); }
        }

        private string _serviceAdditionError;
        public string ServiceAdditionError
        {
            get { return _serviceAdditionError; }
            set { SetProperty(ref _serviceAdditionError, value); }
        }


        public ServicesSubformViewModel(IEventAggregator eventAggregator, IDialogService dialogService)
        {
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;
            _serviceAdditionError = string.Empty;
            Quantity = 1;

            AddServiceCommand = new DelegateCommand(
                executeMethod: AddServiceToInvoice,
                canExecuteMethod: () => CreatedOrSearchedService != null && Quantity > 0
                )
                .ObservesProperty(() => CreatedOrSearchedService)
                .ObservesProperty(() => Quantity);

            RemoveServiceCommand = new DelegateCommand(
                executeMethod: RemoveServiceFromInvoice,
                canExecuteMethod: () => SelectedInvoiceService != null)
                .ObservesProperty(() => SelectedInvoiceService);

            ShowCreateServiceFormCommand = new DelegateCommand(ShowCreateService);

            ShowServiceSearchCommand = new DelegateCommand(ShowServiceSearch);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _invoice = navigationContext.Parameters.GetValue<Invoice>(ParamKeys.Invoice);
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        private void ShowCreateService()
        {
            _dialogService.ShowDialog(
                        DialogNames.ServiceFormDialog,
                        result => SetCreatedOrSearchedService(result));
        }

        private void ShowServiceSearch()
        {
            _dialogService.ShowDialog(
                        DialogNames.ServiceSearchDialog,
                        result => SetCreatedOrSearchedService(result));
        }

        private void SetCreatedOrSearchedService(IDialogResult result)
        {
            if (result.Result == ButtonResult.OK)
            {
                CreatedOrSearchedService = result.Parameters.GetValue<Service>(ParamKeys.Service);
                ServiceAdditionError = string.Empty;
            }
        }

        private void AddServiceToInvoice()
        {
            if (Invoice?.Services != null && CreatedOrSearchedService != null)
            {
                if (ServiceAlreadyInCollection())
                {
                    ServiceAdditionError = "Service already added";
                    return;
                }

                var newInvoiceService = new InvoiceService()
                {
                    Service = CreatedOrSearchedService,
                    Quantity = Quantity,
                };

                Invoice.Services.Add(newInvoiceService);
                CreatedOrSearchedService = null;
                OnCollectionModified();
            }
        }

        private bool ServiceAlreadyInCollection()
        {
            foreach (var item in Invoice!.Services)
            {
                if (item.Service.Id == CreatedOrSearchedService!.Id) return true;
            }

            return false;
        }

        protected void RemoveServiceFromInvoice()
        {
            if (Invoice?.Services != null && SelectedInvoiceService != null)
            {
                Invoice.Services.Remove(SelectedInvoiceService);
                SelectedInvoiceService = null;
                OnCollectionModified();
            }
        }

        private void OnCollectionModified()
        {
            _eventAggregator.GetEvent<InvoiceServicesModifiedEvent>()
                    .Publish();
        }
    }
}
