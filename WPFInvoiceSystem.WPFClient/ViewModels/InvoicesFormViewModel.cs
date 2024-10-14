using ImTools;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WPFInvoiceSystem.API.ApiResources;
using WPFInvoiceSystem.WPFClient.Abstractions;
using WPFInvoiceSystem.WPFClient.Events;
using WPFInvoiceSystem.WPFClient.Exceptions;
using WPFInvoiceSystem.WPFClient.Models;
using WPFInvoiceSystem.WPFClient.Utils.Constants;
using WPFInvoiceSystem.WPFClient.Utils.Enums;

namespace WPFInvoiceSystem.WPFClient.ViewModels
{
    public class InvoicesFormViewModel : BindableBase, INavigationAware
    {
        private readonly IInvoicesProvider _internalInvoicesProvider;
        private readonly ICustomersProvider _customersProvider;
        private readonly IServicesProvider _servicesProvider;
        private readonly IRegionManager _regionManager;
        private InvoiceModel? _invoiceToBeUpdated;
        private IRegionNavigationJournal? _navigationJournal;
        public ObservableCollection<string> Errors { get; }
        public ObservableCollection<InvoiceServiceModel> InvoiceServices { get; }

        private ActionsOnSubmit? _actionOnSubmit;
        public ActionsOnSubmit? ActionOnSubmit
        {
            get { return _actionOnSubmit; }
            private set { SetProperty(ref _actionOnSubmit, value); }
        }

        private ServiceModel? _createdOrSearchedService;
        public ServiceModel? CreatedOrSearchedService
        {
            get { return _createdOrSearchedService; }
            set { SetProperty(ref _createdOrSearchedService, value); }
        }

        private int _createdOrSearchedServiceQuantity;
        public int CreatedOrSearchedServiceQuantity
        {
            get { return _createdOrSearchedServiceQuantity; }
            set { SetProperty(ref _createdOrSearchedServiceQuantity, value); }
        }

        private CustomerModel? _customer;
        public CustomerModel? Customer
        {
            get { return _customer; }
            set { SetProperty(ref _customer, value); }
        }

        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }

        private decimal _exempt;
        public decimal Exempt
        {
            get { return _exempt; }
            set { SetProperty(ref _exempt, value); }
        }

        private int _invoiceNumber;
        public int InvoiceNumber
        {
            get { return _invoiceNumber; }
            set { SetProperty(ref _invoiceNumber, value); }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }


        private InvoiceServiceModel? _selectedInvoiceService;
        public InvoiceServiceModel? SelectedInvoiceService
        {
            get { return _selectedInvoiceService; }
            set { SetProperty(ref _selectedInvoiceService, value); }
        }

        private decimal _tax;
        public decimal Tax
        {
            get { return _tax; }
            set { SetProperty(ref _tax, value); }
        }

        private decimal _taxBase;
        public decimal TaxBase
        {
            get { return _taxBase; }
            set { SetProperty(ref _taxBase, value); }
        }

        public DelegateCommand AddServiceToInvoiceCommand { get; }
        public DelegateCommand GoBackCommand { get; }
        public DelegateCommand GoToCustomersCreationFormCommand { get; }
        public DelegateCommand GoToCustomersModificationFormCommand { get; }
        public DelegateCommand GoToCustomersSearchCommand { get; }
        public DelegateCommand GoToServicesCreationFormCommand { get; }
        public DelegateCommand GoToServicesSearchCommand { get; }
        public DelegateCommand RemoveServiceFromInvoiceCommand { get; }

        public DelegateCommand SubmitInvoiceCommand { get; }

