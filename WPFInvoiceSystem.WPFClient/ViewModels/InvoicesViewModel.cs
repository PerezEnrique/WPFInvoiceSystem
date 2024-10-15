using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WPFInvoiceSystem.WPFClient.Abstractions;
using WPFInvoiceSystem.WPFClient.Events;
using WPFInvoiceSystem.WPFClient.Exceptions;
using WPFInvoiceSystem.WPFClient.Models;
using WPFInvoiceSystem.WPFClient.Utils.Constants;
using WPFInvoiceSystem.WPFClient.Utils.Enums;
using WPFInvoiceSystem.API.ApiResources;

namespace WPFInvoiceSystem.WPFClient.ViewModels
{
    public class InvoicesViewModel : BindableBase, INavigationAware
    {

        private readonly ICustomersProvider _customersProvider;
        private readonly CustomerSearchPerformedEvent _customerSearchPerformedEvent;
        private readonly IInvoicesProvider _invoicesProvider;
        private readonly IRegionManager _regionManager;
        private SubscriptionToken? _customerSearchPerformedEventToken;
        private IRegionNavigationJournal? _navigationJournal;
        public ObservableCollection<string> Errors { get; }
        public ObservableCollection<InvoiceModel> Invoices { get; set; }

        private CustomerModel? _customerToFilterFor;
        public CustomerModel? CustomerToFilterFor
        {
            get { return _customerToFilterFor; }
            set { SetProperty(ref _customerToFilterFor, value); }
        }

        private bool _filterinByDateIsEnable;
        public bool FilteringByDateIsEnable
        {
            get { return _filterinByDateIsEnable; }
            set { SetProperty(ref _filterinByDateIsEnable, value); }
        }

        private DateTime _filterFromDate;
        public DateTime FilterFromDate
        {
            get { return _filterFromDate; }
            set { SetProperty(ref _filterFromDate, value); }
        }

