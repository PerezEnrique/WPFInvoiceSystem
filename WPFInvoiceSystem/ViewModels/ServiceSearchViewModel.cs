using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Domain;
using WPFInvoiceSystem.Domain.Modals;
using WPFInvoiceSystem.Utils.Constants;

namespace WPFInvoiceSystem.ViewModels
{
    public class ServiceSearchViewModel : BaseSearchViewModel<Service>, IDialogAware
    {
        public DelegateCommand ConfirmCommand { get; }
        public DelegateCommand GoBackCommand { get; }
        public DelegateCommandBase SearchCommand { get; }
        public string Title => "Invoice System - Service Search";
        public ObservableCollection<ServiceType> ServiceTypes { get; }

        private ServiceType? _selectedType;
        public ServiceType? SelectedType
        {
            get { return _selectedType; }
            set { SetProperty(ref _selectedType, value); }
        }

        
        public event Action<IDialogResult>? RequestClose;


        public ServiceSearchViewModel(IUnitOfWork unitOfWork) : base(unitOfWork) 
        {
            ServiceTypes = new ObservableCollection<ServiceType>();

            ConfirmCommand = new DelegateCommand(
                executeMethod: GoBackAfterSelectItem,
                canExecuteMethod: () => SelectedItem != null
                )
                .ObservesProperty(() => SelectedItem);

            GoBackCommand = new DelegateCommand(GoBack);

            SearchCommand = new DelegateCommand(
                executeMethod: async () => await Search(s => s.Name.Contains(Query) && s.Type == SelectedType),
                canExecuteMethod: () => !string.IsNullOrWhiteSpace(Query) && !IsLoading
                )
                .ObservesProperty(() => Query)
                .ObservesProperty(() => IsLoading);
        }


        public async void OnDialogOpened(IDialogParameters parameters)
        {
            try
            {
                IsLoading = true;
                await GetServicesTypes();
            }
            catch (Exception)
            {
                Errors.Add(UnexpectedErrorMessage.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task GetServicesTypes()
        {
            var serviceTypes = await Task.Run(() => _unitOfWork.ServiceTypesRepository.GetAll());
            ServiceTypes.AddRange(serviceTypes);
            SelectedType = ServiceTypes.FirstOrDefault();
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        private void GoBack()
        {
            var result = ButtonResult.Cancel;
            RequestClose?.Invoke(new DialogResult(result));
        }

        private void GoBackAfterSelectItem()
        {
            var result = ButtonResult.OK;

            var dialogParams = new DialogParameters { { ParamKeys.Service, SelectedItem } };

            RequestClose?.Invoke(new DialogResult(result, dialogParams));
        }
    }
}
