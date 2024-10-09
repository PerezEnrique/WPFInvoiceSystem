using Flurl.Http;
using Flurl.Http.Configuration;
using System;
using System.Threading.Tasks;
using WPFInvoiceSystem.WPFClient.Abstractions;
using WPFInvoiceSystem.WPFClient.ApiModels;
using WPFInvoiceSystem.WPFClient.Exceptions;
using WPFInvoiceSystem.WPFClient.Utils;

namespace WPFInvoiceSystem.WPFClient.DataProviders
{
    public class DataProviderBase<ReturnType, InputType> : IDataProvider<ReturnType, InputType>
    {
        protected readonly string _apiEndpoint;
        protected readonly IFlurlClientCache _httpClient;
        public DataProviderBase(IFlurlClientCache httpClient, string apiEndpoint)
        {
            _apiEndpoint = apiEndpoint;
            _httpClient = httpClient;
        }

        public virtual async Task<ReturnType> Create(InputType inputData)
        {
            return await ExecuteWithErrorHandling<ReturnType>(async () =>
            {
                return await _httpClient
                    .Get(AppConstants.DefaultHttpClientName)
                    .Request(_apiEndpoint)
                    .PostJsonAsync(inputData)
                    .ReceiveJson<ReturnType>();
            });
        }

        public virtual async Task Delete(int id)
        {
            await ExecuteWithErrorHandling(async () =>
            {
                await _httpClient
                   .Get(AppConstants.DefaultHttpClientName)
                   .Request(_apiEndpoint, id)
                   .DeleteAsync();
            });
        }

        public virtual async Task<ReturnType> Get(int id)
        {
            return await ExecuteWithErrorHandling<ReturnType>(async () =>
            {
                return await _httpClient
                    .Get(AppConstants.DefaultHttpClientName)
                    .Request(_apiEndpoint, id)
                    .GetJsonAsync<ReturnType>();
            });
        }

        public virtual async Task<ReturnType> Update(int id, InputType inputData)
        {
            return await ExecuteWithErrorHandling(async () =>
            {
                return await _httpClient
                    .Get(AppConstants.DefaultHttpClientName)
                    .Request(_apiEndpoint, id)
                    .PutJsonAsync(inputData)
                    .ReceiveJson<ReturnType>();
            });
        }

        protected async Task ExecuteWithErrorHandling(Func<Task> operation)
        {
            try
            {
                await operation();
            }
            catch (FlurlHttpException ex)
            {
                var problemDetails = await ex.GetResponseJsonAsync<ProblemDetailsResponse>();

                if (problemDetails != null)
                    throw new ExpectedServerErrorsException(problemDetails.errors);

                throw new Exception(ex.Message);
            }
        }

        protected async Task<T> ExecuteWithErrorHandling<T>(Func<Task<T>> operation)
        {
            try
            {
                return await operation();
            }
            catch (FlurlHttpException ex)
            {
                var problemDetails = await ex.GetResponseJsonAsync<ProblemDetailsResponse>();

                if (problemDetails != null)
                    throw new ExpectedServerErrorsException(problemDetails.errors);

                throw new Exception(ex.Message);
            }
        }
    }
}
