using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using StorageAPI.Models;
using StorageAPI.Models.Database;
using Slider = StorageAPI.Models.Slider;

namespace StorageAPI.Controllers
{
    public class SliderController : ApiController
    {
        [HttpGet]
        public async Task<List<StorageAPI.Models.Database.Slider>> Get()
        {
            using (var context = new TourAgencyContext())
            {
                return await context.Slider.ToListAsync();
            }
        }
        // GET api/<controller>
        //public IEnumerable<Slider> Get(int limit, int offset)
        //{
        //    limit %= 1000;

        //    using (var context = new TourAgencyContext())
        //    {
        //        return context.Slider.ToList().Select(Slider.CreateFromDb);
        //    }
        //}

        //// GET api/<controller>/5
        //public Slider Get(long id)
        //{
        //    using (var context = new TourAgencyContext())
        //    {
        //        var dbSlider = context.Slider.Find(id);

        //        if (dbSlider != null)
        //        {
        //            return Slider.CreateFromDb(dbSlider);
        //        }
        //    }

        //    return null;
        //}

        //// POST api/<controller>
        //public long Post([FromBody]Slider value)
        //{
        //    using (var context = new TourAgencyContext())
        //    {
        //        var dbSlider = new StorageAPI.Models.Database.Slider
        //        {
        //            Content = value.Content?? "тест",
        //            ContentType = value.ContentType,
        //            Caption = value.Caption ?? "тест"
        //        };
        //        context.Slider.Add(dbSlider);
        //        context.SaveChanges();

        //        return dbSlider.Id;
        //    }
        //}

        //// PUT api/<controller>/5
        //public long Put(long id, [FromBody]Slider value)
        //{
        //    using (var context = new TourAgencyContext())
        //    {
        //        var dbSlider = context.Slider.Find(id);

        //        if (dbSlider != null)
        //        {
        //            dbSlider.Content = value.Content;
        //            dbSlider.ContentType = value.ContentType;
        //            dbSlider.Caption = value.Caption;

        //            context.SaveChanges();

        //            return dbSlider.Id;
        //        }
        //    }

        //    return -1;
        //}

        //// DELETE api/<controller>/5
        //public void Delete(long id)
        //{
        //    using (var context = new TourAgencyContext())
        //    {
        //        var dbSlider = context.Slider.Find(id);

        //        if (dbSlider == null) return;

        //        context.Slider.Remove(dbSlider);
        //        context.SaveChanges();
        //    }
        //}

    }
}