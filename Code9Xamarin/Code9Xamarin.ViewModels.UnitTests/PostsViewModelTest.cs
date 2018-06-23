using Code9Insta.API.Core.DTO;
using Code9Xamarin.Core.Models;
using Code9Xamarin.Core.Services.Interfaces;
using Code9Xamarin.Core.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Code9Xamarin.ViewModels.UnitTests
{
    [TestClass]
    public class PostsViewModelTest
    {
        PostsViewModel _postsViewModel;
        Mock<IRuntimeContext> _runtimeContextMock;
        Mock<IPostService> _postServiceMock;
        Mock<IAuthenticationService> _authenticationServiceMock;
        Mock<INavigationService> _navigationServiceMock;

        [TestInitialize]
        public void Setup()
        {
            _authenticationServiceMock = new Mock<IAuthenticationService>();
            _runtimeContextMock = new Mock<IRuntimeContext>();
            _postServiceMock = new Mock<IPostService>();
            _navigationServiceMock = new Mock<INavigationService>();

            //authenticationService mocks
            _authenticationServiceMock
                .Setup(x => x.Logout())
                .Returns(Task.FromResult(true));

            //app settings mock
            _runtimeContextMock.SetupGet(x => x.BaseEndpoint).Returns("http://test.com");
            _runtimeContextMock.SetupGet(x => x.RefreshToken).Returns("RefreshToken");
            _runtimeContextMock.SetupGet(x => x.Token).Returns("Token");
            _runtimeContextMock.SetupGet(x => x.UserId).Returns(new Guid("38aefc0d-1ba3-47d9-bd84-1bd3e35071ec"));

            //postService mocks
            _postServiceMock
                .Setup(x => x.LikePost(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(Task.FromResult(true));

            _postServiceMock
                .Setup(x => x.GetPost(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new PostDto
                {
                    Comments = null,
                    CreatedBy = "UserName1",
                    CreatedOn = DateTime.MinValue,
                    Description = "Description1",
                    Id = new Guid("8e864dcc-5e0e-49c2-9f66-b3da6cee735f"),
                    ImageData = new byte[0],
                    IsLikedByUser = true,
                    Likes = 2,
                    Tags = null
                }));

            _postServiceMock
                .Setup(x => x.GetAllPosts(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult<IEnumerable<PostDto>>(new List<PostDto>
                {
                    new PostDto
                    {
                        Comments = null,
                        CreatedBy = "UserName1",
                        CreatedOn = DateTime.MinValue,
                        Description = "Description1",
                        Id = new Guid("8e864dcc-5e0e-49c2-9f66-b3da6cee735f"),
                        ImageData = new byte[0],
                        IsLikedByUser = true,
                        Likes = 2,
                        Tags = null
                    },
                    new PostDto
                    {
                        Comments = new Collection<GetCommentDto>
                        {
                            new GetCommentDto { Id = Guid.NewGuid(), Text = "CommentText"}
                        },
                        CreatedBy = "UserName2",
                        CreatedOn = DateTime.MinValue,
                        Description = "Description2",
                        Id = new Guid("46c7656f-6a77-4a48-817f-ec45c5cf5bf7"),
                        ImageData = new byte[0],
                        IsLikedByUser = false,
                        Likes = 2,
                        Tags = new string[2] {"tag1", "tag2"}
                    }
                }));

            _postsViewModel = new PostsViewModel(_navigationServiceMock.Object, _authenticationServiceMock.Object, _postServiceMock.Object, _runtimeContextMock.Object);
        }

        [TestMethod]
        public async Task InitializePageTest()
        {
            //Arrange
            var postId1 = new Guid("8e864dcc-5e0e-49c2-9f66-b3da6cee735f");
            var postId2 = new Guid("46c7656f-6a77-4a48-817f-ec45c5cf5bf7");
            var tags = new string[2] { "tag1", "tag2" };
            var tagText = "tag1, tag2";

            //Act
            await _postsViewModel.InitializeAsync(null);

            //Assert
            Assert.AreEqual(2, _postsViewModel.PostList.Count);

            Assert.AreEqual(0, _postsViewModel.PostList[0].Comments);
            Assert.AreEqual(0, _postsViewModel.PostList[0].CommentList.Count);
            Assert.AreEqual("UserName1", _postsViewModel.PostList[0].CreatedBy);
            Assert.AreEqual(DateTime.MinValue, _postsViewModel.PostList[0].CreatedOn);
            Assert.AreEqual("Description1", _postsViewModel.PostList[0].Description);
            Assert.AreEqual(postId1, _postsViewModel.PostList[0].Id);
            Assert.AreEqual(true, _postsViewModel.PostList[0].IsLikedByUser);
            Assert.AreEqual(2, _postsViewModel.PostList[0].Likes);
            Assert.AreEqual(null, _postsViewModel.PostList[0].Tags);
            Assert.AreEqual(false, _postsViewModel.PostList[0].HasTags);
            Assert.AreEqual("", _postsViewModel.PostList[0].TagsText);

            Assert.AreEqual(1, _postsViewModel.PostList[1].Comments);
            Assert.AreEqual(1, _postsViewModel.PostList[1].CommentList.Count);
            Assert.AreEqual("UserName2", _postsViewModel.PostList[1].CreatedBy);
            Assert.AreEqual(DateTime.MinValue, _postsViewModel.PostList[1].CreatedOn);
            Assert.AreEqual("Description2", _postsViewModel.PostList[1].Description);
            Assert.AreEqual(postId2, _postsViewModel.PostList[1].Id);
            Assert.AreEqual(false, _postsViewModel.PostList[1].IsLikedByUser);
            Assert.AreEqual(2, _postsViewModel.PostList[1].Likes);
            Assert.AreEqual(tags[0], _postsViewModel.PostList[1].Tags[0]);
            Assert.AreEqual(true, _postsViewModel.PostList[1].HasTags);
            Assert.AreEqual(tagText, _postsViewModel.PostList[1].TagsText);

            _postServiceMock.Verify(x => x.GetAllPosts("", _runtimeContextMock.Object.Token), Times.Once);
        }

        [TestMethod]
        public void CreatePostTest()
        {
            //Act
            _postsViewModel.CreatePostCommand.Execute(null);

            //Assert
            _navigationServiceMock.Verify(x => x.NavigateAsync<PostDetailsViewModel>(It.IsAny<bool>()), Times.Once);
        }

        [TestMethod]
        public void LikePostTest()
        {
            //Arrange
            var postId = new Guid("8e864dcc-5e0e-49c2-9f66-b3da6cee735f");
            var postList = new List<Post>
            {
                new Post
                {
                    Comments = 0,
                    CommentList = new List<Comment>(),
                    CreatedBy = "UserName1",
                    CreatedOn = DateTime.MinValue,
                    Description = "Description1",
                    Id = postId,
                    IsLikedByUser = false,
                    Likes = 1,
                    HasTags = false,
                    Tags = null,
                    TagsText = ""
                }
            };

            _postsViewModel.PostList = new ObservableCollection<Post>(postList);

            //Act
            _postsViewModel.LikeCommand.Execute(postId);

            //Assert
            var likedPost = _postsViewModel.PostList.Single(x => x.Id == postId);
            Assert.AreEqual(likedPost.Likes, 2);
            Assert.AreEqual(likedPost.IsLikedByUser, true);
        }

        [TestMethod]
        public void LogoutTest()
        {
            //Act
            _postsViewModel.LogOutCommand.Execute(null);

            //Assert
            _navigationServiceMock.Verify(x => x.SetRootPage(typeof(LoginViewModel)), Times.Once);
        }

        [TestMethod]
        public void CommentTest()
        {
            //Arrange
            var postId = new Guid("8e864dcc-5e0e-49c2-9f66-b3da6cee735f");

            //Act
            _postsViewModel.CommentCommand.Execute(postId);

            //Assert
            _navigationServiceMock.Verify(x => x.NavigateAsync<CommentsViewModel>(postId, It.IsAny<bool>()), Times.Once);
        }

        [TestMethod]
        public void EditPostTest()
        {
            //Arrange
            var postId = new Guid("8e864dcc-5e0e-49c2-9f66-b3da6cee735f");
            var postList = new List<Post>
            {
                new Post
                {
                    Comments = 0,
                    CommentList = new List<Comment>(),
                    CreatedBy = "UserName1",
                    CreatedOn = DateTime.MinValue,
                    Description = "Description1",
                    Id = postId,
                    IsLikedByUser = false,
                    Likes = 1,
                    HasTags = false,
                    Tags = null,
                    TagsText = ""
                }
            };

            _postsViewModel.PostList = new ObservableCollection<Post>(postList);

            //Act
            _postsViewModel.EditCommand.Execute(postId);

            //Assert
            _navigationServiceMock.Verify(x => x.NavigateAsync<PostDetailsViewModel>(It.IsAny<Post>(), It.IsAny<bool>()), Times.Once);
        }
    }
}
