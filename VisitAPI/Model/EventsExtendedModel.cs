using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitAPI.Model
{
    public class EventsExtendedModel
    {
        public int total_results { get; set; }
        public List<EventsModel> results { get; set; }

        public EventsExtendedModel()
        {
            results = new List<EventsModel>();
        }
    }
}
