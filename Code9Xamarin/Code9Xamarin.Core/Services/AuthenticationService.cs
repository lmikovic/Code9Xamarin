using Code9Xamarin.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Code9Xamarin.Core.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IRequestService _requestService;

        public AuthenticationService(IRequestService requestService)
        {
            _requestService = requestService;
        }

        public async Task<bool> Login(string userName, string password)
        {
            UriBuilder builder = new UriBuilder(AppSettings.BaseEndpoint)
            {
                Path = "api/token/request",
                Query = $"userName={userName}&password={password}"
            };

            string uri = builder.ToString();

            var tokenResponse = await _requestService.GetAsync<TokenModel>(uri);

            AppSettings.Token = tokenResponse.Token;
            AppSettings.RefreshToken = tokenResponse.RefreshToken;

            return await Task.FromResult(true);
        }

        public Task<bool> Logout()
        {
            AppSettings.RemoveToken();
            return Task.FromResult(true);
        }

        public async Task<bool> RenewSession(Guid userId, string refreshToken)
        {
            UriBuilder builder = new UriBuilder(AppSettings.BaseEndpoint)
            {
                Path = $"api/token/refresh",
                Query = $"userId={userId}&refreshToken={refreshToken}"
            };

            string uri = builder.ToString();

            var tokenResponse = await _requestService.GetAsync<TokenModel>(uri);

            AppSettings.Token = tokenResponse.Token;
            AppSettings.RefreshToken = tokenResponse.RefreshToken;

            return await Task.FromResult(true);
        }
    }
}
