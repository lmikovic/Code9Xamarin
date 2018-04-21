using System;

namespace Code9Xamarin.Core.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Text { get; set; }
    }
}
