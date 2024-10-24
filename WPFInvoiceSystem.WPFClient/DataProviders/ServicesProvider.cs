using Flurl.Http;
using Flurl.Http.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using WPFInvoiceSystem.WPFClient.Abstractions;
using WPFInvoiceSystem.WPFClient.ApiModels;
using WPFInvoiceSystem.WPFClient.Models;
using WPFInvoiceSystem.WPFClient.Utils.Constants;

namespace WPFInvoiceSystem.WPFClient.DataProviders
{
    public class ServicesProvider :
        DataProviderBase<ServiceModel, ServiceInputAPIModel>,
        IServicesProvider
    {
        public ServicesProvider(IFlurlClientCache httpClient) 
            : base(httpClient, apiEndpoint: "services")
        {
        }

        public async Task<IEnumerable<ServiceModel>> FindByName(string name)
        {
            return await ExecuteWithErrorHandling<IEnumerable<ServiceModel>>(async () =>
            {
                return await _httpClient
                    .Get(AppConstants.DefaultHttpClientName)
                    .Request(_apiEndpoint, "by-name", name)
                    .GetJsonAsync<IEnumerable<ServiceModel>>();
            });
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
