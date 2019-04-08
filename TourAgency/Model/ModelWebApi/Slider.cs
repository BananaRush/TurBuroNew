
namespace TourAgency.Model.ModelWebApi
{
    public partial class Slider
    {
        public long Id { get; set; }

        public string Content { get; set; }

        public string Caption { get; set; }

        public SliderContentType ContentType { get; set; }
    }
}
