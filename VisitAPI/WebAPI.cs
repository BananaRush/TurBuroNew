using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using VisitAPI.Model;

namespace VisitAPI
{
    public class WebAPI
    {
        private static readonly string hostv1 = "http://www.visit-petersburg.ru/api/v1";
        private static readonly string hostv2 = "http://www.visit-petersburg.ru/api/v2.1";
        private static HttpClient httpClient = new HttpClient();

        static WebAPI()
        {
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static class Events
        {
            public static async Task<EventsExtendedModel> GetExtended(int Page = 1, int index = 10, string lang = "ru")
            {
                try
                {
                    HttpResponseMessage client = await httpClient.GetAsync($"{hostv2}/{lang}/news/?page={Page}&page_size={index}").ConfigureAwait(false);
                    string result = await client.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<EventsExtendedModel>(result);
                }
                catch (Exception e)
                {
                    return null;
                }
            }

            public static async Task<List<EventsModel>> Get(int index = 10, string lang = "ru")
            {
                try
                {
                    HttpResponseMessage client = await httpClient.GetAsync($"{hostv2}/{lang}/eventsnews/?total={index}").ConfigureAwait(false);
                    string result = await client.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<List<EventsModel>>(result);
                }
                catch (Exception e)
                {
                    return null;
                }
            }

            public static async Task<EventsInfoModel> GetEvent(int id, string lang = "ru")
            {
                try
                {
                    HttpResponseMessage client = await httpClient.GetAsync($"{hostv2}/{lang}/news/{id}").ConfigureAwait(false);
                    string result = await client.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<EventsInfoModel>(result);
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        public static class Account
        {
            public static async Task<AccauntAnswerModel> Login(LoginModel model)
            {
                var objectContent = JsonConvert.SerializeObject(model);
                StringContent value = new StringContent(objectContent, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage client = await httpClient.PostAsync($"{hostv1}/account/login/", value).ConfigureAwait(false);
                    string result = await client.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (client.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return new AccauntAnswerModel(true, result);
                    }
                    else
                    {
                        return new AccauntAnswerModel(false, result);
                    }
                }
                catch
                {
                    return null;
                }
            }

            public static async Task<AccauntAnswerModel> Registration(RegistrationModel model)
            {
                var objectContent = JsonConvert.SerializeObject(model);
                StringContent value = new StringContent(objectContent, Encoding.UTF8, "application/json");
                try
                {
                    HttpResponseMessage client = await httpClient.PostAsync($"{hostv1}/account/", value).ConfigureAwait(false);
                    string result = await client.Content.ReadAsStringAsync().ConfigureAwait(false);

                    if (client.StatusCode == System.Net.HttpStatusCode.OK || client.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        return new AccauntAnswerModel(true, result);
                    }
                    else
                    {
                        return new AccauntAnswerModel(false, result);
                    }
                }
                catch 
                {
                    return null;
                }
            }
        }
    }
}
