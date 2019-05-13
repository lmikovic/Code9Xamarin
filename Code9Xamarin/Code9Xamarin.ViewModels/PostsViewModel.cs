using Code9Xamarin.Core.Mappers;
using Code9Xamarin.Core.Models;
using Code9Xamarin.Core.Services.Interfaces;
using Code9Xamarin.Core.Settings;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Code9Xamarin.ViewModels
{
    public class PostsViewModel : ViewModelBase
    {
        private readonly IPostService _postService;
        private readonly IAuthenticationService _authenticationService;

        public Command<Guid> LikeCommand { get; }
        public Command CreatePostCommand { get; }
        public Command LogOutCommand { get; }
        public Command<Guid> CommentCommand { get; }
        public Command<Guid> DeleteCommand { get; }
        public Command<Guid> EditCommand { get; }
        public Command SearchCommand { get; }

        private ObservableCollection<Post> _postList;
        public ObservableCollection<Post> PostList
        {
            get => _postList;
            set => SetProperty(ref _postList, value);
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        public PostsViewModel(INavigationService navigationService, IAuthenticationService authenticationService, IPostService postService)
            : this(navigationService, authenticationService, postService, new RuntimeContext())
        { }

        public PostsViewModel(INavigationService navigationService, IAuthenticationService authenticationService, IPostService postService, IRuntimeContext runtimeContext)
            : base(navigationService, runtimeContext)
        {
            _postService = postService;
            _authenticationService = authenticationService;

            LikeCommand = new Command<Guid>(async (id) => await Like(id), (id) => !IsBusy);
            CreatePostCommand = new Command(async () => await CreatePost(), () => !IsBusy);
            LogOutCommand = new Command(async () => await LogOut(), () => !IsBusy);
            CommentCommand = new Command<Guid>(async (id) => await Comment(id), (id) => !IsBusy);
            DeleteCommand = new Command<Guid>(async (id) => await Delete(id), (id) => !IsBusy);
            EditCommand = new Command<Guid>(async (id) => await Edit(id), (id) => !IsBusy);
            SearchCommand = new Command(async () => await Search(), () => !IsBusy);

            PropertyChanged += PostsViewModel_PropertyChanged;
        }

        // Everytime a property in RegisterViewModel calls SetProeprty with a new value check if the RegisterCommand meets the criteria to be executed
        private void PostsViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            LikeCommand.ChangeCanExecute();
            CreatePostCommand.ChangeCanExecute();
            LogOutCommand.ChangeCanExecute();
            CommentCommand.ChangeCanExecute();
            DeleteCommand.ChangeCanExecute();
            EditCommand.ChangeCanExecute();
            SearchCommand.ChangeCanExecute();
        }

        public override async Task InitializeAsync(object navigationData)
        {
            await SearchPosts("");
        }

        private async Task Search()
        {
            await SearchPosts(SearchText);
        }

        private async Task SearchPosts(string searchText)
        {
            try
            {
                IsBusy = true;

                var searchPosts = await _postService.GetAllPosts(searchText, _runtimeContext.Token);

                var postMapper = new PostMapper();

                PostList = new ObservableCollection<Post>(postMapper.ToDomainEntities(searchPosts));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task<bool> Like(Guid id)
        {
            try
            {
                IsBusy = true;

                await _postService.LikePost(id, _runtimeContext.Token);
                var postDto = await _postService.GetPost(id, _runtimeContext.Token);
                PostList.First(x => x.Id == id).Likes = postDto.Likes;
                PostList.First(x => x.Id == id).IsLikedByUser = postDto.IsLikedByUser;

                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return await Task.FromResult(false);
            }
            finally
            {
                IsBusy = false;
            }

        }

        private async Task CreatePost()
        {
            try
            {
                IsBusy = true;
                await _navigationService.NavigateAsync<PostDetailsViewModel>();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task LogOut()
        {
            try
            {
                IsBusy = true;
                await _authenticationService.Logout();
                _navigationService.SetRootPage(typeof(LoginViewModel));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task Comment(Guid id)
        {
            try
            {
                IsBusy = true;
                await _navigationService.NavigateAsync<CommentsViewModel>(id);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task Edit(Guid id)
        {
            try
            {
                IsBusy = true;
                var selectedPost = PostList.Single(x => x.Id == id);
                await _navigationService.NavigateAsync<PostDetailsViewModel>(selectedPost);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task Delete(Guid id)
        {
            try
            {
                var answer = await Application.Current.MainPage.DisplayAlert("Delete", "Are you sure you want to delete post", "Yes", "No");

                if (answer)
                {
                    IsBusy = true;
                    await _postService.DeletePost(id, AppSettings.Token);
                    var deletedPost = PostList.Single(x => x.Id == id);
                    PostList.Remove(deletedPost);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
