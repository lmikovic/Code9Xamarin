using System.Collections.Generic;
using System.Threading.Tasks;

namespace Code9Xamarin.Core.Services
{
    public interface IRequestService
    {
        Task<TResult> GetAsync<TResult>(string uri, string token = "");
        Task<TResult> PostAsync<TRequest, TResult>(string uri, TRequest data, string token = "");
        Task<TResult> PutAsync<TRequest, TResult>(string uri, TRequest data, string token = "");
        Task<TResult> DeleteAsync<TRequest, TResult>(string uri, TRequest data, string token = "");
    }
}