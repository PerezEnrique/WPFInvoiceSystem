using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WPFInvoiceSystem.Domain;
using WPFInvoiceSystem.Utils.Constants;

namespace WPFInvoiceSystem.ViewModels
{
    public class BaseSearchViewModel<T> : BindableBase  
    {
        protected readonly IUnitOfWork _unitOfWork;
        public ObservableCollection<string> Errors { get; }
        public ObservableCollection<T> Items { get; }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            protected set { SetProperty(ref _isLoading, value); }
        }

        private string _query;
        public string Query
        {
            get { return _query; }
            set { SetProperty(ref _query, value); }
        }

        private T? _selectedItem;
        public T? SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }

        public BaseSearchViewModel(IUnitOfWork unitOfWork)
        {
            _query = string.Empty;
            _unitOfWork = unitOfWork;
            Errors = new ObservableCollection<string>();
            Items = new ObservableCollection<T>();
        }

        protected async Task Search(Expression<Func<T, bool>> predicate)
        {
            if (Query != null && IsLoading != true)
            {
                IsLoading = true;
                Items.Clear();
                Errors.Clear();

                try
                {
                    var searchResult = await _unitOfWork.GetRepository<T>().Find(predicate);

                    if (searchResult.Any())
                    {
                        Items.AddRange(searchResult);
                    }
                    else
                    {
                        Errors.Add("Sorry, we couldn't find any results");
                    }
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
}
