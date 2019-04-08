using Client.Utilits.Language;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Client.Utilits
{
    static internal class XmlSerializable
    {
        static internal bool Serializable()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(LangWord));

            // получаем поток, куда будем записывать сериализованный объект
            using (FileStream fs = new FileStream("D:\\RU.xml", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, LangWord.Lang);
            }

            return false;
        }

        static public LangWord Deserialization(string path)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(LangWord));
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                return formatter.Deserialize(fs) as LangWord;
            }
        }
    }
}
