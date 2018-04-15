using Code9Xamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Code9Xamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CommentsView : ContentPage
	{
		public CommentsView ()
		{
			InitializeComponent ();
            BindingContext = new CommentsViewModel(AppBootstrapper.NavigationService, AppBootstrapper.CommentService);
        }
	}
}