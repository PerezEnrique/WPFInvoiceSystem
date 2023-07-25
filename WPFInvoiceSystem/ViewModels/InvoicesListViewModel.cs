using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Domain;
using WPFInvoiceSystem.Domain.Entities;
using WPFInvoiceSystem.Utils.Constants;

namespace WPFInvoiceSystem.ViewModels
{
    public class InvoicesListViewModel : BindableBase, INavigationAware
    {
        public readonly IDialogService _dialogService;
        private readonly IRegionManager _regionManager;
        private readonly IUnitOfWork _unitOfWork;
        public DelegateCommand DeleteInvoiceCommand { get; }
        public DelegateCommand TogglePaymentStatusCommand { get; }
        public DelegateCommand GoToNewInvoiceFormCommand { get; }
        public DelegateCommand GoToInvoiceSearchCommand { get; }
        public DelegateCommand GoToUpdateInvoiceFormCommand { get; }
        public ObservableCollection<string> Errors { get; }

        private ObservableCollection<Invoice> _invoices;
        public ObservableCollection<Invoice> Invoices
        {
            get { return _invoices; }
            set { SetProperty(ref _invoices, value); }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        private Invoice? _selectedInvoice;
        public Invoice? SelectedInvoice
        {
            get { return _selectedInvoice; }
            set { SetProperty(ref _selectedInvoice, value); }
        }


        public InvoicesListViewModel(IDialogService dialogService, IRegionManager regionManager, IUnitOfWork unitOfWork)
        {
            _dialogService = dialogService;
            _invoices = new ObservableCollection<Invoice>();
            _regionManager = regionManager;
            _unitOfWork = unitOfWork;

            Errors = new ObservableCollection<string>();

            DeleteInvoiceCommand = new DelegateCommand(
                executeMethod: () => DeleteInvoice(),
                canExecuteMethod: () => SelectedInvoice != null
                )
                .ObservesProperty(() => SelectedInvoice);

            TogglePaymentStatusCommand = new DelegateCommand(
                executeMethod: async () => await TogglePaymentStatus(),
                canExecuteMethod: () => SelectedInvoice != null
                )
                .ObservesProperty(() => SelectedInvoice);

            GoToNewInvoiceFormCommand = new DelegateCommand(() => GoToInvoiceForm(SubmitActions.Create));

            GoToInvoiceSearchCommand = new DelegateCommand(GoToInvoiceSearch);

            GoToUpdateInvoiceFormCommand = new DelegateCommand(
                executeMethod: () => GoToInvoiceForm(SubmitActions.Update),
                canExecuteMethod: () => SelectedInvoice != null
                )
                .ObservesProperty(() => SelectedInvoice);
        }

        private void DeleteInvoice()
        {
            if (SelectedInvoice != null)
            {
                var dialogParams = new DialogParameters
                {
                    { ParamKeys.Message, "You are about to delete the selected invoice. Are you sure you want to continue?" }
                };

                _dialogService.ShowDialog(DialogNames.ConfirmOperationDialog, dialogParams, async result =>
                {
                    if (result.Result == ButtonResult.OK)
                    {
                        Errors.Clear();
                        IsLoading = true;
                        try
                        {
                            _unitOfWork.InvoicesRepository.Remove(SelectedInvoice);
                            await _unitOfWork.CompleteAsync();
                            Invoices.Clear();
                            await GetInvoices();
                        }
                        catch (Exception)
                        {
                            Errors.Add(UnexpectedErrorMessage.Message);
                        }
                        finally
                        {
                            IsLoading = false;
                        }
                    }
                });
            }
        }

        private async Task<IEnumerable<Invoice>> GetInvoices()
        {
            return await _unitOfWork.InvoicesRepository.GetAllWithCustomerData();
        }

        private void GoToInvoiceForm(string submitAction)
        {
            var navParams = new NavigationParameters
            {
                { ParamKeys.SumbitAction, submitAction } //Action to perform on submit
            };

            if (submitAction == SubmitActions.Update)
            {
                navParams.Add(ParamKeys.InvoiceId, SelectedInvoice?.Id);
            }

            _regionManager.RequestNavigate(
                RegionNames.ContentRegion,
                ViewNames.InvoiceFormView,
                navParams
                );
        }

        private void GoToInvoiceSearch()
        {
            _regionManager.RequestNavigate(
                RegionNames.ContentRegion,
                ViewNames.InvoiceSearchView);
        }
        
        private async Task TogglePaymentStatus()
        {
            Errors.Clear();

            if (SelectedInvoice != null && IsLoading != true)
            {
                IsLoading = true;

                try
                {
                    SelectedInvoice.IsPaid = !SelectedInvoice.IsPaid;
                    await _unitOfWork.CompleteAsync();
                    Invoices.Clear();
                    Invoices.AddRange(await GetInvoices());
                }
                catch (Exception)
                {
                    Errors.Add(UnexpectedErrorMessage.Message);
                }
                finally
                {
                    IsLoading = false;
                }
            }
        }

        //INavigationAware methods implementation
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            _unitOfWork.Dispose();
        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            IsLoading = true;

            try
            {
                Invoices.AddRange(await Task.Run(async () => await GetInvoices()));
            }
            catch (Exception)
            {
                Errors.Add(UnexpectedErrorMessage.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
