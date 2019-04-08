using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StorageAPI.Models.Database
{
    [Table("ConnectVideo")]
    public class VideoConnect
    {
        public string Name { get; set; }
        public bool IsBusy { get; set; }
        public int Id { get; set; }
    }
}