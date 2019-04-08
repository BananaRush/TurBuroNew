namespace StorageAPI.Models.Database
{
    using ModelData.Interface;
    using ModelData.Model.Database;
    using ModelData.Models.Database;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("NewsModel")]
    public partial class NewsModel : ILnagModel
    {
        public int Id { get; set; }
        public int IdContent { get; set; }
        public string Language { get; set; }
        public string Text { get; set; }
        [StringLength(1000)]
        public string IconUri { get; set; }
        [StringLength(1000)]
        public string AdvertisingImageUri { get; set; }
        [DataType("int")]
        public NewsContentType ContentType { get; set; }
        public List<Information> ListInformation { get; set; }
        public List<PassageImage> ListPassageImages { get; set; }
        public List<UrlInfo> ListUrlInfo { get; set; }
        public List<Section> ListSections { get; set; }
        public List<VideoModel> ListVideo { get; set; }
        public List<TerminalDataModel> ListTerminal { get; set; }

        public NewsModel()
        {
            ListInformation = new List<Information>();
            ListPassageImages = new List<PassageImage>();
            ListUrlInfo = new List<UrlInfo>();
            ListSections = new List<Section>();
            ListVideo = new List<VideoModel>();
            ListTerminal = new List<TerminalDataModel>();
        }
    }
}
