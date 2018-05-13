using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Code9Xamarin.Core.Models
{
    public class Post : BindableObject
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public int Comments { get; set; }
        public ImageSource ImageData { get; set; }
        public string[] Tags { get; set; }
        public string TagsText { get; set; }
        public bool HasTags { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public List<Comment> CommentList { get; set; }

        private int _likes;
        public int Likes
        {
            get { return _likes; }
            set
            {
                _likes = value;
                OnPropertyChanged();
            }
        }

        private bool _isLikedByUser;
        public bool IsLikedByUser
        {
            get { return _isLikedByUser; }
            set
            {
                _isLikedByUser = value;
                OnPropertyChanged();
            }
        }
    }
}
