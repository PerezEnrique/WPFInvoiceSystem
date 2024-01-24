using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Domain.Modals;

namespace WPFInvoiceSystem.Domain
{
    public interface IReportsGenerator
    {
        void GenerateInvoicesReport(IList<Invoice> invoices);
    }
}
