using System;
using System.Threading.Tasks;

namespace Code9Xamarin.Core.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<bool> Login(string userName, string password);
        Task<bool> Logout();
        Task<bool> RenewSession(Guid userId, string refreshToken);
        Task<bool> IsTokenExpired(string token);
    }
}
