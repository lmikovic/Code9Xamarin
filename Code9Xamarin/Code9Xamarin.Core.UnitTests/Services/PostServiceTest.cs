using Code9Insta.API.Core.DTO;
using Code9Xamarin.Core.Services;
using Code9Xamarin.Core.Services.Interfaces;
using Code9Xamarin.Core.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Code9Xamarin.Core.UnitTests.Services
{
    [TestClass]
    public class PostServiceTest
    {
        PostService _postService;
        Mock<IRequestService> _requestServiceMock;
        Mock<IAuthenticationService> _authenticationServiceMock;
        Mock<IRuntimeContext> _runtimeContextMock;

        [TestInitialize]
        public void Setup()
        {
            _requestServiceMock = new Mock<IRequestService>();
            _authenticationServiceMock = new Mock<IAuthenticationService>();
            _runtimeContextMock = new Mock<IRuntimeContext>();

            //authenticationService mocks
            _authenticationServiceMock
                .Setup(x => x.IsTokenExpired(It.IsAny<string>()))
                .Returns(Task.FromResult(false));

            //requestService mocks
            _requestServiceMock
                .Setup(x => x.PostAsync<CreatePostDto, string>(It.IsAny<Uri>(), It.IsAny<CreatePostDto>(), It.IsAny<string>()))
                .Returns(Task.FromResult(It.IsAny<string>()));

            _requestServiceMock
                .Setup(x => x.PutAsync<EditPostDto, string>(It.IsAny<Uri>(), It.IsAny<EditPostDto>(), It.IsAny<string>()))
                .Returns(Task.FromResult(It.IsAny<string>()));

            _requestServiceMock
                .Setup(x => x.PutAsync<object, string>(It.IsAny<Uri>(), null, It.IsAny<string>()))
                .Returns(Task.FromResult(It.IsAny<string>()));

            _requestServiceMock
                .Setup(x => x.DeleteAsync<object, string>(It.IsAny<Uri>(), null, It.IsAny<string>()))
                .Returns(Task.FromResult(It.IsAny<string>()));

            _requestServiceMock
                .Setup(x => x.GetAsync<PostDto>(It.IsAny<Uri>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new PostDto
                {
                    Comments = null,
                    CreatedBy = "UserName",
                    CreatedOn = DateTime.MinValue,
                    Description = "Description",
                    Id = new Guid("8e864dcc-5e0e-49c2-9f66-b3da6cee735f"),
                    ImageData = null,
                    IsLikedByUser = true,
                    Likes = 2,
                    Tags = null
                }));

            _requestServiceMock
                .Setup(x => x.GetAsync<IEnumerable<PostDto>>(It.IsAny<Uri>(), It.IsAny<string>()))
                .Returns(Task.FromResult<IEnumerable<PostDto>>(new List<PostDto>
                {
                    new PostDto
                    {
                        Comments = null,
                        CreatedBy = "UserName1",
                        CreatedOn = DateTime.MinValue,
                        Description = "Description1",
                        Id = new Guid("8e864dcc-5e0e-49c2-9f66-b3da6cee735f"),
                        ImageData = null,
                        IsLikedByUser = true,
                        Likes = 2,
                        Tags = null
                    },
                    new PostDto
                    {
                        Comments = new Collection<CommentDto>
                        {
                            new CommentDto { Id = Guid.NewGuid(), Text = "CommentText"}
                        },
                        CreatedBy = "UserName2",
                        CreatedOn = DateTime.MinValue,
                        Description = "Description2",
                        Id = new Guid("46c7656f-6a77-4a48-817f-ec45c5cf5bf7"),
                        ImageData = null,
                        IsLikedByUser = false,
                        Likes = 2,
                        Tags = new string[2] {"tag1", "tag2"}
                    }
                }));

            //app settings mock
            _runtimeContextMock.SetupGet(x => x.BaseEndpoint).Returns("http://test.com");
            _runtimeContextMock.SetupGet(x => x.RefreshToken).Returns("RefreshToken");
            _runtimeContextMock.SetupGet(x => x.Token).Returns("Token");
            _runtimeContextMock.SetupGet(x => x.UserId).Returns(new Guid("38aefc0d-1ba3-47d9-bd84-1bd3e35071ec"));

            _postService = new PostService(_requestServiceMock.Object, _authenticationServiceMock.Object, _runtimeContextMock.Object);
        }

        [TestMethod]
        public async Task GetAllPostsServiceTest()
        {
            //Arrange
            string token = "Token";
            Guid postId1 = new Guid("8e864dcc-5e0e-49c2-9f66-b3da6cee735f");
            var postId2 = new Guid("46c7656f-6a77-4a48-817f-ec45c5cf5bf7");
            var searchString = "Search+123";
            var excpectedUri = new Uri($"{_runtimeContextMock.Object.BaseEndpoint}/api/posts/all?searchString={Uri.EscapeDataString(searchString)}");

            //Act
            var result = await _postService.GetAllPosts(searchString, token);
            var allPosts = (List<PostDto>)result;

            //Assert
            Assert.AreEqual(null, allPosts[0].Comments);
            Assert.AreEqual("UserName1", allPosts[0].CreatedBy);
            Assert.AreEqual(DateTime.MinValue, allPosts[0].CreatedOn);
            Assert.AreEqual("Description1", allPosts[0].Description);
            Assert.AreEqual(postId1, allPosts[0].Id);
            Assert.AreEqual(null, allPosts[0].ImageData);
            Assert.AreEqual(true, allPosts[0].IsLikedByUser);
            Assert.AreEqual(2, allPosts[0].Likes);
            Assert.AreEqual(null, allPosts[0].Tags);

            Assert.AreEqual(1, allPosts[1].Comments.Count);
            Assert.AreEqual("UserName2", allPosts[1].CreatedBy);
            Assert.AreEqual(DateTime.MinValue, allPosts[1].CreatedOn);
            Assert.AreEqual("Description2", allPosts[1].Description);
            Assert.AreEqual(postId2, allPosts[1].Id);
            Assert.AreEqual(null, allPosts[1].ImageData);
            Assert.AreEqual(false, allPosts[1].IsLikedByUser);
            Assert.AreEqual(2, allPosts[1].Likes);
            Assert.AreEqual(2, allPosts[1].Tags.Length);

            _requestServiceMock.Verify(x => x.GetAsync<IEnumerable<PostDto>>(excpectedUri, token), Times.Once);
        }

        [TestMethod]
        public async Task GetPostServiceTest()
        {
            //Arrange
            string token = "Token";
            Guid postId = new Guid("8e864dcc-5e0e-49c2-9f66-b3da6cee735f");
            var expectedUri = new Uri($"{_runtimeContextMock.Object.BaseEndpoint}/api/posts/{postId}");

            //Act
            var result = await _postService.GetPost(postId, token);

            //Assert
            Assert.AreEqual(null, result.Comments);
            Assert.AreEqual("UserName", result.CreatedBy);
            Assert.AreEqual(DateTime.MinValue, result.CreatedOn);
            Assert.AreEqual("Description", result.Description);
            Assert.AreEqual(postId, result.Id);
            Assert.AreEqual(null, result.ImageData);
            Assert.AreEqual(true, result.IsLikedByUser);
            Assert.AreEqual(2, result.Likes);
            Assert.AreEqual(null, result.Tags);
            _requestServiceMock.Verify(x => x.GetAsync<PostDto>(expectedUri, token), Times.Once);
        }

        [TestMethod]
        public async Task CreatePostServiceTest()
        {
            //Arrange
            string token = "Token";
            var expectedUri = new Uri($"{_runtimeContextMock.Object.BaseEndpoint}/api/posts");
            CreatePostDto postDto = new CreatePostDto
            {
                Description = "Test description",
                ImageData = null,
                Tags = null
            };

            //Act
            var result = await _postService.CreatePost(postDto, token);

            //Assert
            Assert.IsTrue(result);
            _requestServiceMock.Verify(x => x.PostAsync<CreatePostDto, string>(expectedUri, postDto, token), Times.Once);
        }

        [TestMethod]
        public async Task EditPostServiceTest()
        {
            //Arrange
            string token = "Token";
            Guid postId = new Guid("8e864dcc-5e0e-49c2-9f66-b3da6cee735f");
            var expectedUri = new Uri($"{_runtimeContextMock.Object.BaseEndpoint}/api/posts/{postId}");
            EditPostDto editPostDto = new EditPostDto
            {
                Description = "Test description",
                Tags = null
            };

            //Act
            var result = await _postService.EditPost(editPostDto, postId, token);

            //Assert
            Assert.IsTrue(result);
            _requestServiceMock.Verify(x => x.PutAsync<EditPostDto, string>(expectedUri, editPostDto, token), Times.Once);
        }

        [TestMethod]
        public async Task LikePostServiceTest()
        {
            //Arrange
            string token = "Token";
            Guid postId = new Guid("8e864dcc-5e0e-49c2-9f66-b3da6cee735f");
            var expectedUri = new Uri($"{_runtimeContextMock.Object.BaseEndpoint}/api/posts/{postId}/reactToPost");

            //Act
            var result = await _postService.LikePost(postId, token);

            //Assert
            Assert.IsTrue(result);
            _requestServiceMock.Verify(x => x.PutAsync<object, string>(expectedUri, null, token), Times.Once);
        }

        [TestMethod]
        public async Task DeletePostServiceTest()
        {
            //Arrange
            string token = "Token";
            Guid postId = new Guid("8e864dcc-5e0e-49c2-9f66-b3da6cee735f");
            var expectedUri = new Uri($"{_runtimeContextMock.Object.BaseEndpoint}/api/posts/{postId}");

            //Act
            var result = await _postService.DeletePost(postId, token);

            //Assert
            Assert.IsTrue(result);
            _requestServiceMock.Verify(x => x.DeleteAsync<object, string>(expectedUri, null, token), Times.Once);
        }
    }
}
