using Code9Xamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Code9Xamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CommentsView : ContentPage
	{
        CommentsViewModel _commentsViewModel;
        object _parameter;

        public CommentsView ()
		{
			InitializeComponent ();
            _commentsViewModel = new CommentsViewModel(AppBootstrapper.NavigationService, AppBootstrapper.CommentService);
            BindingContext = _commentsViewModel;
        }

        public CommentsView(object parameter) : this()
        {
            _parameter = parameter;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_parameter != null)
            {
                _commentsViewModel.Initialize(_parameter);
            }
        }
    }
}