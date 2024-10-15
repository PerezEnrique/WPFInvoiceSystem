using System.Collections.Generic;
using System.Threading.Tasks;
using WPFInvoiceSystem.API.ApiResources;
using WPFInvoiceSystem.WPFClient.Models;

namespace WPFInvoiceSystem.WPFClient.Abstractions
{
    public interface IInvoicesProvider : IDataProvider<InvoiceModel, InvoiceInputResource>
    {
        Task ChangeInvoicePaymentStatus(int id);
        Task<IEnumerable<InvoiceModel>> Find(InvoicesFilterResource filter);
        Task<IEnumerable<InvoiceModel>> GetLastTenInvoices();
    }
}
