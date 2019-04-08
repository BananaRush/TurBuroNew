using ModelData.Models.Database;
using StorageAPI.Models.Database;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelData.Model.Database
{
    public class Information
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Info { get; set; }
        // Опубликованна?
        public bool IsPublick { get; set; }
        // Категория
        public string Cat { get; set; }
        //ИД ТЕРМИНАЛА
        public Guid IdTerminal { get; set; }
        public DateTime DateTime { get; set; }

        public int? SectionsId { get; set; }
        // Секция 
        [ForeignKey("SectionsId")]
        public virtual Section Section { get; set; }

        public int? ButtonId { get; set; }
        // Секция 
        [ForeignKey("ButtonId")]
        public NewsModel Button { get; set; }

    }
}
