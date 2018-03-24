using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Code9Xamarin.Core.Models
{
    public class ImageItem
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime DateTime { get; set; }
        public string Description { get; set; }
        public int LikesNumber { get; set; }
        public int CommentsNumber { get; set; }
        public string ImageSource { get; set; }
    }
}
