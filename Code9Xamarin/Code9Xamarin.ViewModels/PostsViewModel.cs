using Code9Xamarin.Core;
using Code9Xamarin.Core.Mappers;
using Code9Xamarin.Core.Models;
using Code9Xamarin.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        public Command<Guid> DeleteCommand { get; }
        public Command<Guid> EditCommand { get; }

        public PostsViewModel(INavigationService navigationService, IPostService postService)
            : base(navigationService)
        {
            _postService = postService;

            LikeCommand = new Command<Guid>(async (id) => await LikeClick(id), (id) => !IsBusy);
            CreatePostCommand = new Command(async () => await CreatePost(), () => !IsBusy);
            CommentCommand = new Command<Guid>(async (id) => await CommentClick(id), (id) => !IsBusy);
            DeleteCommand = new Command<Guid>(async (id) => await DeleteClick(id), (id) => !IsBusy);
            EditCommand = new Command<Guid>(async (id) => await EditClick(id), (id) => !IsBusy);
        }

        private ObservableCollection<Post> _postList;
        public ObservableCollection<Post> PostList
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
                DeleteCommand.ChangeCanExecute();
            }
        }

        public override async Task InitializeAsync(object navigationData)
        {
            try
            {
                IsBusy = true;

                var allPosts = await _postService.GetAllPosts("", AppSettings.Token);

                PostList = new ObservableCollection<Post>(PostMapper.ToDomainEntities(allPosts));
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

        private async Task EditClick(Guid id)
        {
            var selectedPost = PostList.Single(x => x.Id == id);
            await _navigationService.NavigateAsync<PostDetailsViewModel>(selectedPost);
        }

        private async Task DeleteClick(Guid id)
        {
            await _postService.DeletePost(id, AppSettings.Token);
            var deletedPost = PostList.Single(x => x.Id == id);
            PostList.Remove(deletedPost);
        }
    }
}
