using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TourAgencyAdmin.Utilities.ModelWebApi;

namespace TourAgencyAdmin.Utilities
{
    public static class WebApi
    {
        private static readonly string host = "http://195.133.1.197/api";
        private static HttpClient httpClient = new HttpClient();

        static WebApi()
        {
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static class News
        {
            public static async Task<IEnumerable<NewsModel>> Get(int limit, int offset)
            {

                return await await httpClient.GetAsync($"{host}/news/get?limit={limit}&offset={offset}").ContinueWith(
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
                return await await httpClient.GetAsync($"{host}/news/getlive").ContinueWith(async r =>
                {
                    if (r.IsFaulted) return null;
                    try
                    {

                        string res = await r.Result.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<string>(res);

                    }
                    catch
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
                return await await httpClient.PostAsync($"{host}/news/post", sc).ContinueWith(async r =>
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
                StringContent sc = new StringContent($"{JsonConvert.SerializeObject(value)}",
                    Encoding.UTF8, "application/json");
                return await await httpClient.PutAsync($"{host}/news/put/{id}", sc).ContinueWith(async r =>
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
                await httpClient.DeleteAsync($"{host}/news/Delete/{id}");
            }
        }

        public static class Slider
        {

            public static async Task<IEnumerable<TourAgencyAdmin.Utilities.ModelWebApi.Slider>> Get(int limit, int offset)
            {
                return await await httpClient.GetAsync($"{host}/slider/get?limit={limit}&offset={offset}").ContinueWith(
                    async r =>
                    {
                        if (r.IsFaulted) return null;
                        try
                        {

                            var result = await r.Result.Content.ReadAsStringAsync();
                            return JsonConvert.DeserializeObject<IEnumerable<TourAgencyAdmin.Utilities.ModelWebApi.Slider>>(result);

                        }
                        catch
                        {
                            return null;
                        }
                    });
            }

            public static async Task<TourAgencyAdmin.Utilities.ModelWebApi.Slider> Get(long id)
            {
                return await await httpClient.GetAsync($"{host}/slider/{id}").ContinueWith(async r =>
                {
                    if (r.IsFaulted) return null;
                    try
                    {
                        var result = await r.Result.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<TourAgencyAdmin.Utilities.ModelWebApi.Slider>(result);
                    }
                    catch
                    {
                        return null;
                    }
                });
            }


            public static async Task<long> Post(TourAgencyAdmin.Utilities.ModelWebApi.Slider value)
            {
                StringContent sc = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
                return await await httpClient.PostAsync($"{host}/slider/post", sc).ContinueWith(async r =>
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

            public static async Task<long> Put(long id, TourAgencyAdmin.Utilities.ModelWebApi.Slider value)
            {

                StringContent sc = new StringContent($"{JsonConvert.SerializeObject(value)}", Encoding.UTF8, "application/json");
                return await await httpClient.PutAsync($"{host}/slider/put/{id}", sc).ContinueWith(async r =>
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
                 await httpClient.DeleteAsync($"{host}/slider/Delete/{id}");
            }

        }


        public static class Test
        {
            public static async Task<IEnumerable<TourAgencyAdmin.Utilities.ModelWebApi.Test>> Get(int limit, int offset)
            {
                return await await httpClient.GetAsync($"{host}/test/get?limit={limit}&offset={offset}").ContinueWith(
                    async r =>
                    {
                        if (r.IsFaulted) return null;
                        try
                        {

                            var result = await r.Result.Content.ReadAsStringAsync();
                            return JsonConvert.DeserializeObject<IEnumerable<TourAgencyAdmin.Utilities.ModelWebApi.Test>>(result);

                        }
                        catch
                        {
                            return null;
                        }
                    });
            }

            public static async Task<TourAgencyAdmin.Utilities.ModelWebApi.Test> Get(long id)
            {
                return await await httpClient.GetAsync($"{host}/test/{id}").ContinueWith(async r =>
                {
                    if (r.IsFaulted) return null;
                    try
                    {
                        var result = await r.Result.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<TourAgencyAdmin.Utilities.ModelWebApi.Test>(result);
                    }
                    catch
                    {
                        return null;
                    }
                });
            }


            public static async Task<long> Post(TourAgencyAdmin.Utilities.ModelWebApi.Test value)
            {
                var tes = JsonConvert.SerializeObject(value);
                StringContent sc = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
                return await await httpClient.PostAsync($"{host}/test/post", sc).ContinueWith(async r =>
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

            public static async Task<long> Put(long id, TourAgencyAdmin.Utilities.ModelWebApi.Test value)
            {
                StringContent sc = new StringContent($"{JsonConvert.SerializeObject(value)}", Encoding.UTF8, "application/json");
                return await await httpClient.PutAsync($"{host}/test/put/{id}", sc).ContinueWith(async r =>
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
                await httpClient.DeleteAsync($"{host}/test/Delete/{id}");
            }

        }

        public static class Users
        {
            public static async Task<User> Auth(string login, string password)
            {
                return await await httpClient.GetAsync($"{host}/user/auth?login={login}&password={password}").ContinueWith(async r =>
                {
                    if (r.IsFaulted) return null;
                    try
                    {
                        var result = await r.Result.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<User>(result);
                    }
                    catch
                    {
                        return null;
                    }
                });
            }

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
    }
}