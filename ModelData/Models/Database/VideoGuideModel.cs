using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelData.Models.Database
{
    public class VideoGuideModel
    {
        [Key]
        public int Id { get; set; }
        public string Header { get; set; }
        public string Path { get; set; }
        public string Cat { get; set; }
    }
}
