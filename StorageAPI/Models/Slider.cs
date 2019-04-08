using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StorageAPI.Models
{
    public class Slider
    {
        public long? Id { get; set; }
        public string Content { get; set; }
        public string Caption { get; set; }
        public SliderContentType ContentType { get; set; }

        public static Slider CreateFromDb(Database.Slider dbSlider)
        {
            var slider = new Slider
            {
                Id = dbSlider.Id,
                Caption = dbSlider.Caption,
                Content = dbSlider.Content,
                ContentType = dbSlider.ContentType
            };

            return slider;
        }
    }
}