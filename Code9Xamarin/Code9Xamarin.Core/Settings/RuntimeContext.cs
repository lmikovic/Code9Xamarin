using System;

namespace Code9Xamarin.Core.Settings
{
    //we can't use AppSettings directly because it is not testable
    public class RuntimeContext : IRuntimeContext
    {
        public string BaseEndpoint
        {
            get => AppSettings.BaseEndpoint;

            set => AppSettings.BaseEndpoint = value;
        }

        public string Token
        {
            get => AppSettings.Token;

            set => AppSettings.Token = value;
        }

        public string RefreshToken
        {
            get => AppSettings.RefreshToken;

            set => AppSettings.RefreshToken = value;
        }

        public Guid UserId
        {
            get => AppSettings.UserId;

            set => AppSettings.UserId = value;
        }

        public void RemoveUserId()
        {
            AppSettings.RemoveUserId();
        }

        public void RemoveToken()
        {
            AppSettings.RemoveToken();
        }
    }
}
