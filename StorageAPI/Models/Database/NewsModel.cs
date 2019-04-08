namespace StorageAPI.Models.Database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NewsModel")]
    public partial class NewsModel
    {
        public long Id { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        [StringLength(1000)]
        public string IconUri { get; set; }

        public string Content { get; set; }

        [StringLength(1000)]
        public string AdvertisingImageUri { get; set; }

        [DataType("int")]
        public NewsContentType ContentType { get; set; }
    }
}
