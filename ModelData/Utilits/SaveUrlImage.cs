using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ModelData.Utilits
{
    static public class SaveUrlImage
    {
        private static string AppPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Config.GetFileDirectory());
        private static WebClient client;

        static SaveUrlImage()
        {
            client = new WebClient();
            if (!Directory.Exists(AppPath))
                Directory.CreateDirectory(AppPath);
        }

        static public string Save(string url)
        {
            if (string.IsNullOrEmpty(url))
                return string.Empty;

            string fileName = Path.GetFileName(url);
            string localFile = Path.Combine(AppPath, fileName);

            try
            {
                if (!File.Exists(localFile))
                {
                    WebClient client1 = new WebClient();
                    client1.DownloadFile(new Uri(url), localFile);
                }
            }
            catch(Exception el)
            {
                return string.Empty;
            }

            return localFile;
        }

        static public string GetByteImg(string url)
        {
            if (string.IsNullOrEmpty(url))
                return string.Empty;

            string base64string = string.Empty;
            string fileName = Path.GetFileName(url);
            string localFile = Path.Combine(AppPath, fileName);

            if (File.Exists(localFile))
            {
                byte[] file = File.ReadAllBytes(localFile);
                base64string = Convert.ToBase64String(file);
            }

            return String.Format(@"data:image/jpeg;base64,{0}", base64string);
        }
    }
}
