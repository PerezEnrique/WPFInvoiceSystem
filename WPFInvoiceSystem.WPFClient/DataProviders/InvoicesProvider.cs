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
    public class InvoicesProvider :
        DataProviderBase<InvoiceModel, InvoiceInputResource>,
        IInvoicesProvider
    {
        public InvoicesProvider(IFlurlClientCache httpClient)
            : base(httpClient, apiEndpoint: "invoices")
        {
        }

        public async Task<IEnumerable<InvoiceModel>> Find(InvoicesFilterResource filter)
        {
            return await ExecuteWithErrorHandling<IEnumerable<InvoiceModel>>(async () =>
            {
                return await _httpClient
                .Get(AppConstants.DefaultHttpClientName)
                .Request(_apiEndpoint, "find")
                .SetQueryParams(filter)
                .GetJsonAsync<IEnumerable<InvoiceModel>>();
            });
        }

        public async Task<IEnumerable<InvoiceModel>> GetLastTenInvoices()
        {
            return await ExecuteWithErrorHandling<IEnumerable<InvoiceModel>>(async () =>
            {
                return await _httpClient
                    .Get(AppConstants.DefaultHttpClientName)
                    .Request(_apiEndpoint, "recent")
                    .GetJsonAsync<IEnumerable<InvoiceModel>>();
            });
        }
    }
}
