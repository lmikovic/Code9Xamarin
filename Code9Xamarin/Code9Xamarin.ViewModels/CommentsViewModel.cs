using Code9Xamarin.Core;
using Code9Xamarin.Core.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Code9Xamarin.ViewModels
{
    public class CommentsViewModel : ViewModelBase
    {
        public Command<Guid> SaveCommand { get; }
        public Command<Guid> DeleteCommand { get; }

        private readonly ICommentService _commentService;

        public CommentsViewModel(INavigationService navigationService, ICommentService commentService)
            : base(navigationService)
        {
            _commentService = commentService;

            SaveCommand = new Command<Guid>(async (id) => await SaveClick(id), (id) => !IsBusy);
            DeleteCommand = new Command<Guid>(async (id) => await DeleteClick(id), (id) => !IsBusy);
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged();
                SaveCommand.ChangeCanExecute();
            }
        }

        private async Task SaveClick(Guid id)
        {
            await _commentService.CreateComment(id, "aa", AppSettings.Token);
            await _navigationService.NavigateAsync<PostsViewModel>();
        }

        private async Task DeleteClick(Guid id)
        {
            await _commentService.DeleteComment(id, AppSettings.Token);
            await _navigationService.NavigateAsync<PostsViewModel>();
        }
    }
}
