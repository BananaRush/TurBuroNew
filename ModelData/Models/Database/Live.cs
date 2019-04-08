using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StorageAPI.Models
{
    public class Live
    {
        [StringLength(44)]
        public string LiveString { get; set; }

        public int id { get; set; }
    }
}