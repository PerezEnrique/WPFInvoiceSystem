using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WPFInvoiceSystem.WPFClient.Abstractions;
using WPFInvoiceSystem.WPFClient.ApiModels;
using WPFInvoiceSystem.WPFClient.Exceptions;
using WPFInvoiceSystem.WPFClient.Models;
using WPFInvoiceSystem.WPFClient.Utils.Constants;
using WPFInvoiceSystem.WPFClient.Utils.Enums;

namespace WPFInvoiceSystem.WPFClient.ViewModels
{
    public class ServiceTypesFormViewModel : BindableBase, INavigationAware
    {
        private readonly IServiceTypesProvider _serviceTypesProvider;
        private IRegionNavigationJournal? _navigationJournal;
        private ServiceTypeModel? _serviceTypeToBeUpdated;
        public ObservableCollection<string> Errors { get; }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        private ActionsOnSubmit? _actionOnSubmit;
        public ActionsOnSubmit? ActionOnSubmit
        {
            get { return _actionOnSubmit; }
            private set { SetProperty(ref _actionOnSubmit, value); }
        }

        private string _typeName;
        public string TypeName
        {
            get { return _typeName; }
            set { SetProperty(ref _typeName, value); }
        }

        public DelegateCommand CancelCommand { get; }
        public DelegateCommand ConfirmCommand { get; }

        public ServiceTypesFormViewModel(IServiceTypesProvider serviceTypesProvider)
        {
            _typeName = string.Empty;
            _serviceTypesProvider = serviceTypesProvider;

            Errors = new ObservableCollection<string>();

            CancelCommand = new DelegateCommand(
                executeMethod: () => _navigationJournal?.GoBack()
            );

            ConfirmCommand = new DelegateCommand(
                executeMethod: async () => await Submit(),
                canExecuteMethod: () => !IsLoading && !string.IsNullOrWhiteSpace(TypeName)
            )
            .ObservesProperty(() => IsLoading)
            .ObservesProperty(() => TypeName);
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

                    int serviceTypeToUpdateId = navigationContext.Parameters
                        .GetValue<int>(NavigationParamKeys.ItemId);

                    await GetServiceTypeToBeUpdated(serviceTypeToUpdateId);
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

        private async Task GetServiceTypeToBeUpdated(int id)
        {
            _serviceTypeToBeUpdated = await _serviceTypesProvider.Get(id);

            TypeName = _serviceTypeToBeUpdated.Name;
        }

        private async Task Submit()
        {
            try
            {
                IsLoading = true;
                Errors.Clear();

                var serviceTypeData = new ServiceTypeInputAPIModel(TypeName);

                if (ActionOnSubmit == ActionsOnSubmit.Create)
                {
                    await _serviceTypesProvider.Create(serviceTypeData);
                }
                else if (ActionOnSubmit == ActionsOnSubmit.Update)
                {
                    await _serviceTypesProvider.Update(_serviceTypeToBeUpdated!.Id, serviceTypeData);
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
