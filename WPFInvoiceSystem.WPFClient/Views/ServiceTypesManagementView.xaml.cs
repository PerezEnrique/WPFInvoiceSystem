using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFInvoiceSystem.WPFClient.ViewModels;

namespace WPFInvoiceSystem.WPFClient.Views
{
    /// <summary>
    /// Interaction logic for ServiceTypesManagementView.xaml
    /// </summary>
    public partial class ServiceTypesManagementView : UserControl
    {
        public ServiceTypesManagementView()
        {
            InitializeComponent();
        }

        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            var confimationDialogResult = MessageBox.Show(
                messageBoxText: "Are ypu sure you want to delete the selected item",
                caption: "WPF Invoice System - Delete item",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
                );

            if (confimationDialogResult == MessageBoxResult.Yes)
            {
                ((ServiceTypesManagementViewModel)this.DataContext)
                    .DeleteServiceTypeCommand.Execute();
            }
        }
    }
}
