using Prism.Regions;
using System.Windows;
using WPFInvoiceSystem.WPFClient.Utils.Constants;

namespace WPFInvoiceSystem.WPFClient.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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
                RegionNames.MainRegion,
                ViewNames.HomeView
                );
        }
    }
}
