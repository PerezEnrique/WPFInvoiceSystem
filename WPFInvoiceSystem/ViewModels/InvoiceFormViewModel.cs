using FluentValidation;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Domain;
using WPFInvoiceSystem.Domain.Modals;
using WPFInvoiceSystem.EventClasses;
using WPFInvoiceSystem.Services;
using WPFInvoiceSystem.Utils.Constants;

namespace WPFInvoiceSystem.ViewModels
{
    public class InvoiceFormViewModel : BaseParentFormViewModel<Invoice>, INavigationAware
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<Invoice> _validator;
        private Invoice _invoice;
        private InvoiceServicesModifiedEvent _invoiceServicesModifiedEvent;
        private IRegionNavigationJournal? _navigationJournal;
        private decimal _standardTaxRate;
        public DelegateCommand ConfirmCommand { get; }
        public DelegateCommand GoBackCommand { get; }
        public ObservableCollection<string> Errors { get; }

        private decimal _exempt;
        public decimal Exempt
        {
            get { return Math.Round(_exempt, 2); }
            set { SetProperty(ref _exempt, value); }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            private set { SetProperty(ref _isLoading, value); }
        }

        private string _submitAction;
        public string SubmitAction
        {
            get { return _submitAction; }
            private set { SetProperty(ref _submitAction, value); }
        }

        private decimal _tax;
        public decimal Tax
        {
            get { return Math.Round(_tax, 2); }
            set { SetProperty(ref _tax, value); }
        }

        private decimal _taxBase;
        public decimal TaxBase
        {
            get { return Math.Round(_taxBase, 2); }
            set { SetProperty(ref _taxBase, value); }
        }

        private decimal _total;
        public decimal Total
        {
            get { return _total; }
            set { SetProperty(ref _total, value); }
        }


        public InvoiceFormViewModel(
            IUnitOfWork unitOfWork,
            IRegionManager regionManager,
            IEventAggregator eventAggregator,
            IValidator<Invoice> validator) : base(regionManager)
        {
            _eventAggregator = eventAggregator;
            _invoice = new Invoice();
            _invoiceServicesModifiedEvent = _eventAggregator.GetEvent<InvoiceServicesModifiedEvent>();
            _invoiceServicesModifiedEvent.Subscribe(PerformInvoiceInnerCalcsAndUpdateProperties);
            _standardTaxRate = ConfigurationService.StandardTaxRate;
            _submitAction = string.Empty;
            _unitOfWork = unitOfWork;
            _validator = validator;

            Errors = new ObservableCollection<string>();

            ConfirmCommand = new DelegateCommand(
                executeMethod: async () => await GoToNextStep(_invoice),
                canExecuteMethod: () => !IsLoading)
                .ObservesProperty(() => IsLoading);

            GoBackCommand = new DelegateCommand(() => ReturnToPreviousStep(_invoice));
        }


        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                _navigationJournal = navigationContext.NavigationService.Journal;
                SubmitAction = navigationContext.Parameters.GetValue<string>(ParamKeys.SumbitAction);
                
                if(SubmitAction == SubmitActions.Update)
                {
                    IsLoading = true;

                    var invoiceId = navigationContext.Parameters.GetValue<int>(ParamKeys.InvoiceId);
                    var invoiceToUpdate = await Task.Run(async () => await _unitOfWork.InvoicesRepository.GetWithRelatedData(invoiceId));

                    if (invoiceToUpdate == null)
                        throw new Exception("Couldn't find invoice with provided Id");

                    _invoice = invoiceToUpdate;
                    Exempt = _invoice.Exempt;
                    Tax = _invoice.Tax;
                    TaxBase = _invoice.TaxBase;
                    Total = _invoice.Total;
                }

                PerformInvoiceInnerCalcsAndUpdateProperties();
                GoToNextStep(_invoice);

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

        private void PerformInvoiceInnerCalcsAndUpdateProperties()
        {
            _invoice.Calculate(_standardTaxRate);

            Exempt = _invoice.Exempt;
            TaxBase = _invoice.TaxBase;
            Tax = _invoice.Tax;
            Total = _invoice.Total;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            _unitOfWork.Dispose();

            /*Removes Form region so a new instance of this region can be safely
            registered in the future.*/
            _regionManager.Regions.Remove(RegionNames.FormRegion);
        }

        protected override void ExitForm()
        {
            _navigationJournal?.GoBack();
        }

        protected override void SetFormSteps()
        {
            _formSteps.Add(ViewNames.InvoiceMetadataSubform);
            _formSteps.Add(ViewNames.CustomerSubformView);
            _formSteps.Add(ViewNames.ServiceSubformView);
        }

        protected override async Task OnLastStepReached()
        {
            if(!IsLoading)
            {
                try
                {
                    IsLoading = true;
                    Errors.Clear();

                    if(SubmitAction == SubmitActions.Create)
                    {
                        if (await InvoiceNumberExistsAlready(_invoice.InvoiceNumber))
                        {
                            Errors.Add("Sorry an invoice with that number already exists");
                            return;
                        }
                    }

                    _validator.ValidateAndThrow(_invoice);

                    if (SubmitAction == SubmitActions.Create)
                    {
                        _unitOfWork.InvoicesRepository.Add(_invoice);
                    }

                    await _unitOfWork.CompleteAsync();

                    _navigationJournal!.GoBack();
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

        private async Task<bool> InvoiceNumberExistsAlready(int invoiceNumber)
        {
            var invoice = await _unitOfWork.InvoicesRepository.GetByInvoiceNumber(invoiceNumber);
            return invoice != null;
        }
    }
}
