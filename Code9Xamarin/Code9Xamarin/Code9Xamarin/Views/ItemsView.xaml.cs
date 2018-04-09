using Code9Xamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Code9Xamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ItemsView : ContentPage
	{
		public ItemsView ()
		{
			InitializeComponent ();
            BindingContext = new ItemsViewModel(App.NavigationService, App.PostService).InitializeAsync(null);

            ItemsListView.ItemSelected += (sender, e) =>
            {
                ((ListView)sender).SelectedItem = null;
            };
        }
    }
}