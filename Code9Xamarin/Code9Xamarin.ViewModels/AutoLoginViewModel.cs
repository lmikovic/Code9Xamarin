using Code9Xamarin.Core;
using Code9Xamarin.Core.Services.Interfaces;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Code9Xamarin.ViewModels
{
    public class AutoLoginViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AutoLoginViewModel(INavigationService navigationService, IAuthenticationService authenticationService)
            : base(navigationService)
        {
            _authenticationService = authenticationService;
        }

        public override async Task InitializeAsync(object navigationData)
        {
            try
            {
                if (AppSettings.Token != null && AppSettings.UserId != default(Guid) && AppSettings.RefreshToken != null)
                {
                    if (await _authenticationService.IsTokenExpired(AppSettings.Token))
                    {
                        await _authenticationService.RenewSession(AppSettings.UserId, AppSettings.RefreshToken);
                    }

                    _navigationService.SetRootPage(typeof(PostsViewModel));
                }
                else
                {
                    _navigationService.SetRootPage(typeof(LoginViewModel));
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                _navigationService.SetRootPage(typeof(LoginViewModel));
            }
        }
    }
}
