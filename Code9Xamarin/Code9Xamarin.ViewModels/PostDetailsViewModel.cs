using Code9Insta.API.Core.DTO;
using Code9Xamarin.Core;
using Code9Xamarin.Core.Models;
using Code9Xamarin.Core.Services.Interfaces;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Diagnostics;
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
            SaveCommand = new Command(async () => await SaveClick(), () => !IsBusy && PostImage != null);
        }

        public override void Initialize(object navigationData)
        {
            try
            {
                IsBusy = true;

                var post = navigationData as Post;

                PostImage = post.ImageData;
                Description = post.Description;
                _postId = post.Id;
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private Guid _postId;
        private Stream _photoStream;

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
                SaveCommand.ChangeCanExecute();
            }
        }

        private async Task CameraClick()
        {
            try
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
                        _photoStream = photo.GetStream();
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

        private async Task GalleryClick()
        {
            try
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
                        _photoStream = photo.GetStream();
                        PostImage = ImageSource.FromStream(() =>
                        {
                            var stream = photo.GetStream();
                            photo.Dispose();
                            return stream;
                        });
                    }
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

        private async Task SaveClick()
        {
            try
            {
                IsBusy = true;

                if (_photoStream != null)
                {
                    byte[] byteImage;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        _photoStream.CopyTo(ms);
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

                //Edit
                if (_postId != default(Guid))
                {
                    EditPostDto newPost = new EditPostDto
                    {
                        Description = Description
                    };

                    await _postService.EditPost(newPost, _postId, AppSettings.Token);
                    await _navigationService.NavigateAsync<PostsViewModel>();
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
