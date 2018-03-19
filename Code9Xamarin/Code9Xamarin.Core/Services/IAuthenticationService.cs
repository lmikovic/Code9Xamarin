using System;
using System.Collections.Generic;
using System.Text;

namespace Code9Xamarin.Core.Services
{
    public interface IAuthenticationService
    {
        bool Login(string userName, string password);
        bool Logout();
    }
}
