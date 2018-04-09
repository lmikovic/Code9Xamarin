using Code9Xamarin.Core.Services;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Code9Xamarin.ViewModels
{
    public abstract class ViewModelBase : BindableObject
    {
        protected readonly INavigationService _navigationService;
        private bool _isBusy;

        public ViewModelBase(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }

            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }
    }
}
