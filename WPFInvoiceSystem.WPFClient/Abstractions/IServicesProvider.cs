using System.Collections.Generic;
using System.Threading.Tasks;
using WPFInvoiceSystem.WPFClient.ApiModels;
using WPFInvoiceSystem.WPFClient.Models;

namespace WPFInvoiceSystem.WPFClient.Abstractions
{
    public interface IServicesProvider : IDataProvider<ServiceModel, ServiceInputAPIModel>
    {
        Task<IEnumerable<ServiceModel>> FindByName(string name);
        Task<IEnumerable<ServiceModel>> GetAll();
    }
}
