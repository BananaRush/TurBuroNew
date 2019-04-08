using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitAPI.Model
{
    public class EventsInfoModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Intro { get; set; }
        public string Text { get; set; }
        public string Image { get; set; }
        public bool Important { get; set; }
        public string Created { get; set; }
        public string Category { get; set; }
    }
}
