using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelData.DataCom
{
    public class FileLoaderModel
    {
        public string FileName { get; set; }
        public bool FileExist { get; set; }
        public string Patch { get; set; }
        public long Length { get; set; }
        public int Offset { get; set; }
        public int ByteReadCout { get; set; }
        public int BlockSize { get; set; }
        public byte[] Block { get; set; }
        public string HashMD5 { get; set; }
    }
}
