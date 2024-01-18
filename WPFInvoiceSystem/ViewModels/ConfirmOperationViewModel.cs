using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Utils.Constants;

namespace WPFInvoiceSystem.ViewModels
{
    public class ConfirmOperationViewModel : BindableBase, IDialogAware
    {
        public DelegateCommand ConfirmCommand { get; }
        public DelegateCommand GoBackCommand { get; }
        public string Title => "Confirmar operación";

        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        
        public event Action<IDialogResult>? RequestClose;
        
        
        public ConfirmOperationViewModel()
        {
            _message = "Are you sure you want to proceed?";

            ConfirmCommand = new DelegateCommand(Confirm);

            GoBackCommand = new DelegateCommand(GoBack);
        }


        private void Confirm()
        {
            var result = ButtonResult.OK;
            RequestClose?.Invoke(new DialogResult(result));
        }

        private void GoBack()
        {
            var result = ButtonResult.Cancel;
            RequestClose?.Invoke(new DialogResult(result));
        }

        //INavigationAware methods implementation
        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            var message = parameters.GetValue<string>(ParamKeys.Message);
            if (message != null) Message = message;
        }
    }
}
