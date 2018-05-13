using Code9Xamarin.Core.Services;
using Code9Xamarin.Core.Services.Interfaces;
using Code9Xamarin.ViewModels;
using Code9Xamarin.Views;

namespace Code9Xamarin
{
    public class AppBootstrapper
    {
        //we can use some dependency injection container (autofac, unity, ninject)
        public static INavigationService NavigationService => new NavigationService();
        public static IRequestService RequestService => new RequestService();
        public static IAuthenticationService AuthenticationService => new AuthenticationService(RequestService);
        public static IPostService PostService => new PostService(RequestService, AuthenticationService);
        public static IProfileService ProfileService => new ProfileService(RequestService, AuthenticationService);
        public static ICommentService CommentService => new CommentService(RequestService, AuthenticationService);

        public void Initialize()
        {
            NavigationService.Register<LoginView, LoginViewModel>();
            NavigationService.Register<PostsView, PostsViewModel>();
            NavigationService.Register<RegisterView, RegisterViewModel>();
            NavigationService.Register<PostDetailsView, PostDetailsViewModel>();
            NavigationService.Register<CommentsView, CommentsViewModel>();
            NavigationService.Register<AutoLoginView, AutoLoginViewModel>();
        }

    }
}
