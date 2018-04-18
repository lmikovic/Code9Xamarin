using Code9Insta.API.Core.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Code9Xamarin.Core.Services.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<GetCommentDto>> GetPostComments(Guid postId, string token);
        Task<bool> CreateComment(Guid postId, string text, string token);
        Task<bool> DeleteComment(Guid id, string token);
    }
}
