using Code9Insta.API.Core.DTO;
using Code9Xamarin.Core.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Code9Xamarin.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        public Command RegisterCommand { get; }

        private readonly IProfileService _profileService;

        public RegisterViewModel(INavigationService navigationService, IProfileService profileService)
            : base(navigationService)
        {
            _profileService = profileService;
            RegisterCommand = new Command(async () => await RegisterNewUser(),
                () => !IsBusy && !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(Email));
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged();
                RegisterCommand.ChangeCanExecute();
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
                RegisterCommand.ChangeCanExecute();
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
                RegisterCommand.ChangeCanExecute();
            }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged();
                RegisterCommand.ChangeCanExecute();
            }
        }

        private bool _isPublic;
        public bool IsPublic
        {
            get { return _isPublic; }
            set
            {
                _isPublic = value;
                OnPropertyChanged();
            }
        }

        private async Task RegisterNewUser()
        {
            try
            {
                IsBusy = true;

                CreateProfileDto newProfile = new CreateProfileDto
                {
                    IsPublic = IsPublic,
                    Handle = UserName,
                    User = new AccountDto
                    {
                        UserName = UserName,
                        Password = Password,
                        Email = Email
                    }
                };

                await _profileService.CreateProfile(newProfile);
                await _navigationService.NavigateAsync<LoginViewModel>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Register] Error: {ex}");
                //await DialogService.ShowAlertAsync(Resources.ExceptionMessage, Resources.ExceptionTitle, Resources.DialogOk);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}