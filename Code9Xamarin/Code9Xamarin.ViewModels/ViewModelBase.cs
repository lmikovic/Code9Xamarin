using Code9Xamarin.Core.Services.Interfaces;
using Code9Xamarin.Core.Settings;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Code9Xamarin.ViewModels
{
    public abstract class ViewModelBase : BindableObject
    {
        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        protected readonly INavigationService _navigationService;
        protected readonly IRuntimeContext _runtimeContext;

        public ViewModelBase(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public ViewModelBase(INavigationService navigationService, IRuntimeContext runtimeContext)
        {
            _navigationService = navigationService;
            _runtimeContext = runtimeContext;
        }

        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }

        public virtual void Initialize(object navigationData)
        {
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }
            storage = value;
            OnPropertyChanged(propertyName);

            return true;
        }
    }
}
