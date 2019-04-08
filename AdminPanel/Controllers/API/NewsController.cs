using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ModelData.Models;
using StorageAPI.Models;
using StorageAPI.Models.Database;

namespace StorageAPI.Controllers
{
    public class NewsController : ApiController
    {
        // GET api/<controller>
        [HttpPost]
        public async Task<IEnumerable<NewsModel>> Get(DataComModel model)
        {
            using (var context = new TourAgencyContext())
            {
                return await context.NewsModel
                    .AsNoTracking()
                    .Where(r=>r.Language == model.Language)
                    .ToListAsync();
            }
        }

        [HttpGet]
        public void SetLive(string value)
        {
            using (var context = new TourAgencyContext())
            {
              var str=  context.LivesString.ToList();
                context.LivesString.RemoveRange(str);
                context.SaveChanges();
                context.LivesString.Add(new Live() {LiveString = value});
                context.SaveChanges();

            }
        }

        // POST api/<controller>
        public long Post([FromBody]News value)
        {
            using (var context = new TourAgencyContext())
            {
                var dbNews = new NewsModel
                {
                    AdvertisingImageUri = value.AdvertisingImageUri,
                    //Content = value.Content,
                    ContentType = value.ContentType,
                    IconUri = value.IconUri,
                    Text = value.Text
                };

                context.NewsModel.Add(dbNews);
                context.SaveChanges();

                return dbNews.Id;
            }
        }

        // PUT api/<controller>/5
        public long Put(long id, [FromBody]News value)
        {
            using (var context = new TourAgencyContext())
            {
                var dbNews = context.NewsModel.Find(id);

                if (dbNews != null)
                {
                    dbNews.AdvertisingImageUri = value.AdvertisingImageUri;
                    //dbNews.Content = value.Content;
                    dbNews.ContentType = value.ContentType;
                    dbNews.IconUri = value.IconUri;
                    dbNews.Text = value.Text;

                    context.SaveChanges();

                    return dbNews.Id;
                }
            }

            return -1;
        }

        // DELETE api/<controller>/5
        public void Delete(long id)
        {
            using (var context = new TourAgencyContext())
            {
                var dbNews = context.NewsModel.Find(id);

                if (dbNews == null) return;

                context.NewsModel.Remove(dbNews);
                context.SaveChanges();
            }
        }
    }
}