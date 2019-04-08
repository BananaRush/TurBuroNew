using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourAgency.Model
{
    public class SingleNewsModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public string intro { get; set; }
        public string text { get; set; }
        public string image { get; set; }
        public bool important { get; set; }
        public string created { get; set; }
        public string category { get; set; }
    }

    public class SingleNewsModelPage: SingleNewsModel
    {
        public string[] Images { get; set; }
    }
}
