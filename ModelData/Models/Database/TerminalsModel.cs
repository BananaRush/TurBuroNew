using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
    Список терминалов  
*/
namespace ModelData.Models.Database
{
    public class TerminalsModel
    {
        public int Id { get; set; }
        public Guid TerminalId { get; set; }
        public string Name { get; set; }
        public bool IsAutorizate { get; set; }
        public bool IsSelect { get; set; }
    }
}
