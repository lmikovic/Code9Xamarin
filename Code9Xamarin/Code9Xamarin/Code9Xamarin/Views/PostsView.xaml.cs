using Code9Xamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Code9Xamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PostsView : ContentPage
	{
        PostsViewModel postsViewModel;

        public PostsView()
		{
			InitializeComponent ();
            postsViewModel = new PostsViewModel(AppBootstrapper.NavigationService, AppBootstrapper.AuthenticationService, AppBootstrapper.PostService);
            BindingContext = postsViewModel;

            PostsListView.ItemSelected += (sender, e) =>
            {
                ((ListView)sender).SelectedItem = null;
            };
        }

        protected override async void OnAppearing()
        {
            await postsViewModel.InitializeAsync(null);
        }
    }
}