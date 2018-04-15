using System;
using System.Threading.Tasks;

namespace Code9Xamarin.Core.Services
{
    public interface ICommentService
    {
        Task<bool> CreateComment(Guid postId, string text, string token);
        Task<bool> DeleteComment(Guid id, string token);
    }
}
