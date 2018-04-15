using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Code9Xamarin.Core.Services
{
    public class CommentService : ICommentService
    {
        private readonly IRequestService _requestService;

        public CommentService(IRequestService requestService)
        {
            _requestService = requestService;
        }

        public async Task<bool> CreateComment(Guid postId, string text, string token)
        {
            UriBuilder builder = new UriBuilder(AppSettings.BaseEndpoint)
            {
                Path = "api/comments",
                Query = $"postId={postId}&text={text}"
            };

            string uri = builder.ToString();

            string message = await _requestService.PostAsync<string, string>(uri, null, token);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteComment(Guid id, string token)
        {
            UriBuilder builder = new UriBuilder(AppSettings.BaseEndpoint)
            {
                Path = "api/comments",
                Query = $"commentId={id}"
            };

            string uri = builder.ToString();

            await _requestService.DeleteAsync<string, string>(uri, null, token);

            return await Task.FromResult(true);
        }
    }
}
