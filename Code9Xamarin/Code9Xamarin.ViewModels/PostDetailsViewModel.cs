using Code9Insta.API.Core.DTO;
using Code9Xamarin.Core.Models;
using Code9Xamarin.Core.Services.Interfaces;
using Code9Xamarin.Core.Settings;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.ObjectModel;
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
        public Command AddTagCommand { get; }
        public Command<string> DeleteTagCommand { get; }

        public PostDetailsViewModel(INavigationService navigationService, IPostService postService)
            : this(navigationService, postService, new RuntimeContext())
        { }

        public PostDetailsViewModel(INavigationService navigationService, IPostService postService, IRuntimeContext runtimeContext)
            : base(navigationService, runtimeContext)
        {
            _postService = postService;

            CameraCommand = new Command(async () => await Camera(), () => !IsBusy);
            GalleryCommand = new Command(async () => await Gallery(), () => !IsBusy);
            SaveCommand = new Command(async () => await Save(), () => !IsBusy && PostImage != null);

            AddTagCommand = new Command(() => AddTag(), () => !IsBusy && !string.IsNullOrEmpty(TagText));
            DeleteTagCommand = new Command<string>((item) => DeleteTag(item), (item) => !IsBusy);
        }

        public override void Initialize(object navigationData)
        {
            try
            {
                IsBusy = true;

                var post = navigationData as Post;

                PostImage = post.ImageData;
                Description = post.Description;
                Tags = new ObservableCollection<string>(post.Tags);

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

        private string _tagText;
        public string TagText
        {
            get { return _tagText; }
            set
            {
                _tagText = value;
                OnPropertyChanged();
                AddTagCommand.ChangeCanExecute();
            }
        }

        private ObservableCollection<string> _tags;
        public ObservableCollection<string> Tags
        {
            get { return _tags; }
            set
            {
                _tags = value;
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

        private async Task Camera()
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

        private async Task Gallery()
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

        private async Task Save()
        {
            try
            {
                IsBusy = true;
                string[] tags = null;

                if (Tags != null)
                {
                    tags = new string[Tags.Count];
                    Tags.CopyTo(tags, 0);
                }

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
                        ImageData = byteImage,
                        Tags = tags
                    };

                    await _postService.CreatePost(newPost, _runtimeContext.Token);
                    await _navigationService.NavigateAsync<PostsViewModel>();
                }

                //Edit
                if (_postId != default(Guid))
                {
                    EditPostDto newPost = new EditPostDto
                    {
                        Description = Description,
                        Tags = tags
                    };

                    await _postService.EditPost(newPost, _postId, _runtimeContext.Token);
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

        private void DeleteTag(string item)
        {
            Tags.Remove(item);
        }

        private void AddTag()
        {
            if (Tags == null)
            {
                Tags = new ObservableCollection<string>();
            }
            if (Tags.IndexOf(TagText) == -1)
            {
                Tags.Add(TagText);
            }
            TagText = "";
        }
    }
}
