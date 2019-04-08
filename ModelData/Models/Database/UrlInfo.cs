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
    public class UrlInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Cat { get; set; }
        public string Lang { get; set; }
        public Guid IdTerminal { get; set; }
        public List<UrlListInfo> UrlListInfos { get; set; }
        public int? ButtonId { get; set; }
        // Секция 
        [ForeignKey("ButtonId")]
        public NewsModel Button { get; set; }

        public UrlInfo()
        {
            UrlListInfos = new List<UrlListInfo>();
        }
    }

    public class UrlListInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public UrlInfo urlInfo { get; set; }
    }
}
