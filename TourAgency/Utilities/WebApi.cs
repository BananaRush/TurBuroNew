using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ModelData.Model.Database;
using ModelData.Models.Database;
using Newtonsoft.Json;
using StorageAPI.Models;
using StorageAPI.Models.Database;
using TourAgency.Controls;
using TourAgency.Model.ModelWebApi;

namespace TourAgency.Utilities
{
    public static class WebApi
    {
        private static readonly string host = Config.GetWebAPI();
        private static HttpClient httpClient = new HttpClient();

        static WebApi()
        {
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static class News
        {
            public static async Task<IEnumerable<NewsModel>> Get()
            {

                return await await httpClient.GetAsync($"{host}/news/get").ContinueWith(
                    async r =>
                    {
                        if (r.IsFaulted) return null;
                        try
                        {
                            var result = await r.Result.Content.ReadAsStringAsync();
                            return JsonConvert.DeserializeObject<IEnumerable<NewsModel>>(result);
                        }
                        catch
                        {
                            return null;
                        }
                    });
            }

            public static async Task<bool> SetLive(string value)
            {
               return await httpClient.GetAsync($"{host}/news/setlive?value={value}").ContinueWith(r =>
                {
                    if (r.IsFaulted) return false;
                    return true;
                });
            }


            public static async Task<string> GetLive()
            {
                return await  await httpClient.GetAsync($"{host}/news/getlive").ContinueWith(async r=>
                {
                    if (r.IsFaulted) return null;
                    try
                    {

                    string res = await r.Result.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<string>(res);

                    }
                    catch (Exception e)
                    {
                        return null;
                    }
                });
            }

            public static async Task<NewsModel> Get(long id)
            {
                return await await httpClient.GetAsync($"{host}/news/get/{id}").ContinueWith(async r =>
                {
                    if (r.IsFaulted) return null;
                    try
                    {
                        var result = await r.Result.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<NewsModel>(result);
                    }
                    catch 
                    {
                        return null;
                    }
                });
            }


            public static async Task<long> Post(NewsModel value)
            {
                StringContent sc =
                    new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
                return await await httpClient.PostAsync($"{host}/news", sc).ContinueWith(async r =>
                {
                    if (r.IsFaulted) return -1;
                    try
                    {
                        var result = await r.Result.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<long>(result);
                    }
                    catch 
                    {
                        return -1;
                    }
                });
            }


            public static async Task<long> Put(long id, NewsModel value)
            {
                StringContent sc = new StringContent($"id={id}&value={JsonConvert.SerializeObject(value)}",
                    Encoding.UTF8, "application/json");
                return await await httpClient.PutAsync($"{host}/news", sc).ContinueWith(async r =>
                {
                    if (r.IsFaulted) return -1;
                    try
                    {
                        var result = await r.Result.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<long>(result);
                    }
                    catch
                    {
                        return -1;
                    }
                });
            }


            public static async Task Delete(long id)
            {
                await httpClient.DeleteAsync($"{host}/news/{id}");
            }
        }

        public static class Slider
        {

            public static async Task<IEnumerable<Model.ModelWebApi.Slider>> Get(int limit, int offset)
            {
                return await await httpClient.GetAsync($"{host}/slider/get?limit={limit}&offset={offset}").ContinueWith(
                    async r =>
                    {
                        if (r.IsFaulted) return null;
                        try
                        {

                            var result = await r.Result.Content.ReadAsStringAsync();
                            return JsonConvert.DeserializeObject<IEnumerable<Model.ModelWebApi.Slider>>(result);

                        }
                        catch
                        {
                            return null;
                        }
                    });
            }

            public static async Task<Model.ModelWebApi.Slider> Get(long id)
            {
                return await await httpClient.GetAsync($"{host}/slider/{id}").ContinueWith(async r =>
                {
                    if (r.IsFaulted) return null;
                    try
                    {
                        var result = await r.Result.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<Model.ModelWebApi.Slider>(result);
                    }
                    catch
                    {
                        return null;
                    }
                });
            }


            public static async Task<long> Post(Model.ModelWebApi.Slider value)
            {
                StringContent sc = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
                return await await httpClient.PostAsync($"{host}/slider", sc).ContinueWith(async r =>
                {
                    if (r.IsFaulted) return -1;
                    try
                    {
                        var result = await r.Result.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<long>(result);
                    }
                    catch 
                    {
                        return -1;
                    }
                });
            }

            public static async Task<long> Put(long id, Model.ModelWebApi.Slider value)
            {
                StringContent sc = new StringContent($"id={id}&value={JsonConvert.SerializeObject(value)}", Encoding.UTF8, "application/json");
                return await await httpClient.PutAsync($"{host}/slider", sc).ContinueWith(async r =>
                {
                    if (r.IsFaulted) return -1;
                    try
                    {
                        var result = await r.Result.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<long>(result);
                    }
                    catch 
                    {
                        return -1;
                    }
                });
            }

            public static async Task Delete(long id)
            {
                 await httpClient.DeleteAsync($"{host}/slider/{id}");
            }

        }

        public static class Test
        {
            //public static async Task<IEnumerable<Model.ModelWebApi.Test>> Get(int limit, int offset)
            //{
            //    return await await httpClient.GetAsync($"{host}/test/get?limit={limit}&offset={offset}").ContinueWith(
            //        async r =>
            //        {
            //            if (r.IsFaulted) return null;
            //            try
            //            {

            //                var result = await r.Result.Content.ReadAsStringAsync();
            //                return JsonConvert.DeserializeObject<IEnumerable<Model.ModelWebApi.Test>>(result);

            //            }
            //            catch
            //            {
            //                return null;
            //            }
            //        });
            //}

            public static async Task<bool> MakeVoit(string title, int indexId)
            {
                return await await httpClient.GetAsync($"{host}/test/SetVoit?title={title}&indexId={indexId}")
                    .ContinueWith(
                        async r =>
                        {
                            if (r.IsFaulted) return false;
                            string res = await r.Result.Content.ReadAsStringAsync();
                            return JsonConvert.DeserializeObject<bool>(res);
                        });
            }

            //public static async Task<Model.ModelWebApi.Test> Get(long id)
            //{
            //    return await await httpClient.GetAsync($"{host}/test/{id}").ContinueWith(async r =>
            //    {
            //        if (r.IsFaulted) return null;
            //        try
            //        {
            //            var result = await r.Result.Content.ReadAsStringAsync();
            //            return JsonConvert.DeserializeObject<Model.ModelWebApi.Test>(result);
            //        }
            //        catch
            //        {
            //            return null;
            //        }
            //    });
            //}


            //public static async Task<long> Post(Model.ModelWebApi.Test value)
            //{
            //    StringContent sc = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
            //    return await await httpClient.PostAsync($"{host}/test", sc).ContinueWith(async r =>
            //    {
            //        if (r.IsFaulted) return -1;
            //        try
            //        {
            //            var result = await r.Result.Content.ReadAsStringAsync();
            //            return JsonConvert.DeserializeObject<long>(result);
            //        }
            //        catch
            //        {
            //            return -1;
            //        }
            //    });
            //}

            //public static async Task<long> Put(long id, Model.ModelWebApi.Test value)
            //{
            //    StringContent sc = new StringContent($"id={id}&value={JsonConvert.SerializeObject(value)}", Encoding.UTF8, "application/json");
            //    return await await httpClient.PutAsync($"{host}/test", sc).ContinueWith(async r =>
            //    {
            //        if (r.IsFaulted) return -1;
            //        try
            //        {
            //            var result = await r.Result.Content.ReadAsStringAsync();
            //            return JsonConvert.DeserializeObject<long>(result);
            //        }
            //        catch
            //        {
            //            return -1;
            //        }
            //    });
            //}

            public static async Task Delete(long id)
            {
                await httpClient.DeleteAsync($"{host}/test/{id}");
            }

        }

        public static class Users
        {
            //public static async Task<User> Auth(string login, string password)
            //{
            //    return await await httpClient.GetAsync($"{host}/user/auth?login={login}&password={password}").ContinueWith(async r =>
            //    {
            //        if (r.IsFaulted) return null;
            //        try
            //        {
            //            var result = await r.Result.Content.ReadAsStringAsync();
            //            return JsonConvert.DeserializeObject<User>(result);
            //        }
            //        catch
            //        {
            //            return null;
            //        }
            //    });
            //}

            public static async Task<ExtendedUser> Get(long id)
            {
                return await await httpClient.GetAsync($"{host}/user?id={id}").ContinueWith(async r =>
                {
                    if (r.IsFaulted) return null;
                    try
                    {
                        var result = await r.Result.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<ExtendedUser>(result);
                    }
                    catch
                    {
                        return null;
                    }

                });
            }


            public static async Task<long> Post(ExtendedUser value)
            {
                var test = JsonConvert.SerializeObject(value);
                StringContent sc = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
                return await await httpClient.PostAsync($"{host}/user/post", sc).ContinueWith(async r =>
                {
                    if (r.IsFaulted) return -1;
                    try
                    {
                        var result = await r.Result.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<long>(result);
                    }
                    catch
                    {
                        return -1;
                    }

                });
            }

            public static async Task<long> Put(long id, ExtendedUser value)
            {
                StringContent sc = new StringContent($"id={id}&value={JsonConvert.SerializeObject(value)}", Encoding.UTF8,"application/json");
                return await await httpClient.PutAsync($"{host}/user", sc).ContinueWith(async r =>
                {
                    if (r.IsFaulted) return -1;
                    try
                    {
                        var result = await r.Result.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<long>(result);
                    }
                    catch
                    {
                        return -1;
                    }
                });
            }

            public static async Task<long> TestResultAdd(AccomplishedTestParams value)
            {
                StringContent sc = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
                return await await httpClient.PutAsync($"{host}/user", sc).ContinueWith(async r =>
                {
                    if (r.IsFaulted) return -1;
                    try
                    {
                        var result = await r.Result.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<long>(result);
                    }
                    catch
                    {
                        return -1;
                    }

                });
            }

            public static async Task Delete(long id)
            {
                await httpClient.DeleteAsync($"{host}/user/{id}");
            }
        }

        public static class MenuButton
        {
            public static async Task<List<NewsModel>> Get()
            {
                try
                {
                    HttpResponseMessage client = await httpClient.GetAsync($"{host}/News/Get").ConfigureAwait(false);
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
    }
}