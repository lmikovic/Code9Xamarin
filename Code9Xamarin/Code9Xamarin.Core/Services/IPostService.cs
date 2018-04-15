using Code9Insta.API.Core.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Code9Xamarin.Core.Services
{
    public interface IPostService
    {
        Task<IEnumerable<PostDto>> GetAllPosts(string searchString, string token);
        Task<PostDto> GetPost(Guid id, string token);
        Task<bool> CreatePost(CreatePostDto post, string token);
        Task<bool> EditPost(EditPostDto post, Guid id, string token);
        Task<bool> LikePost(Guid id, string token);
        Task<bool> DeletePost(Guid id, string token);
    }
}
