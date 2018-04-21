using Code9Insta.API.Core.DTO;
using Code9Xamarin.Core.Models;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;

namespace Code9Xamarin.Core.Mappers
{
    public class PostMapper
    {
        public static List<Post> ToDomainEntities(IEnumerable<PostDto> allPosts)
        {
            List<Post> result = new List<Post>();
            foreach (var postDto in allPosts)
            {
                Post post = ToDomainEntity(postDto);
                result.Add(post);
            }
            return result;
        }

        public static Post ToDomainEntity(PostDto postDto)
        {
            var commentList = new List<Comment>();

            foreach (var commentDto in postDto.Comments)
            {
                var comment = new Comment
                {
                    Id = commentDto.Id,
                    Text = commentDto.Text
                };
                commentList.Add(comment);
            }

            return new Post
            {
                Comments = postDto.Comments.Count,
                CreatedBy = postDto.CreatedBy,
                CreatedOn = postDto.CreatedOn,
                Description = postDto.Description,
                Id = postDto.Id,
                ImageData = ImageSource.FromStream(() => new MemoryStream(postDto.ImageData)),
                IsLikedByUser = postDto.IsLikedByUser,
                Likes = postDto.Likes,
                Tags = postDto.Tags,
                CommentList = commentList
            };
        }
    }


}
