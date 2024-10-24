using Prism.Mvvm;

namespace WPFInvoiceSystem.WPFClient.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "WPF Invoice System";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {
        }
    }
}
