using Code9Insta.API.Core.DTO;
using Code9Xamarin.Core.Mappers.Interfaces;
using Code9Xamarin.Core.Models;
using System.Collections.Generic;

namespace Code9Xamarin.Core.Mappers
{
    public class CommentMapper : IMapper<GetCommentDto, Comment>
    {
        public List<Comment> ToDomainEntities(IEnumerable<GetCommentDto> comments)
        {
            List<Comment> result = new List<Comment>();
            foreach (var commentDto in comments)
            {
                Comment comment = ToDomainEntity(commentDto);
                result.Add(comment);
            }
            return result;
        }

        public Comment ToDomainEntity(GetCommentDto commentDto)
        {
            return new Comment
            {
                Id = commentDto.Id,
                CreatedOn = commentDto.CreatedOn,
                Text = commentDto.Text,
                CreatedBy = commentDto.Handle
            };
        }
    }
}