        private DateTime _filterToDate;
        public DateTime FilterToDate
        {
            get { return _filterToDate; }
            set { SetProperty(ref _filterToDate, value); }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        private ListContents _listCurrentlyShowing;
        public ListContents ListCurrentlyShowing
        {
            get { return _listCurrentlyShowing; }
            set { SetProperty(ref _listCurrentlyShowing, value); }
        }

        private InvoiceModel? _selectedInvoice;
        public InvoiceModel? SelectedInvoice
        {
            get { return _selectedInvoice; }
            set { SetProperty(ref _selectedInvoice, value); }
        }

        public DelegateCommand ChangeInvoicePaymentStatusCommand { get; }
        public DelegateCommand DeleteInvoiceCommand { get; }
        public DelegateCommand GoBackCommand { get; }
        public DelegateCommand GoToInvoiceCreationFormCommand { get; }
        public DelegateCommand GoToInvoiceModificationFormCommand { get; }
        public DelegateCommand GoToCustomerSearchCommand { get; set; }
        public DelegateCommand QueryWithFilterCommand { get; set; }

        public InvoicesViewModel(
            IInvoicesProvider internalInvoicesDataService,
            ICustomersProvider customersDataService,
            IRegionManager regionManager,
            IEventAggregator eventAggregator
            )
        {
            _customersProvider = customersDataService;
            _filterFromDate = DateTime.Now;
            _filterToDate = DateTime.Now;
            _invoicesProvider = internalInvoicesDataService;
            _regionManager = regionManager;

            Errors = new ObservableCollection<string>();
            Invoices = new ObservableCollection<InvoiceModel>();

            ChangeInvoicePaymentStatusCommand = new DelegateCommand(
                executeMethod: async () => await ChangeInvoicePaymentStatus(),
                canExecuteMethod: () => !IsLoading && SelectedInvoice != null
                )
                .ObservesProperty(() => IsLoading)
                .ObservesProperty(() => SelectedInvoice);

            DeleteInvoiceCommand = new DelegateCommand(
                executeMethod: async () => await DeleteInvoice(),
                canExecuteMethod: () => !IsLoading && SelectedInvoice != null
                )
                .ObservesProperty(() => IsLoading)
                .ObservesProperty(() => SelectedInvoice);

            GoBackCommand = new DelegateCommand(executeMethod: CloseView);

            GoToInvoiceCreationFormCommand = new DelegateCommand(
                executeMethod: () => GoToInvoiceForm(ActionsOnSubmit.Create)
                );

            GoToInvoiceModificationFormCommand = new DelegateCommand(
                executeMethod: () => GoToInvoiceForm(ActionsOnSubmit.Update),
                canExecuteMethod: () => !IsLoading && SelectedInvoice != null
                )
                .ObservesProperty(() => IsLoading)
                .ObservesProperty(() => SelectedInvoice);

            GoToCustomerSearchCommand = new DelegateCommand(GoToCustomerSearch);

            QueryWithFilterCommand = new DelegateCommand(
                executeMethod: async () => await QueryWithFilter(),
                canExecuteMethod: () => !IsLoading && (CustomerToFilterFor != null || FilteringByDateIsEnable)
                )
            .ObservesProperty(() => IsLoading)
            .ObservesProperty(() => CustomerToFilterFor)
            .ObservesProperty(() => FilteringByDateIsEnable);

            _customerSearchPerformedEvent = eventAggregator
                .GetEvent<CustomerSearchPerformedEvent>();
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
                if (_customerSearchPerformedEventToken != null)
                {
                    _customerSearchPerformedEvent
                        .Unsubscribe(_customerSearchPerformedEventToken);
                }

                _navigationJournal = navigationContext.NavigationService.Journal;

                IsLoading = true;

                await GetLastTenInvoices();
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

        private async Task GetLastTenInvoices()
        {
            Invoices.Clear();
            ListCurrentlyShowing = ListContents.LastTenItems;

            IEnumerable<InvoiceModel> internalInvoices = await _invoicesProvider
                .GetLastTenInvoices();

            Invoices.AddRange(internalInvoices);
        }

        private void CloseView()
        {
            object? thisView = _regionManager.Regions[RegionNames.MainRegion].ActiveViews.First();
            _navigationJournal!.GoBack();
            _regionManager.Regions[RegionNames.MainRegion].Remove(thisView);
        }

        private async Task ChangeInvoicePaymentStatus()
        {
            try
            {
                IsLoading = true;
                Errors.Clear();

                await _invoicesProvider
                    .ChangeInvoicePaymentStatus(SelectedInvoice!.Id);

                await GetLastTenInvoices();
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

        private async Task DeleteInvoice()
        {
            try
            {
                IsLoading = true;
                Errors.Clear();

                await _invoicesProvider
                    .Delete(SelectedInvoice!.Id);

                await GetLastTenInvoices();
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

        private void GoToInvoiceForm(ActionsOnSubmit actionOnSubmit)
        {
            var navigationParams = new NavigationParameters
            {
                { NavigationParamKeys.ActionOnSubmit, actionOnSubmit }
            };

            if (actionOnSubmit == ActionsOnSubmit.Update)
            {
                navigationParams.Add(NavigationParamKeys.ItemId, SelectedInvoice!.Id);
            }

            _regionManager.RequestNavigate(
                RegionNames.MainRegion,
                ViewNames.InvoicesFormView,
                navigationParams
                );
        }

        private void GoToCustomerSearch()
        {
            _customerSearchPerformedEventToken = _customerSearchPerformedEvent
                    .Subscribe(async (customerId) => await GetCustomerToFilterFor(customerId));

            _regionManager.RequestNavigate(
                RegionNames.MainRegion,
                ViewNames.CustomersSearchView
                );
        }

        private async Task GetCustomerToFilterFor(int customerId)
        {
            try
            {
                IsLoading = true;
                CustomerToFilterFor = await _customersProvider.Get(customerId);
                QueryWithFilterCommand.Execute();
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

        private async Task QueryWithFilter()
        {
            try
            {
                Errors.Clear();
                IsLoading = true;

                if (FilterFromDate > FilterToDate)
                    throw new ClientValidationException("Initial Date cannot be greater than Final Date");

                if (FilterFromDate == FilterToDate)
                    FilterToDate = FilterToDate.AddDays(1);

                var internalInvoicesFilter = new InvoicesFilterResource
                    (
                        FromDate: FilteringByDateIsEnable ? FilterFromDate : null,
                        ToDate: FilteringByDateIsEnable ? FilterToDate : null,
                        CustomerId: CustomerToFilterFor?.Id
                    );

                IEnumerable<InvoiceModel> internalInvoices = await _invoicesProvider
                    .Find(internalInvoicesFilter);

                Invoices.Clear();

                ListCurrentlyShowing = ListContents.SearchResults;

                Invoices.AddRange(internalInvoices);
            }
            catch (ClientException ex)
            {
                Errors.Add(ex.Message);
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
    }
}
