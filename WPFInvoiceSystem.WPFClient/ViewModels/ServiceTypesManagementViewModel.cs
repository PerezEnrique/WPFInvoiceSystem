using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WPFInvoiceSystem.WPFClient.Abstractions;
using WPFInvoiceSystem.WPFClient.Exceptions;
using WPFInvoiceSystem.WPFClient.Models;
using WPFInvoiceSystem.WPFClient.Utils.Constants;
using WPFInvoiceSystem.WPFClient.Utils.Enums;

namespace WPFInvoiceSystem.WPFClient.ViewModels
{
    public class ServiceTypesManagementViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly IServiceTypesProvider _serviceTypesProvider;
        private IRegionNavigationJournal? _navigationJournal;
        public ObservableCollection<string> Errors { get; }
        public ObservableCollection<ServiceTypeModel> Types { get; set; }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        private ServiceTypeModel? _selectedServiceType;
        public ServiceTypeModel? SelectedServiceType
        {
            get { return _selectedServiceType; }
            set { SetProperty(ref _selectedServiceType, value); }
        }

        public DelegateCommand GoBackCommand { get; }
        public DelegateCommand DeleteServiceTypeCommand { get; }
        public DelegateCommand GoToServiceTypeCreationFormCommand { get; }
        public DelegateCommand GoToServiceTypeModificationFormCommand { get; }

        public ServiceTypesManagementViewModel(
            IServiceTypesProvider serviceTypesProvider,
            IRegionManager regionManager
)
        {
            _regionManager = regionManager;
            _serviceTypesProvider = serviceTypesProvider;

            Errors = new ObservableCollection<string>();
            Types = new ObservableCollection<ServiceTypeModel>();

            DeleteServiceTypeCommand = new DelegateCommand(
                executeMethod: async () => await DeleteServiceType(),
                canExecuteMethod: () => !IsLoading && SelectedServiceType != null
            )
            .ObservesProperty(() => IsLoading)
            .ObservesProperty(() => SelectedServiceType);

            GoBackCommand = new DelegateCommand(executeMethod: () => _navigationJournal?.GoBack());

            GoToServiceTypeCreationFormCommand = new DelegateCommand(
                executeMethod: () => GoToServiceTypeForm(ActionsOnSubmit.Create),
                canExecuteMethod: () => !IsLoading
            )
            .ObservesProperty(() => IsLoading);

            GoToServiceTypeModificationFormCommand = new DelegateCommand(
                executeMethod: () => GoToServiceTypeForm(ActionsOnSubmit.Update),
                canExecuteMethod: () => !IsLoading && SelectedServiceType != null
            )
            .ObservesProperty(() => IsLoading)
            .ObservesProperty(() => SelectedServiceType);
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

                IsLoading = true;

                await GetServiceTypes();
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
            Types.Clear();
            IEnumerable<ServiceTypeModel> types = await _serviceTypesProvider
                .GetAll();
            Types.AddRange(types);
        }

        private async Task DeleteServiceType()
        {
            try
            {
                IsLoading = true;
                Errors.Clear();

                await _serviceTypesProvider.Delete(SelectedServiceType!.Id);

                await GetServiceTypes();
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

        private void GoToServiceTypeForm(ActionsOnSubmit actionOnSubmit)
        {
            var navigationParams = new NavigationParameters
            {
                { NavigationParamKeys.ActionOnSubmit, actionOnSubmit }
            };

            if (actionOnSubmit == ActionsOnSubmit.Update)
            {
                navigationParams.Add(NavigationParamKeys.ItemId, SelectedServiceType!.Id);
            }

            _regionManager.RequestNavigate(
                RegionNames.MainRegion,
                ViewNames.ServiceTypesFormView,
                navigationParams
                );
        }
    }
}
