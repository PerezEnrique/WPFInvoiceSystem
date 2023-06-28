using FluentValidation;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Domain;
using WPFInvoiceSystem.Utils.Constants;

namespace WPFInvoiceSystem.ViewModels
{
    public abstract class BaseFormViewModel<T> : BindableBase
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected IValidator<T> _validator;
        public ObservableCollection<string> Errors { get; }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            protected set { SetProperty(ref _isLoading, value); }
        }

        private string? _submitAction;
        public string? SubmitAction
        {
            get { return _submitAction; }
            protected set { SetProperty(ref _submitAction, value); }
        }

        
        public BaseFormViewModel(IUnitOfWork unitOfWork, IValidator<T> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            Errors = new ObservableCollection<string>();
        }


        //Template pattern is implemented here.Submit es the template method

        protected abstract Task<string?> AditionalValidation();
        protected abstract T ComposeObjectToSave();
        protected abstract void OnSavingComplete();

        protected async Task Submit()
        {
            if (!IsLoading)
            {
                IsLoading = true;

                try
                {
                    var objectToSave = ComposeObjectToSave();

                    Errors.Clear();

                    var validationResult = _validator.Validate(objectToSave);

                    if (!validationResult.IsValid)
                    {
                        foreach (var failure in validationResult.Errors)
                        {
                            Errors.Add(failure.ErrorMessage);
                        }

                        return;
                    }

                    //If It is creating a new objetct It adds it to the repository
                    if (SubmitAction == SubmitActions.Create)
                    {
                        var aditionalValidationResult = await AditionalValidation();
                        if (aditionalValidationResult != null)
                        {
                            Errors.Add(aditionalValidationResult);
                            return;
                        }

                        _unitOfWork.GetRepository<T>().Add(objectToSave);
                    }

                    //Whether It's creating or updating an object It saves changes
                    Errors.Clear();
                    await _unitOfWork.CompleteAsync();

                    //Finally let derived class do more things if the saving process was successfull
                    OnSavingComplete();
                }
                catch (Exception)
                {
                    Errors.Add(UnexpectedErrorMessage.Message);
                    return;
                }
                finally
                {
                    IsLoading = false;
                }
            }
        }
    }
}
