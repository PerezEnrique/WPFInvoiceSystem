using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WPFInvoiceSystem.WPFClient.Abstractions;
using WPFInvoiceSystem.WPFClient.ApiModels;
using WPFInvoiceSystem.WPFClient.Exceptions;
using WPFInvoiceSystem.WPFClient.Models;
using WPFInvoiceSystem.WPFClient.Utils.Constants;
using WPFInvoiceSystem.WPFClient.Utils.Enums;

namespace WPFInvoiceSystem.WPFClient.ViewModels
{
    public class ServicesFormViewModel : BindableBase, INavigationAware
    {
        private readonly IServicesProvider _servicesProvider;
        private readonly IServiceTypesProvider _serviceTypesProvider;
        private IRegionNavigationJournal? _navigationJournal;
        private ServiceModel? _serviceToBeUpdated;
        public ObservableCollection<string> Errors { get; }
        public ObservableCollection<ServiceTypeModel> ServiceTypes { get; }

        private bool _isExempt;
        public bool IsExempt
        {
            get { return _isExempt; }
            set { SetProperty(ref _isExempt, value); }
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

        private decimal _price;
        public decimal Price
        {
            get { return _price; }
            set { SetProperty(ref _price, value); }
        }

        private ServiceTypeModel? _selectedType;
        public ServiceTypeModel? SelectedType
        {
            get { return _selectedType; }
            set { SetProperty(ref _selectedType, value); }
        }

        private ActionsOnSubmit? _actionOnSubmit;
        public ActionsOnSubmit? ActionOnSubmit
        {
            get { return _actionOnSubmit; }
            private set { SetProperty(ref _actionOnSubmit, value); }
        }

        public DelegateCommand CancelCommand { get; }
        public DelegateCommand ConfirmCommand { get; }

        public ServicesFormViewModel(
            IServicesProvider servicesProvider,
            IServiceTypesProvider serviceTypesProvider
            )
        {
            _name = string.Empty;
            _servicesProvider = servicesProvider;
            _serviceTypesProvider = serviceTypesProvider;

            Errors = new ObservableCollection<string>();
            ServiceTypes = new ObservableCollection<ServiceTypeModel>();

            CancelCommand = new DelegateCommand(
                executeMethod: () => _navigationJournal?.GoBack()
                );

            ConfirmCommand = new DelegateCommand(
                executeMethod: async () => await Submit(),
                canExecuteMethod: () => !IsLoading && !string.IsNullOrWhiteSpace(Name) && SelectedType != null
                )
                .ObservesProperty(() => IsLoading)
                .ObservesProperty(() => Name)
                .ObservesProperty(() => SelectedType);
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
                IsLoading = true;
                _navigationJournal = navigationContext.NavigationService.Journal;

                ActionOnSubmit = navigationContext.Parameters
                    .GetValue<ActionsOnSubmit>(NavigationParamKeys.ActionOnSubmit);

                await GetServiceTypes();

                if (ActionOnSubmit == ActionsOnSubmit.Update)
                {
                    int serviceToBeUpdatedId = navigationContext.Parameters
                        .GetValue<int>(NavigationParamKeys.ItemId);

                    await GetServiceToBeUpdated(serviceToBeUpdatedId);
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

        private async Task GetServiceTypes()
        {
            IEnumerable<ServiceTypeModel> serviceTypes = await _serviceTypesProvider
                .GetAll();

            ServiceTypes.AddRange(serviceTypes);

            SelectedType = ServiceTypes.FirstOrDefault();
        }

        private async Task GetServiceToBeUpdated(int id)
        {
            _serviceToBeUpdated = await _servicesProvider.Get(id);

            IsExempt = _serviceToBeUpdated.IsExempt;
            Name = _serviceToBeUpdated.Name;
            Price = _serviceToBeUpdated.Price;
            SelectedType = ServiceTypes.SingleOrDefault(s => s.Id == _serviceToBeUpdated.Type.Id);
        }

        private async Task Submit()
        {
            try
            {
                IsLoading = true;
                Errors.Clear();

                var serviceData = new ServiceInputAPIModel(
                        Name,
                        Price,
                        SelectedType!.Id,
                        IsExempt
                    );

                if (ActionOnSubmit == ActionsOnSubmit.Create)
                {
                    await _servicesProvider
                        .Create(serviceData);
                }
                else if (ActionOnSubmit == ActionsOnSubmit.Update)
                {
                    await _servicesProvider
                        .Update(_serviceToBeUpdated!.Id, serviceData);
                }
                else
                {
                    throw new Exception("Action on Submit set to invalid value");
                }

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
    }
}
