using System.Collections.Generic;
using System.Threading.Tasks;
using WPFInvoiceSystem.WPFClient.ApiModels;
using WPFInvoiceSystem.WPFClient.Models;

namespace WPFInvoiceSystem.WPFClient.Abstractions
{
    public interface IServiceTypesProvider : IDataProvider<ServiceTypeModel, ServiceTypeInputAPIModel>
    {
        Task<IEnumerable<ServiceTypeModel>> GetAll();
    }
}
