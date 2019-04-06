using Code9Xamarin.Core.Services.Interfaces;
using Code9Xamarin.Core.Settings;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Code9Xamarin.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public Command LoginCommand { get; }
        public Command RegisterCommand { get; }

        private string _userName;
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private readonly IAuthenticationService _authenticationService;
        private readonly IProfileService _profileService;

        public LoginViewModel(INavigationService navigationService, IAuthenticationService authenticationService, IProfileService profileService)
            : this(navigationService, authenticationService, profileService, new RuntimeContext())
        { }

        public LoginViewModel(INavigationService navigationService, IAuthenticationService authenticationService, IProfileService profileService, IRuntimeContext runtimeContext)
            : base(navigationService, runtimeContext)
        {
            _authenticationService = authenticationService;
            _profileService = profileService;

            LoginCommand = new Command(
                execute: async () => await Login(),
                canExecute: () => !IsBusy && !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password));

            RegisterCommand = new Command(async () => await RegisterNewUser());
            PropertyChanged += LoginViewModel_PropertyChanged;
        }

        private void LoginViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            LoginCommand.ChangeCanExecute();
        }

        private async Task Login()
        {
            try
            {
                IsBusy = true;

                await _authenticationService.Login(UserName, Password);
                await _profileService.GetProfile(_runtimeContext.Token);
                _navigationService.SetRootPage(typeof(PostsViewModel));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task RegisterNewUser()
        {
            try
            {
                IsBusy = true;
                await _navigationService.NavigateAsync<RegisterViewModel>();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
