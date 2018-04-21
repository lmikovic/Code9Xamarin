using Code9Xamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Code9Xamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AutoLoginView : ContentPage
	{
        AutoLoginViewModel _autoLoginViewModel;

        public AutoLoginView ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
            _autoLoginViewModel = new AutoLoginViewModel(AppBootstrapper.NavigationService, AppBootstrapper.AuthenticationService);
        }

        protected override async void OnAppearing()
        {
            await _autoLoginViewModel.InitializeAsync(null);
        }
    }
}