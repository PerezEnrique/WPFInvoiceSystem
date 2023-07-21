using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Domain;
using WPFInvoiceSystem.Domain.Entities;
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
                executeMethod: Confirm,
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


        private void Confirm()
        {
            var result = ButtonResult.OK;

            //Set params to return
            var dialogParams = new DialogParameters { { ParamKeys.Service, SelectedItem } };

            //Request dialog close
            RequestClose?.Invoke(new DialogResult(result, dialogParams));
        }

        private async Task<IEnumerable<ServiceType>> GetServiceTypes()
        {
            return await _unitOfWork.ServiceTypesRepository.GetAll();
        }

        private void GoBack()
        {
            var result = ButtonResult.Cancel;
            RequestClose?.Invoke(new DialogResult(result));
        }

        //IDialogAware methods implementation
        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public async void OnDialogOpened(IDialogParameters parameters)
        {
            IsLoading = true;

            try
            {
                ServiceTypes.AddRange(await Task.Run(async () => await GetServiceTypes()));

                if (!ServiceTypes.Any())
                {
                    throw new Exception("Services types collection is empty");
                }

                SelectedType = ServiceTypes.FirstOrDefault();
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
    }
}
