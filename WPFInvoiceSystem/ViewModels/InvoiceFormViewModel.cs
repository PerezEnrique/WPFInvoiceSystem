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
using WPFInvoiceSystem.Domain.Entities;
using WPFInvoiceSystem.EventClasses;
using WPFInvoiceSystem.Services;
using WPFInvoiceSystem.Utils.Constants;

namespace WPFInvoiceSystem.ViewModels
{
    public class InvoiceFormViewModel : BaseFormViewModel<Invoice>, INavigationAware
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly List<string> _formSteps;
        private readonly IRegionManager _regionManager;
        private Invoice _invoice;
        private InvoiceServicesModifiedEvent _invoiceServicesModifiedEvent;
        private IRegionNavigationJournal? _navigationJournal;
        private decimal _standardTaxRate;

        public DelegateCommand ConfirmCommand { get; }
        public DelegateCommand GoBackCommand { get; }

        private int _currentFormStep;
        public int CurrentFormStep
        {
            get { return _currentFormStep; }
            set { SetProperty(ref _currentFormStep, value); }
        }

        private decimal _exempt;
        public decimal Exempt
        {
            get { return Math.Round(_exempt, 2); }
            set { SetProperty(ref _exempt, value); }
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
            IValidator<Invoice> validator) : base(unitOfWork, validator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _formSteps = new List<string>();
            _invoice = new Invoice();
            _invoiceServicesModifiedEvent = _eventAggregator.GetEvent<InvoiceServicesModifiedEvent>();
            _invoiceServicesModifiedEvent.Subscribe(CalculateInvoice);
            _standardTaxRate = ConfigurationService.StandardTaxRate;

            ConfirmCommand = new DelegateCommand(async () => await GoToNextStep());
            GoBackCommand = new DelegateCommand(ReturnToPreviousStep);

            SetFormSteps();
        }


        private void CalculateInvoice()
        {
            _invoice.Calculate(_standardTaxRate);

            Exempt = _invoice.Exempt;
            TaxBase = _invoice.TaxBase;
            Tax = _invoice.Tax;
            Total = _invoice.Total;
        }

        private async Task GoToNextStep()
        {

            //if it's the last step it performs submit and returns
            if (CurrentFormStep > _formSteps.Count)
            {
                await Submit(); //Possibles exception trhown by Submit() are handled in base class

                return; //A method to go back will be called in OnSavedComplete method (part of Submit() template "steps")
            };

            var navParams = new NavigationParameters
            {
                { ParamKeys.Invoice, _invoice }
            };

            /*Notice the navigation here happens on the FormRegion
            not the Content Region used in most of the project*/
            _regionManager.RequestNavigate(
                RegionNames.FormRegion,
                _formSteps[CurrentFormStep++],
                navParams
                );
        }

        private void ReturnToPreviousStep()
        {
            //If there's no more entry to return to, it will get out from the whole form 
            if (CurrentFormStep - 1 <= 0)
            {
                _navigationJournal?.GoBack();
                return;
            }

            //Else it will navigate to a previous step
            var navParams = new NavigationParameters
            {
                { ParamKeys.Invoice, _invoice }
            };

            /*Notice the navigation here happens on the Form region
             not the Content Region used in most of the project*/
            _regionManager.RequestNavigate(
                RegionNames.FormRegion,
                _formSteps[--CurrentFormStep - 1],
                navParams
                );
        }

        private void SetFormSteps()
        {
            _formSteps.Add(ViewNames.InvoiceMetadataSubform);
        }

        //BaseFormViewModel methods overriding
        protected override async Task<string?> AditionalValidation()
        {
            Invoice? invoice;
            invoice = await _unitOfWork.InvoicesRepository.GetByInvoiceNumber(_invoice.InvoiceNumber);
            if (invoice != null) return "Sorry an invoice with that number already exists";

            return null;
        }

        protected override Invoice ComposeObjectToSave()
        {
            /*_invoice here will be either an invoice from db to update 
            or a new invoice object instanciated in the constructor*/

            return _invoice;
        }

        protected override void OnSavingComplete()
        {
            _navigationJournal?.GoBack();
        }

        //INavigationAware methods implementation
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            _unitOfWork.Dispose();

            /*Removes Form region so a new instance of this region can be safely
            registered in the future.*/
            _regionManager.Regions.Remove(RegionNames.FormRegion);
        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            _navigationJournal = navigationContext.NavigationService.Journal;

            //Gets the action to perform on submit from params
            SubmitAction = navigationContext.Parameters.GetValue<string>(ParamKeys.SumbitAction);

            /*It will use the invoice obect created on the constructor but if
            a valid Id is received when submit action is Update it gets the invoice from the db.*/
            if(SubmitAction == SubmitActions.Update)
            {
                IsLoading = true;

                try
                {
                    var invoiceId = navigationContext.Parameters.GetValue<int>(ParamKeys.InvoiceId);
                    if (invoiceId <= 0)
                    {
                        throw new Exception("Id coming from params is invalid");
                    }

                    var invoiceToUpdate = await Task.Run(async () => await _unitOfWork.InvoicesRepository.GetWithRelatedData(invoiceId));

                    if (invoiceToUpdate == null)
                    {
                        throw new Exception("Couldn't find invoice with provided Id");
                    }

                    _invoice = invoiceToUpdate;
                    Exempt = _invoice.Exempt;
                    Tax = _invoice.Tax;
                    TaxBase = _invoice.TaxBase;
                    Total = _invoice.Total;
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

            GoToNextStep(); /*We know the very next step from here won't be the one calling
                        async Submit() so we can use fire and forget here*/
        }
    }
}
