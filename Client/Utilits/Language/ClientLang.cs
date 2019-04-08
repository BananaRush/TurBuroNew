using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Utilits.Language
{
    public static class ClientLang
    {
        public static IClientWord Lang
        {
            get
            {
                return LangWord.Lang;
            }
        }

        static public void SetLanguage(string language)
        {
            string Paths = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Language");
            Paths = RtPath(Paths, language);

            if (File.Exists(Paths))
            {
                LangWord.Lang = XmlSerializable.Deserialization(Paths);
            }
            else
            {
                XmlSerializable.Serializable();
            }
        }

        static public string RtPath(string path, string subdomen)
        {
            try
            {
                List<string> filesname = Directory.GetFiles(path, "*.xml").ToList<string>();

                if (filesname == null)
                {
                    return string.Empty;
                }

                for (int i = 0; i < filesname.Count; i++)
                {
                    string str = Path.GetFileNameWithoutExtension(filesname[i]);
                    if (!string.IsNullOrEmpty(str) && str.ToUpperInvariant() == subdomen.ToUpperInvariant())
                    {
                        return filesname[i];
                    }
                }
            }
            catch (Exception e)
            {

            }

            return string.Empty;
        }
    }
}
