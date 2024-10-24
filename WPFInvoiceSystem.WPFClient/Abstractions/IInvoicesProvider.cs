using System.Collections.Generic;
using System.Threading.Tasks;
using WPFInvoiceSystem.WPFClient.ApiModels;
using WPFInvoiceSystem.WPFClient.Models;

namespace WPFInvoiceSystem.WPFClient.Abstractions
{
    public interface IInvoicesProvider : IDataProvider<InvoiceModel, InvoiceInputAPIModel>
    {
        Task ChangeInvoicePaymentStatus(int id);
        Task<IEnumerable<InvoiceModel>> Find(InvoicesFilterAPIModel filter);
        Task<IEnumerable<InvoiceModel>> GetLastTenInvoices();
    }
}
