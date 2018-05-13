using Code9Xamarin.Core.Services.Interfaces;
using Code9Xamarin.Core.Settings;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Code9Xamarin.ViewModels
{
    public class AutoLoginViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AutoLoginViewModel(INavigationService navigationService, IAuthenticationService authenticationService)
            : this(navigationService, authenticationService, new RuntimeContext())
        {}

        public AutoLoginViewModel(INavigationService navigationService, IAuthenticationService authenticationService, IRuntimeContext runtimeContext)
            : base(navigationService, runtimeContext)
        {
            _authenticationService = authenticationService;
        }

        public override async Task InitializeAsync(object navigationData)
        {
            try
            {
                if (_runtimeContext.Token != null && _runtimeContext.UserId != default(Guid) && _runtimeContext.RefreshToken != null)
                {
                    if (await _authenticationService.IsTokenExpired(_runtimeContext.Token))
                    {
                        await _authenticationService.RenewSession(_runtimeContext.UserId, _runtimeContext.RefreshToken);
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
