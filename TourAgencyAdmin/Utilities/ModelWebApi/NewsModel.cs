namespace TourAgencyAdmin.Utilities.ModelWebApi
{
    public partial class NewsModel
    {
        public long Id { get; set; }

        public string Text { get; set; }

        public string IconUri { get; set; }

        public string Content { get; set; }

        public string AdvertisingImageUri { get; set; }

        public NewsContentType ContentType { get; set; }
    }
}
