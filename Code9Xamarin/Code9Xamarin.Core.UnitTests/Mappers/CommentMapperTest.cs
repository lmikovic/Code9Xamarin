using Code9Insta.API.Core.DTO;
using Code9Xamarin.Core.Mappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Code9Xamarin.Core.UnitTests.Mappers
{
    [TestClass]
    public class CommentMapperTest
    {
        CommentMapper _mapper;

        [TestInitialize]
        public void Setup()
        {
            _mapper = new CommentMapper();
        }

        [TestMethod]
        public void CommentMapperToDomainEntityTest()
        {
            GetCommentDto commentDto = new GetCommentDto
            {
                UserId = Guid.NewGuid(),
                CreatedOn = DateTime.Now,
                Id = Guid.NewGuid(),
                Text = "Test",
                Handle = "UserName"
            };

            var comment = _mapper.ToDomainEntity(commentDto);

            Assert.AreEqual(commentDto.Handle, comment.CreatedBy);
            Assert.AreEqual(commentDto.CreatedOn, comment.CreatedOn);
            Assert.AreEqual(commentDto.Id, comment.Id);
            Assert.AreEqual(commentDto.Text, comment.Text);
        }

        [TestMethod]
        public void CommentMapperToDomainEntitiesTest()
        {
            List<GetCommentDto> getCommentDtos = new List<GetCommentDto>
            {
                new GetCommentDto
                {
                    UserId = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    Id = Guid.NewGuid(),
                    Text = "Test1",
                    Handle = "UserName1"
                },
                new GetCommentDto
                {
                    UserId = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    Id = Guid.NewGuid(),
                    Text = "Test2",
                    Handle = "UserName2"
                },
            };

            var comments = _mapper.ToDomainEntities(getCommentDtos);

            Assert.AreEqual(getCommentDtos[0].Handle, comments[0].CreatedBy);
            Assert.AreEqual(getCommentDtos[0].CreatedOn, comments[0].CreatedOn);
            Assert.AreEqual(getCommentDtos[0].Id, comments[0].Id);
            Assert.AreEqual(getCommentDtos[0].Text, comments[0].Text);

            Assert.AreEqual(getCommentDtos[1].Handle, comments[1].CreatedBy);
            Assert.AreEqual(getCommentDtos[1].CreatedOn, comments[1].CreatedOn);
            Assert.AreEqual(getCommentDtos[1].Id, comments[1].Id);
            Assert.AreEqual(getCommentDtos[1].Text, comments[1].Text);
        }
    }

}
