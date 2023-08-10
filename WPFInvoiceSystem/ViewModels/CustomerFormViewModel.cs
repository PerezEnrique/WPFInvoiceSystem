using FluentValidation;
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
    public class CustomerFormViewModel : BaseFormViewModel<Customer>, IDialogAware
    {
        private Customer _customer;
        public DelegateCommand ConfirmCommand { get; }
        public DelegateCommand GoBackCommand { get; }
        public string Title => "Invoice System - Customer Form";

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

        public event Action<IDialogResult>? RequestClose;


        public CustomerFormViewModel(
            IUnitOfWork unitOfWork,
            IValidator<Customer> validator
            ) : base(unitOfWork, validator)
                {
                    _customer = new Customer();
                    _name = string.Empty;
                    _address = string.Empty;
                    _phone = string.Empty;

            ConfirmCommand = new DelegateCommand(
                executeMethod: async () => await Submit(),
                canExecuteMethod: () => !IsLoading
                )
                .ObservesProperty(() => IsLoading);

            GoBackCommand = new DelegateCommand(GoBack);

        }

        private void GoBack()
        {
            var result = ButtonResult.Cancel;
            RequestClose?.Invoke(new DialogResult(result));
        }

        //BaseFormViewModel methods overriding
        protected override async Task<string?> AditionalValidation()
        {
            Customer? customer;

            customer = await _unitOfWork.CustomersRepository.GetByIdentityCard(_customer.IdentityCard);
            if (customer != null) return "A customer with that Identity card number already exists";

            return null;
        }

        protected override Customer ComposeObjectToSave()
        {
            /*_customer here will be either a customer from db to update 
            or a new customer object instanciated in the constructor*/
            _customer.Address = Address;
            _customer.Birthdate = Birthdate;
            _customer.IdentityCard = IdentityCard;
            _customer.Name = Name;
            _customer.Phone = Phone;

            return _customer;
        }

        protected override void OnSavingComplete()
        {
            //Set dialog result
            var result = ButtonResult.OK;

            //Set params to return
            var dialogParams = new DialogParameters { { ParamKeys.Customer, _customer } };

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
                SubmitAction = parameters.GetValue<string>(ParamKeys.SumbitAction);

                /*if the submit action is set to update and a valid id is received 
                it replaces the object instanciated on the constructor with the result from the db*/
                if (SubmitAction == SubmitActions.Update)
                {
                    var customerId = parameters.GetValue<int>(ParamKeys.CustomerId);
                    if (customerId <= 0)
                    {
                        throw new Exception("Id coming from params is invalid");
                    }

                    var customerToUpdate = await Task.Run(async () => await _unitOfWork.CustomersRepository.Get(customerId));

                    if (customerToUpdate == null)
                    {
                        throw new Exception("Couldn't find customer with provided Id");
                    }

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
    }
}
