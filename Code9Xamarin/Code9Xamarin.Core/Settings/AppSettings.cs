using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;

namespace Code9Xamarin.Core.Settings
{
    public static class AppSettings
    {
        // Endpoints
        private const string DefaultBaseEndpoint = "http://code9api.azurewebsites.net/";

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

        public static Guid UserId
        {
            get => Settings.GetValueOrDefault(nameof(UserId), default(Guid));

            set => Settings.AddOrUpdateValue(nameof(UserId), value);
        }

        public static void RemoveUserId()
        {
            Settings.Remove(nameof(UserId));
        }

        public static void RemoveToken()
        {
            Settings.Remove(nameof(Token));
            Settings.Remove(nameof(RefreshToken));
        }
    }
}
