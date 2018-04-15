using Code9Insta.API.Core.DTO;
using System;
using System.Threading.Tasks;

namespace Code9Xamarin.Core.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IRequestService _requestService;

        public ProfileService(IRequestService requestService)
        {
            _requestService = requestService;
        }

        public async Task<GetProfileDto> GetProfile(string token)
        {
            UriBuilder builder = new UriBuilder(AppSettings.BaseEndpoint)
            {
                Path = $"api/profile"
            };

            string uri = builder.ToString();

            return await _requestService.GetAsync<GetProfileDto>(uri, token);
        }

        public async Task<bool> CreateProfile(CreateProfileDto profile)
        {
            UriBuilder builder = new UriBuilder(AppSettings.BaseEndpoint)
            {
                Path = "api/profile"
            };

            string uri = builder.ToString();

            var message = await _requestService.PostAsync<CreateProfileDto, string>(uri, profile);

            return await Task.FromResult(true);
        }
    }
}
