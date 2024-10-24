using Flurl.Http;
using Flurl.Http.Configuration;
using System;
using System.Threading.Tasks;
using WPFInvoiceSystem.WPFClient.Abstractions;
using WPFInvoiceSystem.WPFClient.ApiModels;
using WPFInvoiceSystem.WPFClient.Exceptions;
using WPFInvoiceSystem.WPFClient.Utils.Constants;

namespace WPFInvoiceSystem.WPFClient.DataProviders
{
    public class ReportsProvider : IReportsProvider
    {
        private readonly string _apiEndpoint;
        private readonly IFlurlClientCache _httpClient;

        public ReportsProvider(IFlurlClientCache httpClient)
        {
            _apiEndpoint = "reports";    
            _httpClient = httpClient;
        }

        public async Task RequestInvoicesReport(DateRangeFilterAPIModel filter)
        {
            try
            {
                string localDestinationPath = Environment
                    .GetFolderPath(Environment.SpecialFolder.Desktop);

                string fileLocalName = "invoices-report.xlsx";

                await _httpClient
                    .Get(AppConstants.DefaultHttpClientName)
                    .Request(_apiEndpoint, "invoices")
                    .SetQueryParams(filter)
                    .DownloadFileAsync(localDestinationPath, fileLocalName);
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
