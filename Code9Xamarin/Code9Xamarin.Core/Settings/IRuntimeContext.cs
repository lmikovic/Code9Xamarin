using System;

namespace Code9Xamarin.Core.Settings
{
    public interface IRuntimeContext
    {
        string BaseEndpoint { get; set; }
        string RefreshToken { get; set; }
        string Token { get; set; }
        Guid UserId { get; set; }

        void RemoveToken();
        void RemoveUserId();
    }
}