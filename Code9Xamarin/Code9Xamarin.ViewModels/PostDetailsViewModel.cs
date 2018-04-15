using Code9Insta.API.Core.DTO;
using Code9Xamarin.Core;
using Code9Xamarin.Core.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Code9Xamarin.ViewModels
{
    public class PostDetailsViewModel : ViewModelBase
    {
        private readonly IPostService _postService;
        public Command CameraCommand { get; }
        public Command GalleryCommand { get; }
        public Command SaveCommand { get; }

        public PostDetailsViewModel(INavigationService navigationService, IPostService postService)
            : base(navigationService)
        {
            _postService = postService;

            CameraCommand = new Command(async () => await CameraClick(), () => !IsBusy);
            GalleryCommand = new Command(async () => await GalleryClick(), () => !IsBusy);
            SaveCommand = new Command(async () => await SaveClick(), () => !IsBusy && PhotoStream != null);
        }

        private Stream _photoStream;
        public Stream PhotoStream
        {
            get { return _photoStream; }
            set
            {
                _photoStream = value;
                OnPropertyChanged();
                SaveCommand.ChangeCanExecute();
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
                CameraCommand.ChangeCanExecute();
                GalleryCommand.ChangeCanExecute();
                SaveCommand.ChangeCanExecute();
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        private ImageSource _postImage;
        public ImageSource PostImage
        {
            get { return _postImage; }
            set
            {
                _postImage = value;
                OnPropertyChanged();
            }
        }

        async Task CameraClick()
        {
            IsBusy = true;

            PostImage = null;

            await CrossMedia.Current.Initialize();

            if (CrossMedia.Current.IsCameraAvailable || CrossMedia.Current.IsTakePhotoSupported)
            {
                MediaFile photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    Name = $"post-{DateTime.Now.ToString("ddMMyyyyTHHmmss")}.jpg",
                    MaxWidthHeight = 1000,
                    PhotoSize = PhotoSize.MaxWidthHeight,
                    SaveToAlbum = true
                });

                if (photo != null)
                {
                    PhotoStream = photo.GetStream();
                    PostImage = ImageSource.FromStream(() =>
                    {
                        var stream = photo.GetStream();
                        photo.Dispose();
                        return stream;
                    });
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Camera unavailable.", "OK");
            }

            IsBusy = false;
        }

        async Task GalleryClick()
        {
            IsBusy = true;

            PostImage = null;

            await CrossMedia.Current.Initialize();

            if (CrossMedia.Current.IsPickPhotoSupported)
            {
                MediaFile photo = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    MaxWidthHeight = 1000,
                    PhotoSize = PhotoSize.MaxWidthHeight
                });

                if (photo != null)
                {
                    PhotoStream = photo.GetStream();
                    PostImage = ImageSource.FromStream(() =>
                    {
                        var stream = photo.GetStream();
                        photo.Dispose();
                        return stream;
                    });
                }
            }

            IsBusy = false;
        }

        private async Task SaveClick()
        {
            if (PhotoStream != null)
            {
                byte[] byteImage;
                using (MemoryStream ms = new MemoryStream())
                {
                    PhotoStream.CopyTo(ms);
                    byteImage = ms.ToArray();
                }

                CreatePostDto newPost = new CreatePostDto
                {
                    Description = Description,
                    ImageData = byteImage
                };

                await _postService.CreatePost(newPost, AppSettings.Token);
                await _navigationService.NavigateAsync<PostsViewModel>();
            }
        }
    }
}
