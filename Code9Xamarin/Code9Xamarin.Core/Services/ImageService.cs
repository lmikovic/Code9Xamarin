using System;
using System.Collections.Generic;
using System.Text;
using Code9Xamarin.Core.Models;

namespace Code9Xamarin.Core.Services
{
    public class ImageService : IImageService
    {
        private static List<ImageItem> dummyImageList = new List<ImageItem>
            {
                new ImageItem()
                {
                    Id = 1,
                    UserName = "UserName1",
                    DateTime = DateTime.Now,
                    Description = "Kathrin Romary Beckinsale (born 26 July 1973) is an English actress. After some minor television roles, she made her film debut in Much Ado About Nothing (1993) while still a student at the University of Oxford. She appeared in British costume dramas such as Prince of Jutland (1994), Cold Comfort Farm (1995), Emma (1996), and The Golden Bowl (2000), in addition to various stage and radio productions. She began to seek film work in the United States in the late 1990s and, after appearing in small-scale dramas The Last Days of Disco (1998) and Brokedown Palace (1999), she had starring roles in the war drama Pearl Harbor and the romantic comedy Serendipity. She followed those with appearances in The Aviator (2004) and Click (2006).",
                    ImageSource = "https://i.ytimg.com/vi/LUeF9xikKS8/maxresdefault.jpg",
                    LikesNumber = 5,
                    CommentsNumber = 10
                },
                new ImageItem()
                {
                    Id = 2,
                    UserName = "UserName1",
                    DateTime = DateTime.Now,
                    Description = "Alicia Amanda Vikander (born 3 October 1988) is a Swedish actress. She has received several nominations and awards for her acting, including an Academy Award, and was listed by Forbes in their 30 Under 30 list in 2016. In 2016, Vikander won the Best Supporting Actress Oscar for her role in The Danish Girl.",
                    ImageSource = "https://i.mdel.net/i/db/2015/9/422264/422264-800w.jpg",
                    LikesNumber = 5,
                    CommentsNumber = 10
                },
                new ImageItem()
                {
                    Id = 3,
                    UserName = "UserName2",
                    DateTime = DateTime.Now,
                    Description = "Natalie Portman (born Neta-Lee Hershlag; June 9, 1981) is an actress and film producer with dual Israeli and American citizenship. Portman is best known for her roles as Padmé Amidala in the Star Wars prequel trilogy and Nina Sayers in Black Swan (2010); she won an Academy Award, Golden Globe Award, and Screen Actors Guild Award, among other accolades, for her performance in the latter.",
                    ImageSource = "http://www.filmlervekitaplar.com/wp-content/gallery/natalie-portman/natalie-portman-10.jpg",
                    LikesNumber = 5,
                    CommentsNumber = 10
                }
            };



        public List<ImageItem> GetImageList()
        {
            return dummyImageList;
        }

        public void AddLike(int itemId)
        {
            dummyImageList.Find(x => x.Id == itemId).LikesNumber++;
        }

        public ImageItem GetItem(int itemId)
        {
            return dummyImageList.Find(x => x.Id == itemId);
        }
    }
}