using System.Collections.Generic;
using System.Threading.Tasks;
using WPFInvoiceSystem.API.ApiResources;
using WPFInvoiceSystem.WPFClient.Models;

namespace WPFInvoiceSystem.WPFClient.Abstractions
{
    public interface IServicesProvider : IDataProvider<ServiceModel, ServiceInputResource>
    {
        Task<IEnumerable<ServiceModel>> FindByName(string name);
        Task<IEnumerable<ServiceModel>> GetAll();
    }
}
