using System.Collections.Generic;
using System.Threading.Tasks;
using WPFInvoiceSystem.API.ApiResources;
using WPFInvoiceSystem.WPFClient.Models;

namespace WPFInvoiceSystem.WPFClient.Abstractions
{
    public interface IServiceTypesProvider : IDataProvider<ServiceTypeModel, ServiceTypeInputResource>
    {
        Task<IEnumerable<ServiceTypeModel>> GetAll();
    }
}
