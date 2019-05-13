using Code9Insta.API.Core.DTO;
using Code9Xamarin.Core.Services.Interfaces;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Code9Xamarin.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
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

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private bool _isPublic;
        public bool IsPublic
        {
            get => _isPublic;
            set => SetProperty(ref _isPublic, value);
        }

        public Command RegisterCommand { get; }

        private readonly IProfileService _profileService;

        public RegisterViewModel(INavigationService navigationService, IProfileService profileService)
            : base(navigationService)
        {
            _profileService = profileService;
            RegisterCommand = new Command(
                 execute: async () => await RegisterNewUser(),
                 canExecute: () => !IsBusy && !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(Email));

            this.PropertyChanged += RegisterViewModel_PropertyChanged;
        }

        // Everytime a property in RegisterViewModel calls SetProeprty with a new value check if the RegisterCommand meets the criteria to be executed
        private void RegisterViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RegisterCommand.ChangeCanExecute();
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
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}