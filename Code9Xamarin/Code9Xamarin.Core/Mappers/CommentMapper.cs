using Code9Insta.API.Core.DTO;
using Code9Xamarin.Core.Models;
using System.Collections.Generic;

namespace Code9Xamarin.Core.Mappers
{
    public class CommentMapper
    {
        public static List<Comment> ToDomainEntities(IEnumerable<GetCommentDto> comments)
        {
            List<Comment> result = new List<Comment>();
            foreach (var commentDto in comments)
            {
                Comment comment = ToDomainEntity(commentDto);
                result.Add(comment);
            }
            return result;
        }

        public static Comment ToDomainEntity(GetCommentDto commentDto)
        {
            return new Comment
            {
                Id = commentDto.Id,
                CreatedOn = commentDto.CreatedOn,
                Text = commentDto.Text,
                CreatedBy = commentDto.UserId.ToString()
            };
        }
    }
}
