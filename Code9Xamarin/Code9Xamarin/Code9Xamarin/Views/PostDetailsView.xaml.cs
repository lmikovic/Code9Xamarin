using Code9Xamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Code9Xamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PostDetailsView : ContentPage
	{
		public PostDetailsView ()
		{
			InitializeComponent ();
            BindingContext = new PostDetailsViewModel(AppBootstrapper.NavigationService, AppBootstrapper.PostService);
        }
	}
}