        public InvoicesFormViewModel(
            IInvoicesProvider invoicesProvider,
            ICustomersProvider customersProvider,
            IServicesProvider serviceProvider,
            IRegionManager regionManager,
            IEventAggregator eventAggregator
            )
        {
            _customersProvider = customersProvider;
            _date = DateTime.Now;
            _internalInvoicesProvider = invoicesProvider;
            _servicesProvider = serviceProvider;
            _regionManager = regionManager;

            Errors = new ObservableCollection<string>();
            InvoiceServices = new ObservableCollection<InvoiceServiceModel>();

            AddServiceToInvoiceCommand = new DelegateCommand(
                executeMethod: AddServiceToInvoice,
                canExecuteMethod: () => !IsLoading && CreatedOrSearchedService != null && CreatedOrSearchedServiceQuantity > 0
                )
                .ObservesProperty(() => IsLoading)
                .ObservesProperty(() => CreatedOrSearchedService)
                .ObservesProperty(() => CreatedOrSearchedServiceQuantity);

            GoBackCommand = new DelegateCommand(executeMethod: CloseForm);

            GoToCustomersCreationFormCommand = new DelegateCommand(
                executeMethod: () => GoToCustomersForm(ActionsOnSubmit.Create),
                canExecuteMethod: () => !IsLoading
                )
                .ObservesProperty(() => IsLoading);

            GoToCustomersModificationFormCommand = new DelegateCommand(
                executeMethod: () => GoToCustomersForm(ActionsOnSubmit.Update),
                canExecuteMethod: () => !IsLoading && Customer != null
                )
                .ObservesProperty(() => IsLoading)
                .ObservesProperty(() => Customer);

            GoToCustomersSearchCommand = new DelegateCommand(
                executeMethod: GoToCustomersSearch,
                canExecuteMethod: () => !IsLoading
                )
                .ObservesProperty(() => IsLoading);

            GoToServicesCreationFormCommand = new DelegateCommand(
                executeMethod: GoToServicesForm,
                canExecuteMethod: () => !IsLoading
                )
                .ObservesProperty(() => IsLoading);

            GoToServicesSearchCommand = new DelegateCommand(
                executeMethod: GoToServicesSearch,
                canExecuteMethod: () => !IsLoading
                )
                .ObservesProperty(() => IsLoading);


            RemoveServiceFromInvoiceCommand = new DelegateCommand(
                executeMethod: RemoveServiceFromInvoice,
                canExecuteMethod: () => !IsLoading && SelectedInvoiceService != null
                )
                .ObservesProperty(() => IsLoading)
                .ObservesProperty(() => SelectedInvoiceService);

            SubmitInvoiceCommand = new DelegateCommand(
                executeMethod: async () => await Submit(),
                canExecuteMethod: () => !IsLoading && Customer != null
                )
                .ObservesProperty(() => IsLoading)
                .ObservesProperty(() => Customer);

            eventAggregator.GetEvent<CustomerFormSubmittedEvent>()
                .Subscribe(async (customerId) => await GetCustomer(customerId));

            eventAggregator.GetEvent<CustomerSearchPerformedEvent>()
                .Subscribe(async (customerId) => await GetCustomer(customerId));

            eventAggregator.GetEvent<ServiceSearchPerformedEvent>()
                .Subscribe(async (serviceId) => await GetService(serviceId));

        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                Errors.Clear(); //Clear previous errors when coming from search or form views
                _navigationJournal = navigationContext.NavigationService.Journal;

                ActionOnSubmit = navigationContext.Parameters
                    .GetValue<ActionsOnSubmit>(NavigationParamKeys.ActionOnSubmit);

                if (ActionOnSubmit == ActionsOnSubmit.Update && _invoiceToBeUpdated == default) //This last check is because invoice might be defined already when we come from form views
                {
                    int invoiceToBeUpdatedId = navigationContext.Parameters
                        .GetValue<int>(NavigationParamKeys.ItemId);

                    await GetInvoiceTobeUpdated(invoiceToBeUpdatedId);
                }
            }
            catch (ExpectedServerErrorsException ex)
            {
                Errors.AddRange(ex.Errors);
            }
            catch (Exception)
            {
                Errors.Add("An unexpected error ocurred. Please try again.");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task GetInvoiceTobeUpdated(int id)
        {
            _invoiceToBeUpdated = await _internalInvoicesProvider.Get(id);

            Customer = _invoiceToBeUpdated.Customer;
            Date = _invoiceToBeUpdated.Date;
            InvoiceNumber = _invoiceToBeUpdated.InvoiceNumber;
            Exempt = _invoiceToBeUpdated.Exempt;
            Tax = _invoiceToBeUpdated.Tax;
            TaxBase = _invoiceToBeUpdated.TaxBase;
            InvoiceServices.AddRange(_invoiceToBeUpdated.Services);
        }

        private void CloseForm()
        {
            object? thisView = _regionManager.Regions[RegionNames.MainRegion].ActiveViews.First();
            _navigationJournal!.GoBack();
            _regionManager.Regions[RegionNames.MainRegion].Remove(thisView);
        }

        private void AddServiceToInvoice()
        {
            try
            {
                if (ServiceIsAlreadyOnInvoice())
                    throw new ClientForbiddenActionException("Selected service has been already added");

                var invoiceService = new InvoiceServiceModel
                {
                    Service = CreatedOrSearchedService!,
                    Quantity = CreatedOrSearchedServiceQuantity,
                };

                InvoiceServices.Add(invoiceService);

                CreatedOrSearchedService = null;
                CreatedOrSearchedServiceQuantity = default;
            }
            catch (ClientException ex)
            {
                Errors.Add(ex.Message);
            }
            catch (Exception)
            {
                Errors.Add("An unexpected error ocurred. Please try again.");
            }
        }

        private void GoToCustomersForm(ActionsOnSubmit actionOnSubmit)
        {
            var navigationParams = new NavigationParameters
            {
                { NavigationParamKeys.ActionOnSubmit, actionOnSubmit }
            };

            if (actionOnSubmit == ActionsOnSubmit.Update)
            {
                navigationParams.Add(NavigationParamKeys.ItemId, Customer!.Id);
            }

            _regionManager.RequestNavigate(
                RegionNames.MainRegion,
                ViewNames.CustomersFormView,
                navigationParams
                );
        }

        private void GoToCustomersSearch()
        {
            _regionManager.RequestNavigate(
                RegionNames.MainRegion,
                ViewNames.CustomersSearchView
                );
        }

        private void GoToServicesForm()
        {
            var navigationParams = new NavigationParameters
            {
                { NavigationParamKeys.ActionOnSubmit, ActionsOnSubmit.Create }
            };

            _regionManager.RequestNavigate(
                RegionNames.MainRegion,
                ViewNames.ServicesFormView,
                navigationParams
                );
        }

        private void GoToServicesSearch()
        {
            _regionManager.RequestNavigate(
                RegionNames.MainRegion,
                ViewNames.ServicesSearchView
                );
        }

        private void RemoveServiceFromInvoice()
        {
            Errors.Clear(); //These method doen't generate errors but If there's an error being displayed we want to clear it if the user try this action
            InvoiceServices.Remove(SelectedInvoiceService!);
            SelectedInvoiceService = null;
        }

        private async Task GetCustomer(int customerId)
        {
            try
            {
                IsLoading = true;
                Customer = await _customersProvider.Get(customerId);
            }
            catch (ExpectedServerErrorsException ex)
            {
                Errors.AddRange(ex.Errors);
            }
            catch (Exception)
            {
                Errors.Add("An unexpected error ocurred. Please try again.");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task GetService(int serviceId)
        {
            try
            {
                IsLoading = true;
                CreatedOrSearchedService = await _servicesProvider.Get(serviceId);
                CreatedOrSearchedServiceQuantity = 1;
            }
            catch (ExpectedServerErrorsException ex)
            {
                Errors.AddRange(ex.Errors);
            }
            catch (Exception)
            {
                Errors.Add("An unexpected error ocurred. Please try again.");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task Submit()
        {
            try
            {
                IsLoading = true;
                Errors.Clear();

                IEnumerable<InvoiceServiceInputResource> servicesData = InvoiceServices
                    .Select(invoiceService =>
                    {
                        return new InvoiceServiceInputResource
                        (
                            ServiceId: invoiceService.Service.Id,
                            Quantity: invoiceService.Quantity
                        );
                    });

                var invoiceData = new InvoiceInputResource
                (
                    Date,
                    InvoiceNumber,
                    CustomerId: Customer!.Id,
                    InvoiceServices: servicesData
                );

                if (ActionOnSubmit == ActionsOnSubmit.Create)
                {
                    await _internalInvoicesProvider.Create(invoiceData);
                }
                else if (ActionOnSubmit == ActionsOnSubmit.Update)
                {
                    await _internalInvoicesProvider
                        .Update(_invoiceToBeUpdated!.Id, invoiceData);
                }
                else
                {
                    throw new Exception("Action on Submit set to invalid value");
                }

                CloseForm();
            }
            catch (ExpectedServerErrorsException ex)
            {
                Errors.AddRange(ex.Errors);
            }
            catch (Exception)
            {
                Errors.Add("An unexpected error ocurred. Please try again.");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private bool ServiceIsAlreadyOnInvoice()
        {
            InvoiceServiceModel? result = InvoiceServices
                .FindFirst(i => i.Service.Id == CreatedOrSearchedService!.Id);

            if (result == null) return false;
            return true;
        }
    }
}
