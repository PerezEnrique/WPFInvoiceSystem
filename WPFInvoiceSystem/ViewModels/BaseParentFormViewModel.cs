using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Utils.Constants;

namespace WPFInvoiceSystem.ViewModels
{
    public abstract class BaseParentFormViewModel<T> : BindableBase
    {
        protected readonly IList<string> _formSteps;
        protected readonly IRegionManager _regionManager;

        private int _currentFormStep;
        public int CurrentFormStep
        {
            get { return _currentFormStep; }
            private set { SetProperty(ref _currentFormStep, value); }
        }

        public BaseParentFormViewModel(IRegionManager regionManager)
        {
            _formSteps = new List<string>();
            _regionManager = regionManager;

            SetFormSteps();
        }

        protected abstract void ExitForm();
        protected async Task GoToNextStep(T formProduct)
        {

            //if it's the last step it performs submit and returns
            if (CurrentFormStep >= _formSteps.Count)
            {
                await OnLastStepReached();
                return;
            };

            //Sets the invoice param required for every step and then requests navigation
            var navParams = new NavigationParameters
            {
                { ParamKeys.FormProduct, formProduct }
            };


            /*Notice the navigation here happens on a differente region 
              not on the main region used in most of the project*/
            _regionManager.RequestNavigate(
                RegionNames.FormRegion,
                _formSteps[CurrentFormStep++],
                navParams
                );
        }

        protected void ReturnToPreviousStep(T formProduct)
        {
            //If there's no more entry to return to, it will exit from the whole form 
            if (CurrentFormStep - 1 <= 0)
            {
                ExitForm();
                return;
            }

            //Else it will navigate to a previous step
            var navParams = new NavigationParameters
            {
                { ParamKeys.FormProduct, formProduct }
            };

            /*Notice the navigation here happens on the Form region
             not the Content Region used in most of the project*/
            _regionManager.RequestNavigate(
                RegionNames.FormRegion,
                _formSteps[--CurrentFormStep - 1],
                navParams
                );
        }

        protected abstract void SetFormSteps();
        protected abstract Task OnLastStepReached();
    }
}
