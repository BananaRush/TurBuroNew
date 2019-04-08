using ModelData;

namespace TourAgency
{
    static class Config
    {
        public static string GetWebAPI()
        {
            return SiteSettings.ReadSetting("WebApiHost") + "/api";
        }
    }
}
