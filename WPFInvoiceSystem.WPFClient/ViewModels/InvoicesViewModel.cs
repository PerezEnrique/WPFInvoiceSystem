using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Linq;
using WPFInvoiceSystem.WPFClient.Utils.Constants;

namespace WPFInvoiceSystem.WPFClient.ViewModels
{
    public class InvoicesViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private IRegionNavigationJournal? _navigationJournal;
        public DelegateCommand GoBackCommand { get; }
        public InvoicesViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            GoBackCommand = new DelegateCommand(executeMethod: CloseView);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _navigationJournal = navigationContext.NavigationService.Journal;
        }

        private void CloseView()
        {
            object? thisView = _regionManager.Regions[RegionNames.MainRegion].ActiveViews.First();
            _navigationJournal!.GoBack();
            _regionManager.Regions[RegionNames.MainRegion].Remove(thisView);
        }
    }
}
