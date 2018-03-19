﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Code9Xamarin.Core.Services
{
    public class NavigationService : INavigationService
    {
        private static Dictionary<Type, Type> _pagesByType = new Dictionary<Type, Type>();

        public void Register<TView, TViewModel>()
        {
            _pagesByType.Add(typeof(TView), typeof(TViewModel));
        }

        public Task NavigateAsync<TViewModel>(bool animated = true)
        {
            return NavigateToAsync(typeof(TViewModel), null, animated);
        }

        public Task NavigateAsync<TViewModel>(object parameter, bool animated = true)
        {
            return NavigateToAsync(typeof(TViewModel), parameter, animated);
        }

        private async Task NavigateToAsync(Type viewModelType, object parameter, bool animated)
        {
            Page page = CreatePage(viewModelType);

            var navigationPage = Application.Current.MainPage as NavigationPage;
            if (navigationPage != null)
            {
                await navigationPage.PushAsync(page, animated);
            }
            else
            {
                Application.Current.MainPage = new NavigationPage(page);
            }
        }

        public void SetRootPage(Type viewModelType)
        {
            Page page = CreatePage(viewModelType);
            Application.Current.MainPage = new NavigationPage(page);
        }

        private Page CreatePage(Type viewModelType)
        {
            Type pageType = _pagesByType.FirstOrDefault(x => x.Value == viewModelType).Key;
            if (pageType == null)
            {
                throw new Exception($"Cannot locate page type for {viewModelType.Name}");
            }

            Page page = Activator.CreateInstance(pageType) as Page;
            return page;
        }
    }
}