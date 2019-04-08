using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace AdminPanel.Utilit
{
    public static class SaveImageBase64
    {
        public static string Save(string name, string data)
        {
            if (string.IsNullOrEmpty(name))
                return string.Empty;
            try
            {
                string base64str = data.Substring(data.IndexOf(',') + 1);
                string pach = Path.Combine("FileLoad", name);
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FileLoad");

                if (!Directory.Exists(fullPath))
                    Directory.CreateDirectory(fullPath);

                byte[] bytes = Convert.FromBase64String(base64str);
                File.WriteAllBytes(Path.Combine(fullPath, name), bytes);
                return pach;
            }
            catch(Exception el)
            {
                return string.Empty;
            }
        }
    }
}