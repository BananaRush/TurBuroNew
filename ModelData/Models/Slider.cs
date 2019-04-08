using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StorageAPI.Models
{
    public class Slider
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Caption { get; set; }
        public int Timeout { get; set; }
        public int Number { get; set; }
        public SliderContentType ContentType { get; set; }

        public static Slider CreateFromDb(Database.Slider dbSlider)
        {
            var slider = new Slider
            {
                Caption = dbSlider.Caption,
                Content = dbSlider.Content,
                ContentType = dbSlider.ContentType,
                Number = dbSlider.Number,
                Timeout = dbSlider.Timeout


            };

            return slider;
        }
    }
}