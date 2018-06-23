using Code9Insta.API.Core.DTO;
using Code9Xamarin.Core.Services.Interfaces;
using Code9Xamarin.Core.Settings;
using System;
using System.Threading.Tasks;

namespace Code9Xamarin.Core.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IRequestService _requestService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IRuntimeContext _runtimeContext;

        public ProfileService(IRequestService requestService, IAuthenticationService authenticationService)
            : this(requestService, authenticationService, new RuntimeContext())
        { }

        public ProfileService(IRequestService requestService, IAuthenticationService authenticationService, IRuntimeContext runtimeContext)
        {
            _requestService = requestService;
            _authenticationService = authenticationService;
            _runtimeContext = runtimeContext;
        }

        public async Task<GetProfileDto> GetProfile(string token)
        {
            UriBuilder builder = new UriBuilder(_runtimeContext.BaseEndpoint)
            {
                Path = $"api/profiles"
            };

            if (await _authenticationService.IsTokenExpired(token))
            {
                await _authenticationService.RenewSession(_runtimeContext.UserId, _runtimeContext.RefreshToken);
            }

            var profile = await _requestService.GetAsync<GetProfileDto>(builder.Uri, token);

            _runtimeContext.UserId = profile.UserId;

            return profile;
        }

        public async Task<bool> CreateProfile(CreateProfileDto profile)
        {
            UriBuilder builder = new UriBuilder(_runtimeContext.BaseEndpoint)
            {
                Path = "api/profiles"
            };

            var message = await _requestService.PostAsync<CreateProfileDto, string>(builder.Uri, profile);

            return await Task.FromResult(true);
        }
    }
}
