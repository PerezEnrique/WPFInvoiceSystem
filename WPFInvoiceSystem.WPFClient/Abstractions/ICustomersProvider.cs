using System.Collections.Generic;
using System.Threading.Tasks;
using WPFInvoiceSystem.WPFClient.ApiModels;
using WPFInvoiceSystem.WPFClient.Models;

namespace WPFInvoiceSystem.WPFClient.Abstractions
{
    public interface ICustomersProvider : IDataProvider<CustomerModel, CustomerInputAPIModel>
    {
        Task<IEnumerable<CustomerModel>> FindByIdentityCard(int identityCard);
        Task<IEnumerable<CustomerModel>> FindByName(string name);
    }
}
