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
	public partial class ItemCellView : ContentPage
	{
		public ItemCellView ()
		{
			InitializeComponent ();
            BindingContext = new ItemCellViewModel(App.NavigationService);
        }
	}
}