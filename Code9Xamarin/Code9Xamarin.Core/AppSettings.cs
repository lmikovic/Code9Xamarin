using Code9Xamarin.Core.Models;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Code9Xamarin.Core
{
    public static class AppSettings
    {
        // Endpoints
        private const string DefaultBaseEndpoint = "http://192.168.0.11:5000";// "http://code9instaapi20180404072500.azurewebsites.net";

        private static ISettings Settings => CrossSettings.Current;

        // API Endpoints
        public static string BaseEndpoint
        {
            get => Settings.GetValueOrDefault(nameof(BaseEndpoint), DefaultBaseEndpoint);

            set => Settings.AddOrUpdateValue(nameof(BaseEndpoint), value);
        }

        public static string Token
        {
            get => Settings.GetValueOrDefault(nameof(Token), default(string));

            set => Settings.AddOrUpdateValue(nameof(Token), value);
        }

        public static string RefreshToken
        {
            get => Settings.GetValueOrDefault(nameof(RefreshToken), default(string));

            set => Settings.AddOrUpdateValue(nameof(RefreshToken), value);
        }

        public static void RemoveToken()
        {
            Settings.Remove(nameof(Token));
            Settings.Remove(nameof(RefreshToken));
        }
    }
}
