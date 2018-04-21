using Code9Xamarin.ViewModels;
using Code9Xamarin.Views;
using Xamarin.Forms;

namespace Code9Xamarin
{
    public partial class App : Application
	{
        public App()
        {
            InitializeComponent();

            var boot = new AppBootstrapper();
            boot.Initialize();

            MainPage = new AutoLoginView();
        }

        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
