using System.Threading.Tasks;
using WPFInvoiceSystem.API.ApiResources;

namespace WPFInvoiceSystem.WPFClient.Abstractions
{
    public interface IReportsProvider
    {
        Task RequestInvoicesReport(DateRangeFilterResource filter);
    }
}
