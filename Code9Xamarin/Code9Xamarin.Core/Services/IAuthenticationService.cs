using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Code9Xamarin.Core.Services
{
    public interface IAuthenticationService
    {
        Task<bool> Login(string userName, string password);
        Task<bool> Logout();
        Task<bool> RenewSession(Guid userId, string refreshToken);
    }
}
