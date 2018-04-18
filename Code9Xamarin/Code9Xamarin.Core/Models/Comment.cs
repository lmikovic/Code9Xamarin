using System;

namespace Code9Xamarin.Core.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Text { get; set; }
    }
}
