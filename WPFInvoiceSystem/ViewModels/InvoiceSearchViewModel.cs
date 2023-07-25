using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Domain;
using WPFInvoiceSystem.Domain.Entities;
using WPFInvoiceSystem.Utils.Constants;

namespace WPFInvoiceSystem.ViewModels
{
    public class InvoiceSearchViewModel : BindableBase, INavigationAware
    {
        private readonly IDialogService _dialogService;
        private readonly IRegionManager _regionManager;
        protected readonly IUnitOfWork _unitOfWork;
        private IRegionNavigationJournal? _navigationJournal;
        public DelegateCommand DeleteInvoiceCommand { get; }
        public DelegateCommand GoBackCommand { get; }
        public DelegateCommand GoToUpdateInvoiceFormCommand { get; }
        public DelegateCommand SearchCommand { get; }
        public DelegateCommand TogglePaymentStatusCommand { get; }
        public ObservableCollection<string> Errors { get; }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            private set { SetProperty(ref _isLoading, value); }
        }

        private Invoice? _invoice;
        public Invoice? Invoice
        {
            get { return _invoice; }
            set
            {
                if (value != null) FormatedDate = value.Date.ToString("d", new CultureInfo("es-VE"));
                SetProperty(ref _invoice, value);
            }
        }

        private string? _formatedDate;
        public string? FormatedDate
        {
            get { return _formatedDate; }
            set { SetProperty(ref _formatedDate, value); }
        }


        private int _query;
        public int Query
        {
            get { return _query; }
            set { SetProperty(ref _query, value); }
        }


        public InvoiceSearchViewModel(IUnitOfWork unitOfWork, IRegionManager regionManager, IDialogService dialogService)
        {
            _dialogService = dialogService;
            _regionManager = regionManager;
            _unitOfWork = unitOfWork;

            Errors = new ObservableCollection<string>();

            DeleteInvoiceCommand = new DelegateCommand(
                executeMethod: DeleteInvoice,
                canExecuteMethod: () => Invoice != null && !IsLoading
                )
                .ObservesProperty(() => Invoice)
                .ObservesProperty(() => IsLoading);

            GoBackCommand = new DelegateCommand(() => _navigationJournal?.GoBack());

            GoToUpdateInvoiceFormCommand = new DelegateCommand(
                executeMethod: GoToInvoiceForm,
                canExecuteMethod: () => Invoice != null)
                .ObservesProperty(() => Invoice);

            SearchCommand = new DelegateCommand(
                executeMethod: async () => await Search(),
                canExecuteMethod: () => Query > 0 && !IsLoading
                )
                .ObservesProperty(() => Query)
                .ObservesProperty(() => IsLoading);

            TogglePaymentStatusCommand = new DelegateCommand(
                executeMethod: async () => await TogglePaymentStatus(),
                canExecuteMethod: () => Invoice != null
                )
                .ObservesProperty(() => Invoice);
        }


        private void DeleteInvoice()
        {
            if (Invoice != null && !IsLoading)
            {
                Errors.Clear();

                var dialogParams = new DialogParameters
                {
                    { ParamKeys.Message, "You are about to delete the selected invoice. Are you sure you want to continue?" }
                };

                _dialogService.ShowDialog(DialogNames.ConfirmOperationDialog, dialogParams, async result =>
                {
                    if (result.Result == ButtonResult.OK)
                    {
                        IsLoading = true;
                        try
                        {
                            _unitOfWork.InvoicesRepository.Remove(Invoice);
                            await _unitOfWork.CompleteAsync();
                            Invoice = null;
                            FormatedDate = null;
                            Query = default(int);
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

        private void GoToInvoiceForm()
        {
            var navParams = new NavigationParameters
            {
                { ParamKeys.SumbitAction, SubmitActions.Update },
                { ParamKeys.InvoiceId, Invoice?.Id }
            };

            _regionManager.RequestNavigate(
                RegionNames.ContentRegion,
                ViewNames.InvoiceFormView,
                navParams
                );
        }

        private async Task Search()
        {
            if(!IsLoading)
            {
                IsLoading = true;
                Errors.Clear();

                try
                {
                    var invoice = await _unitOfWork.InvoicesRepository.GetByInvoiceNumberWithRelatedData(Query);
                    if (invoice == null)
                    {
                        Errors.Add("Sorry, we couldn't find any results");
                        return;
                    }

                    Invoice = invoice;
                    Query = default(int);
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

        private async Task TogglePaymentStatus()
        {
            Errors.Clear();

            if (Invoice != null && !IsLoading)
            {
                IsLoading = true;

                try
                {
                    Invoice.IsPaid = !Invoice.IsPaid;
                    await _unitOfWork.CompleteAsync();
                    
                    //Invoice.IsPaid change won't be notified to View, the following lines will:
                    Query = Invoice.InvoiceNumber;
                    Invoice = null; 
                    IsLoading = false; //Because Search method needs IsLoading to be false to execute
                    SearchCommand.Execute();
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
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _navigationJournal = navigationContext.NavigationService.Journal;
        }
    }
}
