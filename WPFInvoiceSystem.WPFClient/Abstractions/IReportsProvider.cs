using System.Threading.Tasks;
using WPFInvoiceSystem.WPFClient.ApiModels;

namespace WPFInvoiceSystem.WPFClient.Abstractions
{
    public interface IReportsProvider
    {
        Task RequestInvoicesReport(DateRangeFilterAPIModel filter);
    }
}
