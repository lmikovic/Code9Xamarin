using Code9Xamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Code9Xamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegisterView : ContentPage
	{
		public RegisterView ()
		{
			InitializeComponent ();
            BindingContext = new RegisterViewModel(AppBootstrapper.NavigationService, AppBootstrapper.ProfileService);
        }
	}
}