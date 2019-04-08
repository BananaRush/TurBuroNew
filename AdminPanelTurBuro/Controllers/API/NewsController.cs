using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StorageAPI.Models;
using StorageAPI.Models.Database;

namespace AdminPanelTurBuro.Controllers.API
{
    public class NewsController : Controller
    {
        private readonly TourAgencyContext _context;

        public NewsController(TourAgencyContext context)
        {
            _context = context;
        }

        // GET api/<controller>
        public IEnumerable<News> Get()
        {
            return _context.NewsModel.ToList().Select(News.CreateFromDb);
        }

        [HttpGet]
        public string GetLive()
        {
            return _context.LivesString.ToList().FirstOrDefault()?.LiveString;
        }

        [HttpGet]
        public void SetLive(string value)
        {
            var str = _context.LivesString.ToList();
            _context.LivesString.RemoveRange(str);
            _context.SaveChanges();
            _context.LivesString.Add(new Live() { LiveString = value });
            _context.SaveChanges();
        }

        // GET api/<controller>/5
        public News Gets(long id)
        {
            var dbNews = _context.NewsModel.Find(id);

            if (dbNews != null)
            {
                return News.CreateFromDb(dbNews);
            }

            return null;
        }

        // POST api/<controller>
        public long Post([FromBody]News value)
        {
            var dbNews = new NewsModel
            {
                AdvertisingImageUri = value.AdvertisingImageUri,
                //Content = value.Content,
                ContentType = value.ContentType,
                IconUri = value.IconUri,
                Text = value.Text
            };

            _context.NewsModel.Add(dbNews);
            _context.SaveChanges();

            return dbNews.Id;
            
        }

        // PUT api/<controller>/5
        public long Put(long id, [FromBody]News value)
        {
            var dbNews = _context.NewsModel.Find(id);

            if (dbNews != null)
            {
                dbNews.AdvertisingImageUri = value.AdvertisingImageUri;
                //dbNews.Content = value.Content;
                dbNews.ContentType = value.ContentType;
                dbNews.IconUri = value.IconUri;
                dbNews.Text = value.Text;

                _context.SaveChanges();

                return dbNews.Id;
            }
            

            return -1;
        }

        // DELETE api/<controller>/5
        public void Delete(long id)
        {
            var dbNews = _context.NewsModel.Find(id);

            if (dbNews == null) return;

            _context.NewsModel.Remove(dbNews);
            _context.SaveChanges();
        }
    }
}
