using Code9Insta.API.Core.DTO;
using System.Threading.Tasks;

namespace Code9Xamarin.Core.Services.Interfaces
{
    public interface IProfileService
    {
        Task<GetProfileDto> GetProfile(string token);
        Task<bool> CreateProfile(CreateProfileDto profile);
    }
}
