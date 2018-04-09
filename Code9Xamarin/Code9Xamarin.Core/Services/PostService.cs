using Code9Insta.API.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Code9Xamarin.Core.Services
{
    public class PostService : IPostService
    {
        private readonly IRequestService _requestService;

        public PostService(IRequestService requestService)
        {
            _requestService = requestService;
        }

        public async Task<IEnumerable<PostDto>> GetAllPosts(string searchString, string token)
        {
            UriBuilder builder = new UriBuilder(AppSettings.BaseEndpoint);
            builder.Path = $"api/posts/all";
            builder.Query = $"searchString={searchString}";

            string uri = builder.ToString();

            return await _requestService.GetAsync<IEnumerable<PostDto>>(uri, token);
        }

        public async Task<PostDto> GetPost(Guid id, string token)
        {
            UriBuilder builder = new UriBuilder(AppSettings.BaseEndpoint);
            builder.Path = $"api/posts/{id}";

            string uri = builder.ToString();

            return await _requestService.GetAsync<PostDto>(uri, token);
        }

        public async Task<bool> CreatePost(CreatePostDto post, string token)
        {
            UriBuilder builder = new UriBuilder(AppSettings.BaseEndpoint);
            builder.Path = "api/posts";

            string uri = builder.ToString();

            return await _requestService.PostAsync<CreatePostDto, bool>(uri, post, token);
        }

        public async Task<bool> EditPost(EditPostDto post, Guid id, string token)
        {
            UriBuilder builder = new UriBuilder(AppSettings.BaseEndpoint);
            builder.Path = $"api/posts/{id}";

            string uri = builder.ToString();

            return await _requestService.PutAsync<EditPostDto, bool>(uri, post, token);
        }

        public async Task<bool> LikePost(Guid id, string token)
        {
            UriBuilder builder = new UriBuilder(AppSettings.BaseEndpoint);
            builder.Path = $"api/posts/{id}/reactToPost";

            string uri = builder.ToString();

            return await _requestService.PutAsync<EditPostDto, bool>(uri, null, token);
        }

        public async Task<bool> DeletePost(Guid id, string token)
        {
            UriBuilder builder = new UriBuilder(AppSettings.BaseEndpoint);
            builder.Path = $"api/posts/{id}";

            string uri = builder.ToString();

            return await _requestService.DeleteAsync<EditPostDto, bool>(uri, null, token);
        }
    }
}
