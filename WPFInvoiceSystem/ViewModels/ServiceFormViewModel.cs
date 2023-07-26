using FluentValidation;
using Prism.Commands;
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
    public class ServiceFormViewModel : BaseFormViewModel<Service>, IDialogAware
    {
        private Service _service;
        public DelegateCommand ConfirmCommand { get; }
        public DelegateCommand GoBackCommand { get; }
        public string Title => "Invoice System - Service Form";
        public ObservableCollection<ServiceType> ServicesTypes { get; }


        private bool _isExempt;
        public bool IsExempt
        {
            get { return _isExempt; }
            set { SetProperty(ref _isExempt, value); }
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
            IValidator<Service> validator) : base(unitOfWork, validator)
        {
            _name = string.Empty;
            _service = new Service();
            ServicesTypes = new ObservableCollection<ServiceType>();

            ConfirmCommand = new DelegateCommand(
                executeMethod: async () => await Submit(),
                canExecuteMethod: () => SelectedType != null && !IsLoading
                )
                .ObservesProperty(() => SelectedType)
                .ObservesProperty(() => IsLoading);

            GoBackCommand = new DelegateCommand(GoBack);
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

        protected override Task<string?> AditionalValidation()
        {
            return Task.FromResult<string?>(null);
        }

        protected override Service ComposeObjectToSave()
        {
            _service.Name = Name;
            _service.Price = Price;
            _service.Type = SelectedType; //If for some reason it's null, validation will trigger
            _service.IsExempt = IsExempt;

            //Validation is handled by base class

            return _service;
        }

        protected override void OnSavingComplete()
        {
            //Set dialog result
            var result = ButtonResult.OK;

            //Set params to return
            var dialogParams = new DialogParameters { { ParamKeys.Service, _service } };

            //Request dialog close
            RequestClose?.Invoke(new DialogResult(result, dialogParams));
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
                ServicesTypes.AddRange(await Task.Run(async () => await GetServiceTypes()));

                if (!ServicesTypes.Any())
                {
                    throw new Exception("Service types collection is empty");
                }

                SelectedType = ServicesTypes.FirstOrDefault();
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
