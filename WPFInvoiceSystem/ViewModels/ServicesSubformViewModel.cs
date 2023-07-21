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
using WPFInvoiceSystem.Domain.Entities;
using WPFInvoiceSystem.EventClasses;
using WPFInvoiceSystem.Utils.Constants;

namespace WPFInvoiceSystem.ViewModels
{
    public class ServicesSubformViewModel : BindableBase, INavigationAware
    {
        public readonly IDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;
        private Invoice? _invoice;
        public DelegateCommand AddServiceCommand { get; }
        public DelegateCommand RemoveServiceCommand { get; }
        public DelegateCommand ShowCreateServiceFormCommand { get; }
        public DelegateCommand ShowServiceSearchCommand { get; }
        public ObservableCollection<InvoiceService> InvoiceServices { get; private set; }
       
        private string _serviceAdditionError;
        public string ServiceAdditionError
        {
            get { return _serviceAdditionError; }
            set { SetProperty(ref _serviceAdditionError, value); }
        }

        //Service added or searched 
        private Service? _service;
        public Service? Service
        {
            get { return _service; }
            set { SetProperty(ref _service, value); }
        }

        private InvoiceService? _selectedInvoiceService;
        public InvoiceService? SelectedInvoiceService
        {
            get { return _selectedInvoiceService; }
            set { SetProperty(ref _selectedInvoiceService, value); }
        }

        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set { SetProperty(ref _quantity, value); }
        }


        public ServicesSubformViewModel(IEventAggregator eventAggregator, IDialogService dialogService)
        {
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;
            _serviceAdditionError = string.Empty;
            Quantity = 1;

            InvoiceServices = new ObservableCollection<InvoiceService>();
            
            AddServiceCommand = new DelegateCommand(
                executeMethod: AddService,
                canExecuteMethod: () => Service != null && Quantity > 0
                )
                .ObservesProperty(() => Service)
                .ObservesProperty(() => Quantity);

            RemoveServiceCommand = new DelegateCommand(
                executeMethod: RemoveService,
                canExecuteMethod: () => SelectedInvoiceService != null)
                .ObservesProperty(() => SelectedInvoiceService);

            ShowCreateServiceFormCommand = new DelegateCommand(ShowCreateService);

            ShowServiceSearchCommand = new DelegateCommand(ShowServiceSearch);

        }

        private void AddService()
        {
            if (Service != null)
            {
                if (ServiceAlreadyInCollection())
                {
                    ServiceAdditionError = "Service already added";
                    return;
                }

                var newInvoiceService = new InvoiceService()
                {
                    Service = Service, //Item shouldn't be null at this point. It's a requirement for triggering this method
                    Quantity = Quantity,
                };

                InvoiceServices.Add(newInvoiceService);
                Service = null;

                // Update the _invoice.InvoiceServices collection.
                if (_invoice != null) _invoice.Services = InvoiceServices;

                //Publish the event 
                _eventAggregator.GetEvent<InvoiceServicesModifiedEvent>()
                        .Publish();
            }
        }

        protected void RemoveService()
        {
            if (SelectedInvoiceService != null)
            {
                InvoiceServices.Remove(SelectedInvoiceService);
                SelectedInvoiceService = null;

                // Update the _invoice.InvoiceServices collection.
                if (_invoice != null) _invoice.Services = InvoiceServices;

                //Publish the event 
                _eventAggregator.GetEvent<InvoiceServicesModifiedEvent>()
                        .Publish();
            }
        }

        private void ShowCreateService()
        {
            _dialogService.ShowDialog(
                        DialogNames.ServiceFormDialog,
                        result => SetService(result));
        }

        private void ShowServiceSearch()
        {
            _dialogService.ShowDialog(
                        DialogNames.ServiceSearchDialog,
                        result => SetService(result));
        }

        private bool ServiceAlreadyInCollection()
        {
            foreach (var item in InvoiceServices)
            {
                if (item.Service.Id == Service!.Id) return true; //Item shouldn't be null at this point. It's a requirement for triggering this metho
            }

            return false;
        }

        private void SetService(IDialogResult result)
        {
            if (result.Result == ButtonResult.OK)
            {
                Service = result.Parameters.GetValue<Service>(ParamKeys.Service);
                ServiceAdditionError = string.Empty;
            }
        }

        //INavigationAware methods implementation
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _invoice = navigationContext.Parameters.GetValue<Invoice>(ParamKeys.Invoice);

            /*If the invoice from params has services attached to it, that collection is setted
             here to the InvoiceService property and then publish the event*/
            if (_invoice != null && _invoice.Services.Any())
            {
                InvoiceServices = _invoice.Services;

                _eventAggregator.GetEvent<InvoiceServicesModifiedEvent>()
                    .Publish();
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}
