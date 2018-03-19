using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Code9Xamarin.Core.Services
{
    public interface INavigationService
    {
        void Register<TView, TViewModel>();
        Task NavigateAsync<TViewModel>(bool animated = true);
        Task NavigateAsync<TViewModel>(object parameter, bool animated = true);
    }
}
