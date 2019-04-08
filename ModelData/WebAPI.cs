using ModelData.DataCom;
using ModelData.Model.Database;
using ModelData.Models;
using ModelData.Models.Database;
using ModelData.Utilits;
using Newtonsoft.Json;
using StorageAPI.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ModelData
{
    public static class WebApi
    {
        private static readonly string host = Config.GetWebAPI();
        private static HttpClient httpClient = new HttpClient();

        static WebApi()
        {
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static class MenuButton
        {
            public static async Task<List<NewsModel>> Get(DataComModel model)
            {
                var objectContent = JsonConvert.SerializeObject(model);
                StringContent value = new StringContent(objectContent, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage client = await httpClient.PostAsync($"{host}/News/Get", value).ConfigureAwait(false);
                    string result = await client.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<List<NewsModel>>(result);
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        public static class WebBrowser
        {
            public static async Task<UrlInfo> Get(int id)
            {
                try
                {
                    HttpResponseMessage client = await httpClient.GetAsync($"{host}/WebBrowser/Get?id={id}").ConfigureAwait(false);
                    string result = await client.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<UrlInfo>(result);
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        public static class InfoPage
        {
            public static async Task<Information> Get(int id)
            {
                try
                {
                    HttpResponseMessage client = await httpClient.GetAsync($"{host}/Info/Get?id={id}").ConfigureAwait(false);
                    string result = await client.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<Information>(result);
                }
                catch (Exception e)
                {
                    return null;
                }
            }

            public static async Task<Information> GetId(int id)
            {
                try
                {
                    HttpResponseMessage client = await httpClient.GetAsync($"{host}/Info/GetId?id={id}").ConfigureAwait(false);
                    string result = await client.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<Information>(result);
                }
                catch (Exception e)
                {
                    return null;
                }
            }

            public static async Task<List<Information>> GetIdSection(int id)
            {
                try
                {
                    HttpResponseMessage client = await httpClient.GetAsync($"{host}/Info/GetIdSection?id={id}").ConfigureAwait(false);
                    string result = await client.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<List<Information>>(result);
                }
                catch (Exception e)
                {
                    return null;
                }
            }

            public static async Task<List<Information>> GetLents(int id)
            {
                try
                {
                    HttpResponseMessage client = await httpClient.GetAsync($"{host}/Info/GetLents?id={id}").ConfigureAwait(false);
                    string result = await client.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<List<Information>>(result);
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        public static class SectionPage
        {
            public static async Task<List<Section>> Get(int id)
            {
                try
                {
                    HttpResponseMessage client = await httpClient.GetAsync($"{host}/Section/Get?id={id}").ConfigureAwait(false);
                    string result = await client.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<List<Section>>(result);
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        public static class UIElement
        {
            public static async Task<List<UIElementModel>> Get(DataComModel model)
            {
                var objectContent = JsonConvert.SerializeObject(model);
                StringContent value = new StringContent(objectContent, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage client = await httpClient.PostAsync($"{host}/UIElement/Get", value).ConfigureAwait(false);
                    string result = await client.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<List<UIElementModel>>(result);
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        public static class SurveyPage
        {
            public static async Task<List<SurveyModel>> Get(DataComModel model)
            {
                var objectContent = JsonConvert.SerializeObject(model);
                StringContent value = new StringContent(objectContent, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage client = await httpClient.PostAsync($"{host}/Survey/Get", value).ConfigureAwait(false);
                    string result = await client.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<List<SurveyModel>>(result);
                }
                catch (Exception e)
                {
                    return null;
                }
            }

            public static async Task<bool> OptionCout(int id)
            {
                try
                {
                    HttpResponseMessage client = await httpClient.GetAsync($"{host}/Survey/OptionCout?id={id}").ConfigureAwait(false);
                    string result = await client.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<bool>(result);
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        public static class PassagePage
        {
            public static async Task<List<PassageImage>> Get(int id)
            {
                try
                {
                    HttpResponseMessage client = await httpClient.GetAsync($"{host}/Passage/Get?id={id}").ConfigureAwait(false);
                    string result = await client.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<List<PassageImage>>(result);
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        public static class Files
        {
            public static async Task<List<FileModel>> GetAllFiles()
            {
                try
                {
                    HttpResponseMessage client = await httpClient.GetAsync($"{host}/File/GetAllFiles").ConfigureAwait(false);
                    string result = await client.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<List<FileModel>>(result);

                }
                catch
                {
                    return null;
                }
            }

            public static async Task<FileLoaderModel> GetFile(FileLoaderModel Model)
            {
                var objectContent = JsonConvert.SerializeObject(Model);
                StringContent value = new StringContent(objectContent, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage client = await httpClient.PostAsync($"{host}/File/GetFileBlock", value).ConfigureAwait(false);
                    string result = await client.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<FileLoaderModel>(result);

                }
                catch
                {
                    return null;
                }
            }
        }

        public static class Video
        {
            public static async Task<string> GetVideoGuide()
            {
                try
                {
                    HttpResponseMessage client = await httpClient.GetAsync($"{host}/Video/GetVideoGuide").ConfigureAwait(false);
                    string result = await client.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<string>(result);
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        public static class Carousel
        {
            public static async Task<List<StorageAPI.Models.Database.Slider>> GetSliders()
            {
                try
                {
                    HttpResponseMessage client = await httpClient.GetAsync($"{host}/Slider/Get").ConfigureAwait(false);
                    string result = await client.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<List<StorageAPI.Models.Database.Slider>>(result);
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }
    }
}
