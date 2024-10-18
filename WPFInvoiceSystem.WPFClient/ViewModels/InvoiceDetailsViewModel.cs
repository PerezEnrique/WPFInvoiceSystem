using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WPFInvoiceSystem.WPFClient.Abstractions;
using WPFInvoiceSystem.WPFClient.Exceptions;
using WPFInvoiceSystem.WPFClient.Models;
using WPFInvoiceSystem.WPFClient.Utils.Constants;

namespace WPFInvoiceSystem.WPFClient.ViewModels
{
    public class InvoiceDetailsViewModel : BindableBase, INavigationAware
    {
        public readonly IInvoicesProvider _invoicesProvider;
        private IRegionNavigationJournal? _navigationJournal;
        public DelegateCommand GoBackCommand { get; }
        public ObservableCollection<string> Errors { get; }

        private InvoiceModel? _invoice;
        public InvoiceModel? Invoice
        {
            get { return _invoice; }
            set { SetProperty(ref _invoice, value); }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        public InvoiceDetailsViewModel(IInvoicesProvider invoicesProvider)
        {
            _invoicesProvider = invoicesProvider;

            Errors = new ObservableCollection<string>();

            GoBackCommand = new DelegateCommand(
                executeMethod: () => _navigationJournal?.GoBack()
            );
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                IsLoading = true;
                _navigationJournal = navigationContext.NavigationService.Journal;

                int invoiceId = navigationContext.Parameters
                        .GetValue<int>(NavigationParamKeys.ItemId);

                await GetInvoice(invoiceId);
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

        private async Task GetInvoice(int id)
        {
            Invoice = await _invoicesProvider.Get(id);
        }
    }
}
