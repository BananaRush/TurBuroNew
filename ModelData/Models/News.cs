using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ModelData.Models.Database;
using StorageAPI.Models.Database;

namespace StorageAPI.Models
{
    public class News
    {
        public long? Id { get; set; }
        public string Text { get; set; }
        public string IconUri { get; set; }
        public string Content { get; set; }
        public int IdContent { get; set; }
        public string AdvertisingImageUri { get; set; }
        public NewsContentType ContentType { get; set; }

        public static News CreateFromDb(NewsModel newsModel)
        {
            var news = new News
            {
                Id = newsModel.Id,
                AdvertisingImageUri = newsModel.AdvertisingImageUri,
                IdContent = newsModel.IdContent,
                ContentType = newsModel.ContentType,
                IconUri = newsModel.IconUri,
                Text = newsModel.Text
            };

            return news;
        }
    }
}