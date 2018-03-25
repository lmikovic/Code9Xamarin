using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Code9Xamarin.Core.Models
{
    public class ImageItem : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime DateTime { get; set; }
        public string Description { get; set; }
        public int CommentsNumber { get; set; }
        public string ImageSource { get; set; }

        private int _likesNumber;
        public int LikesNumber
        {
            get
            {
                return _likesNumber;
            }
            set
            {
                if (value != _likesNumber)
                {
                    _likesNumber = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
