
using Code9Insta.API.Core.DTO;
using Code9Xamarin.Core;
using Code9Xamarin.Core.Models;
using Code9Xamarin.Core.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Code9Xamarin.ViewModels
{
    public class PostsViewModel : ViewModelBase
    {
        private readonly IPostService _postService;

        public Command<Guid> LikeCommand { get; }
        public Command CreatePostCommand { get; }
        public Command<Guid> CommentCommand { get; }

        public PostsViewModel(INavigationService navigationService, IPostService postService)
            : base(navigationService)
        {
            _postService = postService;

            LikeCommand = new Command<Guid>(async (id) => await LikeClick(id), (id) => !IsBusy);
            CreatePostCommand = new Command(async () => await CreatePost(), () => !IsBusy);
            CommentCommand = new Command<Guid>(async (id) => await CommentClick(id), (id) => !IsBusy);
        }

        private List<Post> _postList;
        public List<Post> PostList
        {
            get { return _postList; }
            set
            {
                _postList = value;
                OnPropertyChanged();
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged();
                CreatePostCommand.ChangeCanExecute();
                LikeCommand.ChangeCanExecute();
            }
        }

        public override async Task InitializeAsync(object navigationData)
        {
            try
            {
                IsBusy = true;

                var allPosts = await _postService.GetAllPosts("", AppSettings.Token);

                PostList = MapPosts(allPosts);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Home] Error: {ex}");
                //await DialogService.ShowAlertAsync(Resources.ExceptionMessage, Resources.ExceptionTitle, Resources.DialogOk);
            }
            finally
            {
                IsBusy = false;
            }
        }

        //todo create mapper class
        private List<Post> MapPosts(IEnumerable<PostDto> allPosts)
        {
            List<Post> result = new List<Post>();
            foreach(var postDto in allPosts)
            {
                Post post = ConvertToPost(postDto);
                result.Add(post);
            }
            return result;
        }

        private Post ConvertToPost(PostDto postDto)
        {
            var commentList = new List<Comment>();

            foreach(var commentDto in postDto.Comments)
            {
                var comment = new Comment
                {
                    Id = commentDto.Id,
                    Text = commentDto.Text
                };
                commentList.Add(comment);
            }

            return new Post
            {
                Comments = postDto.Comments.Count,
                CreatedBy = postDto.CreatedBy,
                CreatedOn = postDto.CreatedOn,
                Description = postDto.Description,
                Id = postDto.Id,
                ImageData = ImageSource.FromStream(() => new MemoryStream(postDto.ImageData)),
                IsLikedByUser = postDto.IsLikedByUser,
                Likes = postDto.Likes,
                Tags = postDto.Tags,
                CommentList = commentList
            };
        }

        private async Task<bool> LikeClick(Guid id)
        {
            await _postService.LikePost(id, AppSettings.Token);
            var postDto = await _postService.GetPost(id, AppSettings.Token);
            PostList.First(x => x.Id == id).Likes = postDto.Likes;
            PostList.First(x => x.Id == id).IsLikedByUser = postDto.IsLikedByUser;

            return await Task.FromResult(true);
        }

        private async Task CreatePost()
        {
            await _navigationService.NavigateAsync<PostDetailsViewModel>();
        }

        private async Task CommentClick(Guid id)
        {
            var selectedPost = PostList.Single(x => x.Id == id);
            await _navigationService.NavigateAsync<CommentsViewModel>(selectedPost);
        }
    }
}
