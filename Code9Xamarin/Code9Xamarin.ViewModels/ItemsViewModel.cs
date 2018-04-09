using Code9Insta.API.Core.DTO;
using Code9Xamarin.Core;
using Code9Xamarin.Core.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Code9Xamarin.ViewModels
{
    public class ItemsViewModel : ViewModelBase
    {
        private readonly IPostService _postService;

        public Command<Guid> LikeCommand { get; }
        public ObservableCollection<PostDto> PostList { get; set; }

        public ItemsViewModel(INavigationService navigationService, IPostService postService)
            : base(navigationService)
        {
            _postService = postService;
            LikeCommand = new Command<Guid>(async (id) => await LikeClick(id));
        }

        public override async Task InitializeAsync(object navigationData)
        {
            try
            {
                IsBusy = true;

                var allPosts = await _postService.GetAllPosts("", AppSettings.Token);

                PostList = new ObservableCollection<PostDto>(allPosts);
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
            return await _postService.LikePost(id, AppSettings.Token);
        }
    }
}
