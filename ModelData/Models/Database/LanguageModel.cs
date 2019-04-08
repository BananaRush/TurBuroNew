using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelData.Models.Database
{
    public class LanguageModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CodeLang { get; set; }
        public bool IsActive { get; set; }
        public string IcoLang { get; set; }
    }
}
