using Microsoft.AspNetCore.Mvc;
using StorageAPI.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Slider = StorageAPI.Models.Slider;

namespace AdminPanelTurBuro.Controllers.API
{
    public class SliderController : Controller
    {
        private readonly TourAgencyContext _context;

        public SliderController(TourAgencyContext context)
        {
            _context = context;
        }

        // GET api/<controller>
        public IEnumerable<Slider> Get(int limit, int offset)
        {
            limit %= 1000;
            return _context.Slider.ToList().Select(Slider.CreateFromDb);
        }

        // GET api/<controller>/5
        public Slider Get(long id)
        {
            var dbSlider = _context.Slider.Find(id);

            if (dbSlider != null)
            {
                return Slider.CreateFromDb(dbSlider);
            }

            return null;
        }

        // POST api/<controller>
        public long Post([FromBody]Slider value)
        {
            var dbSlider = new StorageAPI.Models.Database.Slider
            {
                Content = value.Content ?? "тест",
                ContentType = value.ContentType,
                Caption = value.Caption ?? "тест"
            };
            _context.Slider.Add(dbSlider);
            _context.SaveChanges();

            return dbSlider.Id;
            
        }

        // PUT api/<controller>/5
        public long Put(long id, [FromBody]Slider value)
        {
            var dbSlider = _context.Slider.Find(id);

            if (dbSlider != null)
            {
                dbSlider.Content = value.Content;
                dbSlider.ContentType = value.ContentType;
                dbSlider.Caption = value.Caption;

                _context.SaveChanges();

                return dbSlider.Id;
            }
            return -1;
        }

        // DELETE api/<controller>/5
        public void Delete(long id)
        {
            var dbSlider = _context.Slider.Find(id);

            if (dbSlider == null) return;

            _context.Slider.Remove(dbSlider);
            _context.SaveChanges();
        }
    }
}
