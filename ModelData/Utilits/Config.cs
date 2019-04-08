using System;

namespace ModelData.Utilits
{
    public static class Config
    {
        public static string HostImage
        {
            get => SiteSettings.ReadSetting("WebApiHost") + "/Files/Image/";
        }

        public static string GetWebAPI()
        {
            return SiteSettings.ReadSetting("WebApiHost") + "/api";
        }

        public static string GetHost()
        {
            return SiteSettings.ReadSetting("WebApiHost");
        }

        public static string GetFileDirectory()
        {
            return SiteSettings.ReadSetting("FileDirectory");
        }

        public static string GetIndificator()
        {
            return SiteSettings.ReadSetting("Indificator");
        }
    }
}
