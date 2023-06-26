using Prism.Regions;
using System.Windows;
using WPFInvoiceSystem.Utils.Constants;

namespace WPFInvoiceSystem.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        readonly IRegionManager _regionManager;
        public MainWindow(IRegionManager regionManager)
        {
            InitializeComponent();
            _regionManager = regionManager;
            this.Loaded += NavigateToInitialView;
        }

        private void NavigateToInitialView(object sender, RoutedEventArgs e)
        {
            _regionManager.RequestNavigate(
                RegionNames.ContentRegion,
                ViewNames.InvoicesListView
                );
        }
    }
}
