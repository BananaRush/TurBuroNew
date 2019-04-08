using StorageAPI.Models.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelData.Models.Database
{
    public class TerminalDataModel
    {
        [Key]
        public int Id { get; set; }
        public Guid IdTerminal { get; set; }
        public int? UIElementModelId { get; set; }
        [ForeignKey("UIElementModelId")]
        public UIElementModel uIElementModel { get; set; }
        public int? NewsModelId { get; set; }
        [ForeignKey("NewsModelId")]
        public NewsModel newsModel { get; set; }
    }
}
