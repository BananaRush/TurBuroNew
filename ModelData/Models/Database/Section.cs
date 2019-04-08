using ModelData.Model.Database;
using Newtonsoft.Json;
using StorageAPI.Models.Database;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelData.Models.Database
{
    public class Section
    {
        [Key]
        public int Id { get; set; }
        public int? ParnetId { get; set; }
        public virtual Section Parent { get; set; }
        public bool Visibly { get; set; }
        public virtual List<Section> Children { get; set; }

        [JsonIgnore]
        public virtual List<Information> Disciples { get; set; }
        public string Header { get; set; }
        public string Сategory { get; set; }
        // Язык информации
        public string Language { get; set; }

        public int? ButtonId { get; set; }
        // Секция 
        [ForeignKey("ButtonId")]
        public NewsModel Button { get; set; }

        public Section()
        {
            Children = new List<Section>();
            Disciples = new List<Information>();
        }
    }
}
