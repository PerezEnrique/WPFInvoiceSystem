using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WPFInvoiceSystem.WPFClient.Abstractions;
using WPFInvoiceSystem.WPFClient.Events;
using WPFInvoiceSystem.WPFClient.Exceptions;
using WPFInvoiceSystem.WPFClient.Models;

namespace WPFInvoiceSystem.WPFClient.ViewModels
{
    public class ServicesSearchViewModel : BindableBase, INavigationAware
    {
        private readonly IServicesProvider _servicesProvider;
        private readonly IEventAggregator _eventAggregator;
        private IRegionNavigationJournal? _navigationJournal;

        public ObservableCollection<ServiceModel> Services { get; }
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

        private ServiceModel? _selectedService;
        public ServiceModel? SelectedService
        {
            get { return _selectedService; }
            set { SetProperty(ref _selectedService, value); }
        }

        public DelegateCommand CancelCommand { get; }
        public DelegateCommand SearchCommand { get; }
        public DelegateCommand ConfirmCommand { get; }
        public ServicesSearchViewModel(
            IServicesProvider servicesProvider,
            IEventAggregator eventAggregator
            )
        {
            _servicesProvider = servicesProvider;
            _eventAggregator = eventAggregator;
            _query = string.Empty;

            Errors = new ObservableCollection<string>();
            Services = new ObservableCollection<ServiceModel>();

            CancelCommand = new DelegateCommand(
                executeMethod: () => _navigationJournal?.GoBack()
                );

            SearchCommand = new DelegateCommand(
                executeMethod: async () => await Search(),
                canExecuteMethod: () => !IsLoading && !string.IsNullOrWhiteSpace(Query)
                )
                .ObservesProperty(() => IsLoading)
                .ObservesProperty(() => Query);

            ConfirmCommand = new DelegateCommand(
                executeMethod: UseSelectedService,
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

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _navigationJournal = navigationContext.NavigationService.Journal;
        }

        public async Task Search()
        {
            try
            {
                IsLoading = true;
                Errors.Clear();
                Services.Clear();

                IEnumerable<ServiceModel> results = await _servicesProvider
                    .FindByName(name: Query);

                if (!results.Any())
                    Errors.Add("We couldn't find anything that matches your search.");

                Services.AddRange(results);
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

        private void UseSelectedService()
        {
            _eventAggregator.GetEvent<ServiceSearchPerformedEvent>()
                .Publish(SelectedService!.Id);

            _navigationJournal!.GoBack();
        }
    }
}
