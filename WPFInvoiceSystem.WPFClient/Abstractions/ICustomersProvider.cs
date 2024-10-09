using System.Collections.Generic;
using System.Threading.Tasks;
using WPFInvoiceSystem.API.ApiResources;
using WPFInvoiceSystem.WPFClient.Models;

namespace WPFInvoiceSystem.WPFClient.Abstractions
{
    public interface ICustomersProvider : IDataProvider<CustomerModel, CustomerInputResource>
    {
        Task<IEnumerable<CustomerModel>> FindByIdentityCard(int identityCard);
        Task<IEnumerable<CustomerModel>> FindByName(string name);
    }
}
