using ImTools;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
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
    public class CustomersFormViewModel : BindableBase, INavigationAware
    {
        private readonly ICustomersProvider _customersProvider;
        private readonly IEventAggregator _eventAggregator;
        private IRegionNavigationJournal? _navigationJournal;
        public CustomerModel? _customerToBeUpdated;

        public ObservableCollection<string> Errors { get; }

        private ActionsOnSubmit? _actionOnSubmit;
        public ActionsOnSubmit? ActionOnSubmit
        {
            get { return _actionOnSubmit; }
            private set { SetProperty(ref _actionOnSubmit, value); }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set { SetProperty(ref _address, value); }
        }

        private DateTime? _birthdate;
        public DateTime? Birthdate
        {
            get { return _birthdate; }
            set { SetProperty(ref _birthdate, value); }
        }

        private int _identityCard;
        public int IdentityCard
        {
            get { return _identityCard; }
            set { SetProperty(ref _identityCard, value); }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set { SetProperty(ref _phone, value); }
        }

        public DelegateCommand CancelCommand { get; }
        public DelegateCommand ConfirmCommand { get; }

        public CustomersFormViewModel(
            ICustomersProvider customersProvider,
            IEventAggregator eventAggregator
            )
        {
            _customersProvider = customersProvider;
            _eventAggregator = eventAggregator;
            _address = string.Empty;
            _name = string.Empty;
            _phone = string.Empty;

            Errors = new ObservableCollection<string>();

            CancelCommand = new DelegateCommand(
                executeMethod: () => _navigationJournal?.GoBack()
                );

            ConfirmCommand = new DelegateCommand(
                executeMethod: async () => await Submit(),
                    canExecuteMethod: () => !IsLoading && RequiredPropertiesAreNotEmpty()
                    )
                    .ObservesProperty(() => IsLoading)
                    .ObservesProperty(() => Address)
                    .ObservesProperty(() => Name);

        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                _navigationJournal = navigationContext.NavigationService.Journal;

                ActionOnSubmit = navigationContext.Parameters
                    .GetValue<ActionsOnSubmit>(NavigationParamKeys.ActionOnSubmit);

                if (ActionOnSubmit == ActionsOnSubmit.Update)
                {
                    IsLoading = true;

                    int customerToBeUpdatedId = navigationContext.Parameters.GetValue<int>(NavigationParamKeys.ItemId);
                    await GetCustomerToBeUpdated(customerToBeUpdatedId);
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

        private async Task GetCustomerToBeUpdated(int id)
        {
            _customerToBeUpdated = await _customersProvider.Get(id);

            Address = _customerToBeUpdated.Address;
            Birthdate = _customerToBeUpdated.Birthdate?.ToDateTime(TimeOnly.MinValue);
            IdentityCard = _customerToBeUpdated.IdentityCard;
            Name = _customerToBeUpdated.Name;
            Phone = _customerToBeUpdated.Phone;
        }

        private async Task Submit()
        {
            try
            {
                IsLoading = true;
                Errors.Clear();

                var customerData = new CustomerInputResource(
                    Name,
                    IdentityCard,
                    Phone,
                    Address,
                    Birthdate: Birthdate != null ? DateOnly.FromDateTime((DateTime)Birthdate) : null
                    );

                CustomerModel customer;

                if (ActionOnSubmit == ActionsOnSubmit.Create)
                {
                    customer = await _customersProvider
                        .Create(customerData);
                }
                else if (ActionOnSubmit == ActionsOnSubmit.Update)
                {
                    customer = await _customersProvider
                        .Update(_customerToBeUpdated!.Id, customerData);
                }
                else
                {
                    throw new Exception("Action on Submit set to invalid value");
                }

                _eventAggregator.GetEvent<CustomerFormSubmittedEvent>()
                   .Publish(customer.Id);

                _navigationJournal!.GoBack();
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

        private bool RequiredPropertiesAreNotEmpty()
        {
            return !string.IsNullOrWhiteSpace(Address) &&
                !string.IsNullOrWhiteSpace(Name);
        }
    }
}
