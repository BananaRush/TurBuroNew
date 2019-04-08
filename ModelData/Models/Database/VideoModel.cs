using StorageAPI.Models.Database;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelData.Models.Database
{
    public class VideoModel
    {
        [Key]
        public int Id { get; set; }
        public string Header { get; set; }
        public string Path { get; set; }
        public string Cat { get; set; }
        public int? ButtonId { get; set; }
        // Секция 
        [ForeignKey("ButtonId")]
        public NewsModel Button { get; set; }
    }
}
