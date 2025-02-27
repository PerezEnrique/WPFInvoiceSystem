﻿using Flurl.Http;
using Flurl.Http.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using WPFInvoiceSystem.WPFClient.Abstractions;
using WPFInvoiceSystem.WPFClient.ApiModels;
using WPFInvoiceSystem.WPFClient.Models;
using WPFInvoiceSystem.WPFClient.Utils.Constants;

namespace WPFInvoiceSystem.WPFClient.DataProviders
{
    public class CustomersProvider :
        DataProviderBase<CustomerModel, CustomerInputAPIModel>,
        ICustomersProvider
    {
        public CustomersProvider(IFlurlClientCache httpClient) 
            : base(httpClient, apiEndpoint: "customers")
        {
        }

        public async Task<IEnumerable<CustomerModel>> FindByIdentityCard(int identityCard)
        {
            return await ExecuteWithErrorHandling<IEnumerable<CustomerModel>>(async () =>
            {
                return await _httpClient
                    .Get(AppConstants.DefaultHttpClientName)
                    .Request(_apiEndpoint, "by-identity-card", identityCard)
                    .GetJsonAsync<IEnumerable<CustomerModel>>();
            });
        }

        public async  Task<IEnumerable<CustomerModel>> FindByName(string name)
        {
            return await ExecuteWithErrorHandling<IEnumerable<CustomerModel>>(async () =>
            {
                return await _httpClient
                    .Get(AppConstants.DefaultHttpClientName)
                    .Request(_apiEndpoint, "by-name", name)
                    .GetJsonAsync<IEnumerable<CustomerModel>>();
            });
        }
    }
}
