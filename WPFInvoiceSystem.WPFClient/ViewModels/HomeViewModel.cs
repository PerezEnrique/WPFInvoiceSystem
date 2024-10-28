using Flurl.Http;
using Flurl.Http.Configuration;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Threading;
using WPFInvoiceSystem.WPFClient.ApiModels;
using WPFInvoiceSystem.WPFClient.Exceptions;
using WPFInvoiceSystem.WPFClient.Utils.Constants;

namespace WPFInvoiceSystem.WPFClient.ViewModels
{
    public class HomeViewModel : BindableBase
    {
        private readonly IFlurlClientCache _httpClient;
        private readonly IRegionManager _regionManager;

        public ObservableCollection<string> Errors { get; }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        private string _successMessage;
        public string SuccessMessage
        {
            get { return _successMessage; }
            set { SetProperty(ref _successMessage, value); }
        }

        public DelegateCommand GoToInvoicesListCommand { get; set; }
        public DelegateCommand GoToServicesManagementCommand { get; set; }
        public DelegateCommand GoToServiceTypesManagementCommand { get; set; }
        public DelegateCommand ClearDbCommand {  get; set; }

        public HomeViewModel(
            IRegionManager regionManager, 
            IFlurlClientCache httpClient
            )
        {
            _httpClient = httpClient;
            _regionManager = regionManager;

            Errors = new ObservableCollection<string>();
            
            ClearDbCommand = new DelegateCommand(ClearDb);

            GoToInvoicesListCommand = new DelegateCommand(
                () => GoToView(ViewNames.InvoicesView)
                );

            GoToServicesManagementCommand = new DelegateCommand(
                () => GoToView(ViewNames.ServicesManagementView)
                );
            
            GoToServiceTypesManagementCommand = new DelegateCommand(
                () => GoToView(ViewNames.ServiceTypesManagementView)
                );
        }

        private async void ClearDb()
        {
            try
            {
                IsLoading = true;

                await DoClearDb();
                SuccessMessage = "Db successfully cleared";

                var timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(3)
                };

                timer.Tick += (s, e) =>
                {
                    SuccessMessage = string.Empty;
                    timer.Stop();
                };

                timer.Start();
            }
            catch (ExpectedServerErrorsException ex)
            {
                Errors.AddRange(ex.Errors);
            }
            catch (Exception)
            {
                Errors.Add("An unexpected error ocurred. Please try again.");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task DoClearDb()
        {
            try
            {
                await _httpClient
                   .Get(AppConstants.DefaultHttpClientName)
                   .Request("db")
                   .DeleteAsync();
            }
            catch (FlurlHttpException ex)
            {
                var problemDetails = await ex.GetResponseJsonAsync<ProblemDetailsResponse>();

                if (problemDetails != null)
                    throw new ExpectedServerErrorsException(problemDetails.errors);

                throw new Exception(ex.Message);
            }
        }

        private void GoToView(string viewName)
        {
            _regionManager.RequestNavigate(
                RegionNames.MainRegion,
                viewName
                );
        }
    }
}
