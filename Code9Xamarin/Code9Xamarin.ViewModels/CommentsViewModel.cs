using Code9Xamarin.Core;
using Code9Xamarin.Core.Models;
using Code9Xamarin.Core.Services;
using System;
using System.Collections.Generic;
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
            Comments = new List<Comment>();
        }

        public override void Initialize(object navigationData)
        {
            var post = navigationData as Post;
            Comments = post.CommentList;
            _postId = post.Id;
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

        private List<Comment> _comments;
        public List<Comment> Comments
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
            await _commentService.CreateComment(_postId, Text, AppSettings.Token);
            Text = "";
        }

        private async Task DeleteClick(Guid id)
        {
            await _commentService.DeleteComment(id, AppSettings.Token);
        }
    }
}
