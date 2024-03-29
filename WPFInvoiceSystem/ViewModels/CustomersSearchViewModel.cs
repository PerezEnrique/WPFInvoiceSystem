﻿using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Domain;
using WPFInvoiceSystem.Domain.Modals;
using WPFInvoiceSystem.Utils.Constants;

namespace WPFInvoiceSystem.ViewModels
{
    public class CustomersSearchViewModel : BaseSearchViewModel<Customer>, IDialogAware
    {
        public DelegateCommand ConfirmCommand { get; }
        public DelegateCommand GoBackCommand { get; }
        public DelegateCommand<string> SearchCommand { get; }
        
        public string Title => "Invoice System - Customer Search";


        public event Action<IDialogResult>? RequestClose;
        
        
        public CustomersSearchViewModel(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            ConfirmCommand = new DelegateCommand(
                executeMethod: GoBackAfterSelectItem,
                canExecuteMethod: () => SelectedItem != null
                )
                .ObservesProperty(() => SelectedItem);

            GoBackCommand = new DelegateCommand(GoBack);

           SearchCommand = new DelegateCommand<string>(
                executeMethod: async (searchCriterion) => await CallSearchMethod(searchCriterion),
                canExecuteMethod: (arg) => !string.IsNullOrWhiteSpace(Query) && !IsLoading
                )
                .ObservesProperty(() => Query)
                .ObservesProperty(() => IsLoading);
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        private async Task CallSearchMethod(string searchCriterion)
        {
            if (searchCriterion == SearchCriteria.ByName)
            {
                await Search(c => c.Name.Contains(Query));
            }

            if (searchCriterion == SearchCriteria.ByIdentityCard)
            {
                await Search(c => c.IdentityCard.ToString() == Query);
            }
        }

        private void GoBack()
        {
            var result = ButtonResult.Cancel;
            RequestClose?.Invoke(new DialogResult(result));
        }

        private void GoBackAfterSelectItem()
        {
            var result = ButtonResult.OK;

            var dialogParams = new DialogParameters { { ParamKeys.Customer, SelectedItem } };

            RequestClose?.Invoke(new DialogResult(result, dialogParams));
        }
    }
}
