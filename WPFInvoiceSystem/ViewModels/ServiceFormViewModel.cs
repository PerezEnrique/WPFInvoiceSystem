using FluentValidation;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WPFInvoiceSystem.Domain;
using WPFInvoiceSystem.Domain.Modals;
using WPFInvoiceSystem.Utils.Constants;

namespace WPFInvoiceSystem.ViewModels
{
    public class ServiceFormViewModel : BindableBase, IDialogAware
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<Service> _validator;
        private Service _service;
        public DelegateCommand ConfirmCommand { get; }
        public DelegateCommand GoBackCommand { get; }
        public string Title => "Invoice System - Service Form";
        public ObservableCollection<string> Errors { get; }
        public ObservableCollection<ServiceType> ServicesTypes { get; }

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
            private set { SetProperty(ref _isLoading, value); }
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

        private ServiceType? _selectedType;
        public ServiceType? SelectedType
        {
            get { return _selectedType; }
            set { SetProperty(ref _selectedType, value); }
        }


        public event Action<IDialogResult>? RequestClose;


        public ServiceFormViewModel(
            IUnitOfWork unitOfWork,
            IValidator<Service> validator)
        {
            _name = string.Empty;
            _service = new Service();
            _validator = validator;
            _unitOfWork = unitOfWork;

            Errors = new ObservableCollection<string>();
            ServicesTypes = new ObservableCollection<ServiceType>();

            ConfirmCommand = new DelegateCommand(
                executeMethod: async () => await ValidateAndSubmit(),
                canExecuteMethod: () => SelectedType != null && !IsLoading
                )
                .ObservesProperty(() => SelectedType)
                .ObservesProperty(() => IsLoading);

            GoBackCommand = new DelegateCommand(GoBack);
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
            ServicesTypes.AddRange(serviceTypes);
            SelectedType = ServicesTypes.FirstOrDefault();
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

        private async Task ValidateAndSubmit()
        {
            if(!IsLoading && SelectedType != null)
            {
                try
                {
                    IsLoading = true;
                    Errors.Clear();

                    _service.Name = Name;
                    _service.Price = Price;
                    _service.Type = SelectedType;
                    _service.IsExempt = IsExempt;

                    _validator.ValidateAndThrow(_service);

                    _unitOfWork.ServicesRepository.Add(_service);

                    await _unitOfWork.CompleteAsync();

                    GoBackAfterSubmit();
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

        private void GoBackAfterSubmit()
        {
            var result = ButtonResult.OK;

            var dialogParams = new DialogParameters { { ParamKeys.Service, _service } };

            RequestClose?.Invoke(new DialogResult(result, dialogParams));
        }
    }
}
