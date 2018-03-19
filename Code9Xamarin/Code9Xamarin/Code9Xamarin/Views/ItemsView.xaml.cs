using Code9Xamarin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            BindingContext = new ItemsViewModel(App.NavigationService);
        }
	}
}