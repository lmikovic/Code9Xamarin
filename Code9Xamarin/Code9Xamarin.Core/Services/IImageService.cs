using Code9Xamarin.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Code9Xamarin.Core.Services
{
    public interface IImageService
    {
        List<ImageItem> GetImageList();
        void AddLike(int itemId);
        ImageItem GetItem(int itemId);
    }
}
