using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using WPFInvoiceSystem.WPFClient.Utils.Constants;

namespace WPFInvoiceSystem.WPFClient.ViewModels
{
    public class HomeViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        public DelegateCommand GoToInvoicesListCommand { get; set; }

        public HomeViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            GoToInvoicesListCommand = new DelegateCommand(
                () => GoToView(ViewNames.InvoicesView)
                );
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
