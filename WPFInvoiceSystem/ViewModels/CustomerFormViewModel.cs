using FluentValidation;
using Prism.Commands;
using Prism.Mvvm;
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
    public class CustomerFormViewModel : BindableBase, IDialogAware
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<Customer> _validator;
        private Customer _customer;
        public DelegateCommand ConfirmCommand { get; }
        public DelegateCommand GoBackCommand { get; }
        public string Title => "Invoice System - Customer Form";
        public ObservableCollection<string> Errors { get; }

        private DateTime? _birthdate;
        public DateTime? Birthdate
        {
            get { return _birthdate; }
            set { SetProperty(ref _birthdate, value); }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set { SetProperty(ref _address, value); }
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
            private set { SetProperty(ref _isLoading, value); }
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

        private string _submitAction;
        public string SubmitAction
        {
            get { return _submitAction; }
            private set { SetProperty(ref _submitAction, value); }
        }

        public event Action<IDialogResult>? RequestClose;


        public CustomerFormViewModel(
            IUnitOfWork unitOfWork,
            IValidator<Customer> validator
            )
        {
            _customer = new Customer();
            _name = string.Empty;
            _address = string.Empty;
            _phone = string.Empty;
            _submitAction = string.Empty;
            _unitOfWork = unitOfWork;
            _validator = validator;

            Errors = new ObservableCollection<string>();

            ConfirmCommand = new DelegateCommand(
                executeMethod: async () => await ValidateAndSubmit(),
                canExecuteMethod: () => !IsLoading
                )
                .ObservesProperty(() => IsLoading);

            GoBackCommand = new DelegateCommand(GoBack);
        }

        public async void OnDialogOpened(IDialogParameters parameters)
        {

            try
            {
                IsLoading = true;
                SubmitAction = parameters.GetValue<string>(ParamKeys.SumbitAction);

                if (SubmitAction == SubmitActions.Update)
                {
                    var customerId = parameters.GetValue<int>(ParamKeys.CustomerId);
                    var customerToUpdate = await Task.Run(async () => await _unitOfWork.CustomersRepository.Get(customerId));

                    if (customerToUpdate == null)
                        throw new Exception("Couldn't find customer with provided Id");

                    _customer = customerToUpdate;
                    Address = _customer.Address;
                    Birthdate = _customer.Birthdate;
                    IdentityCard = _customer.IdentityCard;
                    Name = _customer.Name;
                    Phone = _customer.Phone;
                }
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
            if(!IsLoading)
            {
                try
                {
                    IsLoading = true;
                    Errors.Clear();

                    _customer.Address = Address;
                    _customer.Birthdate = Birthdate;
                    _customer.IdentityCard = IdentityCard;
                    _customer.Name = Name;
                    _customer.Phone = Phone;

                    if(SubmitAction == SubmitActions.Create)
                    {
                        if(await IdentityCardExistsAlready(_customer.IdentityCard))
                        {
                            Errors.Add("A customer with that Identity card number already exists");
                            return;
                        }
                    }

                    _validator.ValidateAndThrow(_customer);

                    if (SubmitAction == SubmitActions.Create)
                    {
                        _unitOfWork.CustomersRepository.Add(_customer);
                    }

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

        private async Task<bool> IdentityCardExistsAlready(int identityCard)
        {
            var customerFromDb = await _unitOfWork.CustomersRepository
                .GetByIdentityCard(identityCard);

            if (customerFromDb != null)
                return true;

            return false;
        }

        private void GoBackAfterSubmit()
        {
            var result = ButtonResult.OK;

            var dialogParams = new DialogParameters { { ParamKeys.Customer, _customer } };

            RequestClose?.Invoke(new DialogResult(result, dialogParams));
        }
    }
}
