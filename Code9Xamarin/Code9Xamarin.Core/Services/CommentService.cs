using System;
using System.Threading.Tasks;

namespace Code9Xamarin.Core.Services
{
    public class CommentService : ICommentService
    {
        private readonly IRequestService _requestService;
        private readonly IAuthenticationService _authenticationService;

        public CommentService(IRequestService requestService, IAuthenticationService authenticationService)
        {
            _requestService = requestService;
            _authenticationService = authenticationService;
        }

        public async Task<bool> CreateComment(Guid postId, string text, string token)
        {
            UriBuilder builder = new UriBuilder(AppSettings.BaseEndpoint)
            {
                Path = "api/comments",
                Query = $"postId={postId}&text={text}"
            };

            string uri = builder.ToString();

            if (await _authenticationService.IsTokenExpired(token))
            {
                await _authenticationService.RenewSession(AppSettings.UserId, AppSettings.RefreshToken);
            }

            await _requestService.PostAsync<string, string>(uri, null, token);

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

            if (await _authenticationService.IsTokenExpired(token))
            {
                await _authenticationService.RenewSession(AppSettings.UserId, AppSettings.RefreshToken);
            }

            await _requestService.DeleteAsync<string, string>(uri, null, token);

            return await Task.FromResult(true);
        }
    }
}
