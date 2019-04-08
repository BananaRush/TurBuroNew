using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StorageAPI.Models.Database;
using TourAgency.Model;

namespace TourAgency
{
    public static class API
    {
        private static string GetCurrentLang()
        {
            switch (MainSettings.Current.CurrentLang)
            {
                case MainSettings.Language.English:
                case MainSettings.Language.Chinese:
                    return "en";
                default:
                    return "ru";
            }
        }

        public static async Task<object> SignUp()
        {
            return await Task.Run(() =>
            {
                string lang = GetCurrentLang();
                return "";
            });
        }

        public static async Task<object> LogIn()
        {
            return await Task.Run(() =>
            {
                string lang = GetCurrentLang();
                return "";
            });
        }

        public static async Task<object> PasswordRecovery()
        {
            return await Task.Run(() =>
            {
                string lang = GetCurrentLang();
                return "";
            });
        }

        public static async Task<NewsModel> GetAllNews()
        {
                string lang = GetCurrentLang();
                string address = $"http://www.visit-petersburg.ru/api/v2.1/{lang}/news/";
            using (HttpClient httpClient = new HttpClient())
            {
                return await await httpClient.GetAsync(address).ContinueWith(async r =>
                {
                    if (r.IsFaulted) return null;
                    string result = await r.Result.Content.ReadAsStringAsync();
                    var obj = JsonConvert.DeserializeObject<NewsModel>(result);
                    return obj;
                });
            }
        }

        public static async Task<SingleNewsModel> GetSingleNews(int id)
        {
            return await Task.Run(async () =>
            {
                string lang = GetCurrentLang();
                string address = $"http://www.visit-petersburg.ru/api/v2.1/{lang}/news/{id}/";
                using (HttpClient httpClient = new HttpClient())
                {
                  return  await await httpClient.GetAsync(address).ContinueWith(async r =>
                  {
                      if (r.IsFaulted) return null;
                      string result = await r.Result.Content.ReadAsStringAsync();
                      if (result == null) return null;
                      var obj = JsonConvert.DeserializeObject<SingleNewsModel>(result);
                      return obj;
                  });
                }
            });
        }

        public static async Task<object> GetFAQ()
        {
            return await Task.Run(() =>
            {
                string lang = GetCurrentLang();
                return "";
            });
        }

        private static readonly string api_key =
            "trnsl.1.1.20171214T033350Z.3eaa93f6c81e7627.bc60b333096c4604b42b39ed627f7521fda27050";

        public static async Task<string> MakeTranslate(string lang, string text)
        {
            StringContent sc = new StringContent($"text={text}", Encoding.UTF8, "application/x-www-form-urlencoded");
            using (HttpClient hc = new HttpClient())
            {
                   return await await hc
                        .PostAsync($"https://translate.yandex.net/api/v1.5/tr.json/translate?lang={lang}&key={api_key}",
                            sc).ContinueWith(async r =>
                        {
                            if (r.IsFaulted) return null;
                            var res = await r.Result.Content.ReadAsStringAsync();
                            return JsonConvert.DeserializeObject<ModelYad>(res)?.text.FirstOrDefault();
                        });
        
            }
        }
        
        //TODO получение документов для рассылки смс
        //public static async Task<object> GetDocs()


    }

    public class ModelYad
    {
        public int code { get; set; }
        public string lang { get; set; }
        public string[] text { get; set; }
    }
}
