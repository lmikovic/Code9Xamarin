using Code9Xamarin.Core.Mappers;
using Code9Xamarin.Core.Models;
using Code9Xamarin.Core.Services.Interfaces;
using Code9Xamarin.Core.Settings;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Code9Xamarin.ViewModels
{
    public class CommentsViewModel : ViewModelBase
    {
        public Command SaveCommand { get; }
        public Command<Guid> DeleteCommand { get; }
        private Guid _postId;
        private CommentMapper _commentMapper;

        private readonly ICommentService _commentService;

        public CommentsViewModel(INavigationService navigationService, ICommentService commentService)
            : this(navigationService, commentService, new RuntimeContext())
        { }

        public CommentsViewModel(INavigationService navigationService, ICommentService commentService, IRuntimeContext runtimeContext)
            : base(navigationService, runtimeContext)
        {
            _commentService = commentService;

            _commentMapper = new CommentMapper();
            SaveCommand = new Command(async () => await Save(), () => !IsBusy && !string.IsNullOrEmpty(Text));
            DeleteCommand = new Command<Guid>(async (id) => await Delete(id), (id) => !IsBusy);
        }

        public async override Task InitializeAsync(object navigationData)
        {
            try
            {
                IsBusy = true;

                _postId = (Guid)navigationData;
                var comments = await _commentService.GetPostComments(_postId, _runtimeContext.Token);
                Comments = new ObservableCollection<Comment>(_commentMapper.ToDomainEntities(comments));
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

        private async Task Save()
        {
            try
            {
                IsBusy = true;

                await _commentService.CreateComment(_postId, Text, _runtimeContext.Token);
                Text = "";
                var comments = await _commentService.GetPostComments(_postId, _runtimeContext.Token);
                Comments = new ObservableCollection<Comment>(_commentMapper.ToDomainEntities(comments));
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
                var answer = await Application.Current.MainPage.DisplayAlert("Delete", "Are you sure you want to delete comment", "Yes", "No");

                if (answer)
                {
                    IsBusy = true;

                    await _commentService.DeleteComment(id, _runtimeContext.Token);
                    var comments = await _commentService.GetPostComments(_postId, _runtimeContext.Token);
                    Comments = new ObservableCollection<Comment>(_commentMapper.ToDomainEntities(comments));
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
