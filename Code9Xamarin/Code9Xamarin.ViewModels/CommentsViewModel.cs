using Code9Xamarin.Core;
using Code9Xamarin.Core.Mappers;
using Code9Xamarin.Core.Models;
using Code9Xamarin.Core.Services.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Code9Xamarin.ViewModels
{
    public class CommentsViewModel : ViewModelBase
    {
        public Command SaveCommand { get; }
        public Command<Guid> DeleteCommand { get; }
        private Guid _postId;

        private readonly ICommentService _commentService;

        public CommentsViewModel(INavigationService navigationService, ICommentService commentService)
            : base(navigationService)
        {
            _commentService = commentService;

            SaveCommand = new Command(async () => await SaveClick(), () => !IsBusy && !string.IsNullOrEmpty(Text));
            DeleteCommand = new Command<Guid>(async (id) => await DeleteClick(id), (id) => !IsBusy);
        }

        public async override Task InitializeAsync(object navigationData)
        {
            try
            {
                IsBusy = true;

                _postId = (Guid)navigationData;
                var comments = await _commentService.GetPostComments(_postId, AppSettings.Token);
                Comments = new ObservableCollection<Comment>(CommentMapper.ToDomainEntities(comments));
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

        private string _text;
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged();
                SaveCommand.ChangeCanExecute();
            }
        }

        private ObservableCollection<Comment> _comments;
        public ObservableCollection<Comment> Comments
        {
            get { return _comments; }
            set
            {
                _comments = value;
                OnPropertyChanged();
            }
        }

        private async Task SaveClick()
        {
            try
            {
                IsBusy = true;

                await _commentService.CreateComment(_postId, Text, AppSettings.Token);
                Text = "";
                var comments = await _commentService.GetPostComments(_postId, AppSettings.Token);
                Comments = new ObservableCollection<Comment>(CommentMapper.ToDomainEntities(comments));
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

        private async Task DeleteClick(Guid id)
        {
            try
            {
                var answer = await Application.Current.MainPage.DisplayAlert("Delete", "Are you sure you want to delete comment", "Yes", "No");

                if (answer)
                {
                    IsBusy = true;

                    await _commentService.DeleteComment(id, AppSettings.Token);
                    var comments = await _commentService.GetPostComments(_postId, AppSettings.Token);
                    Comments = new ObservableCollection<Comment>(CommentMapper.ToDomainEntities(comments));
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
