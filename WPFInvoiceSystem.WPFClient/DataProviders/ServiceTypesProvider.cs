using Flurl.Http;
using Flurl.Http.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using WPFInvoiceSystem.API.ApiResources;
using WPFInvoiceSystem.WPFClient.Abstractions;
using WPFInvoiceSystem.WPFClient.Models;
using WPFInvoiceSystem.WPFClient.Utils.Constants;

namespace WPFInvoiceSystem.WPFClient.DataProviders
{
    public class ServiceTypesProvider :
        DataProviderBase<ServiceTypeModel, ServiceTypeInputResource>,
        IServiceTypesProvider
    {
        public ServiceTypesProvider(IFlurlClientCache httpClient)
            : base(httpClient, apiEndpoint: "service-types")
        {
        }

        public async Task<IEnumerable<ServiceTypeModel>> GetAll()
        {
            return await ExecuteWithErrorHandling<IEnumerable<ServiceTypeModel>>(async () =>
            {
                return await _httpClient
                    .Get(AppConstants.DefaultHttpClientName)
                    .Request(_apiEndpoint)
                    .GetJsonAsync<IEnumerable<ServiceTypeModel>>();
            });
        }
    }
}
