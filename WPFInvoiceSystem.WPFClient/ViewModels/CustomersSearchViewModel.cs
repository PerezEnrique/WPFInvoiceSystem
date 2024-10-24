using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;
using WPFInvoiceSystem.WPFClient.Models;
using WPFInvoiceSystem.WPFClient.Abstractions;
using WPFInvoiceSystem.WPFClient.Events;
using WPFInvoiceSystem.WPFClient.Exceptions;
using System.Linq;
using WPFInvoiceSystem.WPFClient.Utils.Enums;

namespace WPFInvoiceSystem.WPFClient.ViewModels
{
    public class CustomersSearchViewModel : BindableBase, INavigationAware
    {
        private readonly ICustomersProvider _customersProvider;
        private readonly IEventAggregator _eventAggregator;
        private IRegionNavigationJournal? _navigationJournal;
        public ObservableCollection<CustomerModel> Customers { get; }
        public ObservableCollection<string> Errors { get; }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        private string _query;
        public string Query
        {
            get { return _query; }
            set { SetProperty(ref _query, value); }
        }

        private CustomerModel? _selectedCustomer;
        public CustomerModel? SelectedCustomer
        {
            get { return _selectedCustomer; }
            set { SetProperty(ref _selectedCustomer, value); }
        }

        public DelegateCommand CancelCommand { get; }
        public DelegateCommand SearchByIdentityCardCommand { get; }
        public DelegateCommand SearchByNameCommand { get; }
        public DelegateCommand ConfirmCommand { get; }

        public CustomersSearchViewModel(
            ICustomersProvider customersProvider,
            IEventAggregator eventAggregator
            )
        {
            _customersProvider = customersProvider;
            _eventAggregator = eventAggregator;
            _query = string.Empty;

            Errors = new ObservableCollection<string>();
            Customers = new ObservableCollection<CustomerModel>();

            CancelCommand = new DelegateCommand(
                executeMethod: () => _navigationJournal?.GoBack()
                );

            SearchByIdentityCardCommand = new DelegateCommand(
                executeMethod: async () => await Search(CustomersSearchCriteria.ByIdentityCard),
                canExecuteMethod: () => !IsLoading && !string.IsNullOrWhiteSpace(Query)
                )
                .ObservesProperty(() => IsLoading)
                .ObservesProperty(() => Query);

            SearchByNameCommand = new DelegateCommand(
                executeMethod: async () => await Search(CustomersSearchCriteria.ByName),
                canExecuteMethod: () => !IsLoading && !string.IsNullOrWhiteSpace(Query)
                )
                .ObservesProperty(() => IsLoading)
                .ObservesProperty(() => Query);

            ConfirmCommand = new DelegateCommand(
                executeMethod: UseSelectedCustomer,
                canExecuteMethod: () => !IsLoading && SelectedCustomer != null
                )
                .ObservesProperty(() => IsLoading)
                .ObservesProperty(() => SelectedCustomer);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _navigationJournal = navigationContext.NavigationService.Journal;
        }

        private async Task Search(CustomersSearchCriteria searchCriteria)
        {
            try
            {
                IsLoading = true;
                Errors.Clear();
                Customers.Clear();
                IEnumerable<CustomerModel> result;

                if (searchCriteria == CustomersSearchCriteria.ByIdentityCard)
                {
                    result = await _customersProvider.FindByIdentityCard(identityCard: Convert.ToInt32(Query));
                }
                else if (searchCriteria == CustomersSearchCriteria.ByName)
                {
                    result = await _customersProvider.FindByName(name: Query);
                }
                else
                {
                    throw new ArgumentException($"Unexpected People search criteria value: {searchCriteria}");
                }

                if (!result.Any())
                    Errors.Add("We couldn't find anything that matches your search.");

                Customers.AddRange(result);
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

        private void UseSelectedCustomer()
        {
            _eventAggregator.GetEvent<CustomerSearchPerformedEvent>()
                .Publish(SelectedCustomer!.Id);

            _navigationJournal!.GoBack();
        }
    }
}
