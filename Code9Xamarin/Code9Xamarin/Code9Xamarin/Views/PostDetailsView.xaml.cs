using Code9Xamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Code9Xamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PostDetailsView : ContentPage
	{
        object _parameter;
        PostDetailsViewModel _postDetailsViewModel;

        public PostDetailsView ()
		{
			InitializeComponent ();
            _postDetailsViewModel = new PostDetailsViewModel(AppBootstrapper.NavigationService, AppBootstrapper.PostService);
            BindingContext = _postDetailsViewModel;
        }

        public PostDetailsView(object parameter) : this()
        {
            _parameter = parameter;

            if (_parameter != null)
            {
                GalleryIcon.IsEnabled = false;
                CameraIcon.IsEnabled = false;
                _postDetailsViewModel.Initialize(_parameter);
            }
        }
    }
}