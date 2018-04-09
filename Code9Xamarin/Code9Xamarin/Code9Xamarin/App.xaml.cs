using Code9Xamarin.Core.Services;
using Code9Xamarin.ViewModels;
using Code9Xamarin.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Code9Xamarin
{
	public partial class App : Application
	{
        public App()
        {
            InitializeComponent();

            NavigationService.Register<LoginView, LoginViewModel>();
            NavigationService.Register<ItemsView, ItemsViewModel>();

            MainPage = new Views.LoginView();
        }

        //todo use some dependency injection container (autofac, unity, ninject)?
        public static IRequestService RequestService => new RequestService();
        public static IAuthenticationService AuthenticationService => new AuthenticationService(RequestService);
        public static INavigationService NavigationService => new NavigationService();
        public static IPostService PostService => new PostService(RequestService);

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
