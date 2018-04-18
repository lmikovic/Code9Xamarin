using System;
using System.Threading.Tasks;

namespace Code9Xamarin.Core.Services.Interfaces
{
    public interface INavigationService
    {
        void Register<TView, TViewModel>();
        Task NavigateAsync<TViewModel>(bool animated = true);
        Task NavigateAsync<TViewModel>(object parameter, bool animated = true);
        void SetRootPage(Type viewModelType);
    }
}
