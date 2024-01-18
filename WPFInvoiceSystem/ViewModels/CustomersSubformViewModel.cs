using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Domain.Modals;
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

            ShowCreateCustomerFormCommand = new DelegateCommand(() => ShowCustomerForm(SubmitActions.Create));

            ShowEditCustomerFormCommand = new DelegateCommand(
                executeMethod: () => ShowCustomerForm(SubmitActions.Update),
                canExecuteMethod: () => Customer != null
                )
                .ObservesProperty(() => Customer);

            ShowCustomerSearchCommand = new DelegateCommand(ShowCustomerSearch);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _invoice = navigationContext.Parameters.GetValue<Invoice>(ParamKeys.FormProduct);

            if (_invoice != null)
            {
                Customer = _invoice.Customer;
            }
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        private void ShowCustomerForm(string submitAction)
        {
            var dialogParams = new DialogParameters
            {
                { ParamKeys.SumbitAction, submitAction }
            };

            if (submitAction == SubmitActions.Update)
            {
                dialogParams.Add(ParamKeys.CustomerId, Customer?.Id);
            }

            _dialogService.ShowDialog(DialogNames.CustomersFormDialog, dialogParams, result => SetCustomerOnPropertyAndInvoice(result));
        }

        private void ShowCustomerSearch()
        {
            _dialogService.ShowDialog(DialogNames.CustomersSearchDialog, result => SetCustomerOnPropertyAndInvoice(result));
        }

        private void SetCustomerOnPropertyAndInvoice(IDialogResult result)
        {
            if (result.Result == ButtonResult.OK)
            {
                Customer = null;
                Customer = result.Parameters.GetValue<Customer>(ParamKeys.Customer);
                if (_invoice != null) _invoice.Customer = Customer;
            }
        }
    }
}
