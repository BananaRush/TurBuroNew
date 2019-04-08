using System.Collections.Generic;

namespace TourAgency.Model.ModelWebApi
{
    public partial class Test
    {
        public long? Id { get; set; }
        public string Title { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public Lang Lang { get; set; }
        public List<Response> Responses { get; set; }

    }

    public enum Lang
    {
        Ru,
        En
    }

}
