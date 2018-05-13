using Code9Xamarin.Core.Models;
using Code9Xamarin.Core.Services.Interfaces;
using Code9Xamarin.Core.Settings;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace Code9Xamarin.Core.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IRequestService _requestService;
        private readonly IRuntimeContext _runtimeContext;

        public AuthenticationService(IRequestService requestService)
            : this(requestService, new RuntimeContext())
        { }

        public AuthenticationService(IRequestService requestService, IRuntimeContext runtimeContext)
        {
            _requestService = requestService;
            _runtimeContext = runtimeContext;
        }

        public async Task<bool> Login(string userName, string password)
        {
            UriBuilder builder = new UriBuilder(_runtimeContext.BaseEndpoint)
            {
                Path = "api/token/request",
                Query = $"userName={Uri.EscapeDataString(userName)}&password={Uri.EscapeDataString(password)}"
            };

            var tokenResponse = await _requestService.GetAsync<TokenModel>(builder.Uri);

            _runtimeContext.Token = tokenResponse.Token;
            _runtimeContext.RefreshToken = tokenResponse.RefreshToken;

            return await Task.FromResult(true);
        }

        public Task<bool> Logout()
        {
            _runtimeContext.RemoveToken();
            _runtimeContext.RemoveUserId();
            return Task.FromResult(true);
        }

        public Task<bool> IsTokenExpired(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = (JwtSecurityToken)handler.ReadToken(token);
            return Task.FromResult(DateTime.UtcNow > jwtToken.ValidTo);
        }

        public async Task<bool> RenewSession(Guid userId, string refreshToken)
        {
            UriBuilder builder = new UriBuilder(_runtimeContext.BaseEndpoint)
            {
                Path = $"api/token/refresh",
                Query = $"userId={userId}&refreshToken={Uri.EscapeDataString(refreshToken)}" //refreshToken can have a plus sign, and that's why we have to escape it
            };

            var tokenResponse = await _requestService.GetAsync<TokenModel>(builder.Uri);

            _runtimeContext.Token = tokenResponse.Token;
            _runtimeContext.RefreshToken = tokenResponse.RefreshToken;

            return await Task.FromResult(true);
        }
    }
}
