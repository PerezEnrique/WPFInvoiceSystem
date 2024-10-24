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
    /// Interaction logic for InvoicesFormView.xaml
    /// </summary>
    public partial class InvoicesFormView : UserControl
    {
        public InvoicesFormView()
        {
            InitializeComponent();
        }

        private void PreviousStepButton_Click(object sender, RoutedEventArgs e)
        {
            int newIndex = FormSectionsTabControl.SelectedIndex - 1;

            bool firstStepReached = newIndex < 0;

            if (firstStepReached)
                ((InvoicesFormViewModel)DataContext).GoBackCommand.Execute();

            FormSectionsTabControl.SelectedIndex = newIndex;
        }

        private void NextStepButton_Click(object sender, RoutedEventArgs e)
        {
            int newIndex = FormSectionsTabControl.SelectedIndex + 1;

            bool lastStepReached = newIndex >= FormSectionsTabControl.Items.Count;

            if (lastStepReached)
                ((InvoicesFormViewModel)DataContext).SubmitInvoiceCommand.Execute();

            FormSectionsTabControl.SelectedIndex = newIndex;
        }
    }
}
