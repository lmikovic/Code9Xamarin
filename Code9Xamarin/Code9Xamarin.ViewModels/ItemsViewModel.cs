using Code9Xamarin.Core.Models;
using Code9Xamarin.Core.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using System.Linq;

namespace Code9Xamarin.ViewModels
{
    public class ItemsViewModel : INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;
        private readonly IImageService _imageService;

        public Command<int> LikeCommand { get; }

        public ObservableCollection<ImageItem> ImageList { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ItemsViewModel(INavigationService navigationService, IImageService imageService)
        {
            _navigationService = navigationService;
            _imageService = imageService;

            LikeCommand = new Command<int>((id) => LikeClick(id));

            ImageList = new ObservableCollection<ImageItem>(imageService.GetImageList());
        }

        private void LikeClick(int id)
        {
            ImageList.FirstOrDefault(item => item.Id == id).CommentsNumber++;
        }
    }
}
