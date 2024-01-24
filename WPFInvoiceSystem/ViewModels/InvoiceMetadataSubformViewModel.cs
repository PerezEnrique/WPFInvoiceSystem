using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Domain.Modals;
using WPFInvoiceSystem.Utils.Constants;

namespace WPFInvoiceSystem.ViewModels
{
    public class InvoiceMetadataSubformViewModel : BindableBase, INavigationAware
    {
        private Invoice? _invoice;

        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            set
            {
                if (_invoice != null) _invoice.Date = value;
                SetProperty(ref _date, value);
            }
        }

        private int _invoiceNumber;
        public int InvoiceNumber
        {
            get { return _invoiceNumber; }
            set
            {
                if (_invoice != null) _invoice.InvoiceNumber = value;
                SetProperty(ref _invoiceNumber, value);
            }
        }

        public InvoiceMetadataSubformViewModel()
        {
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
                InvoiceNumber = _invoice.InvoiceNumber;
                var invoiceDateHasBeenSet = _invoice.Date != DateTime.MinValue;

                if (invoiceDateHasBeenSet)
                {
                    Date = _invoice.Date;
                }
                else
                {
                    Date = DateTime.Now;
                }
            }
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}
