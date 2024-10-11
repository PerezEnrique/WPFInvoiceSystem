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
    public class ServicesManagementViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly IServicesProvider _servicesProvider;
        private IRegionNavigationJournal? _navigationJournal;
        public ObservableCollection<string> Errors { get; }
        public ObservableCollection<ServiceModel> Services { get; }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        private ServiceModel? _selectedService;
        public ServiceModel? SelectedService
        {
            get { return _selectedService; }
            set { SetProperty(ref _selectedService, value); }
        }

        public DelegateCommand DeleteServiceCommand { get; }
        public DelegateCommand GoBackCommand { get; }
        public DelegateCommand GoToServiceCreationFormCommand { get; }
        public DelegateCommand GoToServiceModificationFormCommand { get; }

        public ServicesManagementViewModel(
            IServicesProvider servicesProvider,
            IRegionManager regionManager
            )
        {
            _servicesProvider = servicesProvider;
            _regionManager = regionManager;

            Errors = new ObservableCollection<string>();
            Services = new ObservableCollection<ServiceModel>();

            DeleteServiceCommand = new DelegateCommand(
                executeMethod: async () => await DeleteService(),
                canExecuteMethod: () => !IsLoading && SelectedService != null
                )
                .ObservesProperty(() => IsLoading)
                .ObservesProperty(() => SelectedService);

            GoBackCommand = new DelegateCommand(executeMethod: () => _navigationJournal?.GoBack());

            GoToServiceCreationFormCommand = new DelegateCommand(
                executeMethod: () => GoToServicesForm(ActionsOnSubmit.Create),
                canExecuteMethod: () => !IsLoading
                )
                .ObservesProperty(() => IsLoading);

            GoToServiceModificationFormCommand = new DelegateCommand(
                executeMethod: () => GoToServicesForm(ActionsOnSubmit.Update),
                canExecuteMethod: () => !IsLoading && SelectedService != null
                )
                .ObservesProperty(() => IsLoading)
                .ObservesProperty(() => SelectedService);
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
                await GetServices();
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

        private async Task GetServices()
        {
            Services.Clear();

            IEnumerable<ServiceModel> services = await _servicesProvider
                .GetAll();

            Services.AddRange(services);
        }

        private async Task DeleteService()
        {
            try
            {
                IsLoading = true;
                Errors.Clear();

                await _servicesProvider
                    .Delete(SelectedService!.Id);

                await GetServices();
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

        private void GoToServicesForm(ActionsOnSubmit actionOnSubmit)
        {
            var navigationParams = new NavigationParameters
            {
                { NavigationParamKeys.ActionOnSubmit, actionOnSubmit }
            };

            if (actionOnSubmit == ActionsOnSubmit.Update)
            {
                navigationParams.Add(NavigationParamKeys.ItemId, SelectedService!.Id);
            }

            _regionManager.RequestNavigate(
                RegionNames.MainRegion,
                ViewNames.ServicesFormView,
                navigationParams
                );  
        }
    }
}
