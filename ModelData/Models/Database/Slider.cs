using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.IO;

namespace StorageAPI.Models.Database
{
    [Table("Slider")]
    public partial class Slider
    {
        public long Id { get; set; }

        [StringLength(1000)]
        public string Content { get; set; }

        [StringLength(1000)]
        public string Caption { get; set; }

        public int Timeout { get; set; }

        public int Number { get; set; }

        [DataType("int")]
        public SliderContentType ContentType { get; set; }

        public bool Expance(Slider slider)
        {
            if(Path.GetFileName(this.Content) != Path.GetFileName(slider.Content))
                return false;

            if (this.Number != slider.Number)
                return false;

            if (this.Timeout != slider.Timeout)
                return false;

            return true;
        }
    }
}
