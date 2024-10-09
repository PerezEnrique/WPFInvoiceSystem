using Flurl.Http;
using Flurl.Http.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using WPFInvoiceSystem.API.ApiResources;
using WPFInvoiceSystem.WPFClient.Abstractions;
using WPFInvoiceSystem.WPFClient.Models;
using WPFInvoiceSystem.WPFClient.Utils;

namespace WPFInvoiceSystem.WPFClient.DataProviders
{
    public class ServicesProvider :
        DataProviderBase<ServiceModel, ServiceInputResource>,
        IServicesProvider
    {
        public ServicesProvider(IFlurlClientCache httpClient) 
            : base(httpClient, apiEndpoint: "services")
        {
        }

        public async Task<IEnumerable<ServiceModel>> GetAll()
        {
            return await ExecuteWithErrorHandling<IEnumerable<ServiceModel>>(async () =>
            {
                return await _httpClient
                    .Get(AppConstants.DefaultHttpClientName)
                    .Request(_apiEndpoint)
                    .GetJsonAsync<IEnumerable<ServiceModel>>();
            });
        }
    }
}
