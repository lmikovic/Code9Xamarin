using Code9Xamarin.Core.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Code9Xamarin.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public Command LoginCommand { get; }

        private readonly IAuthenticationService _authenticationService;

        public LoginViewModel(INavigationService navigationService, IAuthenticationService authenticationService)
            : base(navigationService)
        {
            _authenticationService = authenticationService;
            LoginCommand = new Command(async () => await Login(),
                () => !IsBusy && !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password));
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged();
                LoginCommand.ChangeCanExecute();
            }
        }


        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged();
                LoginCommand.ChangeCanExecute();
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
                LoginCommand.ChangeCanExecute();
            }
        }

        async Task Login()
        {
            try
            {
                IsBusy = true;

                await _authenticationService.Login(UserName, Password);
                await _navigationService.NavigateAsync<ItemsViewModel>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Login] Error: {ex}");
                //await DialogService.ShowAlertAsync(Resources.ExceptionMessage, Resources.ExceptionTitle, Resources.DialogOk);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
