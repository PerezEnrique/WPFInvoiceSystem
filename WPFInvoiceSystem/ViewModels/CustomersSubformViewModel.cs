using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Domain.Entities;
using WPFInvoiceSystem.Utils.Constants;

namespace WPFInvoiceSystem.ViewModels
{
    public class CustomersSubformViewModel : BindableBase, INavigationAware
    {
        public readonly IDialogService _dialogService;
        private Invoice? _invoice;
        public DelegateCommand ShowCreateCustomerFormCommand { get; }
        public DelegateCommand ShowCustomerSearchCommand { get; }
        public DelegateCommand ShowEditCustomerFormCommand { get; }

        private Customer? _customer;
        public Customer? Customer
        {
            get { return _customer; }
            set { SetProperty(ref _customer, value); }
        }


        public CustomersSubformViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;

            ShowCreateCustomerFormCommand = new DelegateCommand(ShowCreateCustomerForm);

            ShowEditCustomerFormCommand = new DelegateCommand(
                executeMethod: ShowEditCustomerForm,
                canExecuteMethod: () => Customer != null
                )
                .ObservesProperty(() => Customer);

            ShowCustomerSearchCommand = new DelegateCommand(ShowCustomerSearch);
        }


        private void ShowCreateCustomerForm()
        {
            var dialogParams = new DialogParameters
            {
                { ParamKeys.SumbitAction, SubmitActions.Create }
            };

            _dialogService.ShowDialog(DialogNames.CustomersFormDialog, dialogParams, result => SetCustomer(result));
        }

        private void ShowCustomerSearch()
        {
            _dialogService.ShowDialog(DialogNames.CustomersSearchDialog, result => SetCustomer(result));
        }

        private void ShowEditCustomerForm()
        {
            var dialogParams = new DialogParameters
            {
                { ParamKeys.SumbitAction, SubmitActions.Update },
                { ParamKeys.CustomerId, Customer?.Id }
            };

            _dialogService.ShowDialog(DialogNames.CustomersFormDialog, dialogParams, result => SetCustomer(result));
        }

        private void SetCustomer(IDialogResult result)
        {
            if (result.Result == ButtonResult.OK)
            {
                /*It sets Customer to null first, to force the property changed notification when editing,
                 case where the customer coming from the dialog, references the same place 
                 in memory as this view model Customer property*/
                Customer = null;
                Customer = result.Parameters.GetValue<Customer>(ParamKeys.Customer);
                if (_invoice != null) _invoice.Customer = Customer;
            }
        }

        //INavigationAware methods implementation
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _invoice = navigationContext.Parameters.GetValue<Invoice>(ParamKeys.Invoice);

            if (_invoice != null)
            {
                Customer = _invoice.Customer;
            }
        }
    }
}
