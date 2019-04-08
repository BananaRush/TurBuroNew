using StorageAPI.Models.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelData.Model.Database
{
    public class PassageImage
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<ImageList> imageLists { get; set; }
        public int? ButtonId { get; set; }
        // Секция 
        [ForeignKey("ButtonId")]
        public NewsModel Button { get; set; }

        public PassageImage()
        {
            imageLists = new List<ImageList>();
        }
    }

    public class ImageList
    {
        public int Id { get; set; }
        public string ImgUrl { get; set; }
        public PassageImage passageImage { get; set; }
    }
}
