using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using TourAgency.Model.ModelWebApi;

namespace TourAgency.Model
{
    public class Results
    {
        public int id { get; set; }
        public string title { get; set; }
        public string intro { get; set; }
        public string image { get; set; }
        public bool important { get; set; }
        public string created { get; set; }
    }

    //public class NewsModel
    //{
    //    public int total_results { get; set; }
    //    public ObservableCollection<Results> results { get; set; }
    //}

}
