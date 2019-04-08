using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StorageAPI.Models
{
    public class AccomplishedTestParams
    {
        public long UserId { get; set; }
        public long TestId { get; set; }
        public float? Result { get; set; }
    }
}