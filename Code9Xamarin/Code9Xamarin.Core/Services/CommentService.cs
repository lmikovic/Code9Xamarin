using Code9Insta.API.Core.DTO;
using Code9Xamarin.Core.Services.Interfaces;
using Code9Xamarin.Core.Settings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Code9Xamarin.Core.Services
{
    public class CommentService : ICommentService
    {
        private readonly IRequestService _requestService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IRuntimeContext _runtimeContext;

        public CommentService(IRequestService requestService, IAuthenticationService authenticationService)
            : this(requestService, authenticationService, new RuntimeContext())
        { }

        public CommentService(IRequestService requestService, IAuthenticationService authenticationService, IRuntimeContext runtimeContext)
        {
            _requestService = requestService;
            _authenticationService = authenticationService;
            _runtimeContext = runtimeContext;
        }

        public async Task<IEnumerable<GetCommentDto>> GetPostComments(Guid postId, string token)
        {
            UriBuilder builder = new UriBuilder(_runtimeContext.BaseEndpoint)
            {
                Path = $"api/comments/GetPostComments",
                Query = $"postId={postId}"
            };

            if (await _authenticationService.IsTokenExpired(token))
            {
                await _authenticationService.RenewSession(_runtimeContext.UserId, _runtimeContext.RefreshToken);
            }

            return await _requestService.GetAsync<IEnumerable<GetCommentDto>>(builder.Uri, token);
        }

        public async Task<bool> CreateComment(Guid postId, string text, string token)
        {
            UriBuilder builder = new UriBuilder(_runtimeContext.BaseEndpoint)
            {
                Path = "api/comments",
                Query = $"postId={postId}&text={text}"
            };

            if (await _authenticationService.IsTokenExpired(token))
            {
                await _authenticationService.RenewSession(_runtimeContext.UserId, _runtimeContext.RefreshToken);
            }

            await _requestService.PostAsync<string, string>(builder.Uri, null, token);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteComment(Guid id, string token)
        {
            UriBuilder builder = new UriBuilder(_runtimeContext.BaseEndpoint)
            {
                Path = "api/comments",
                Query = $"commentId={id}"
            };

            if (await _authenticationService.IsTokenExpired(token))
            {
                await _authenticationService.RenewSession(_runtimeContext.UserId, _runtimeContext.RefreshToken);
            }

            await _requestService.DeleteAsync<string, string>(builder.Uri, null, token);

            return await Task.FromResult(true);
        }
    }
}
