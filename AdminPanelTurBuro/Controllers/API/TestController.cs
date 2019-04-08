using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StorageAPI.Models;
using StorageAPI.Models.Database;
using Slider = StorageAPI.Models.Slider;
using Test = StorageAPI.Models.Test;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdminPanelTurBuro.Controllers.API
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private readonly TourAgencyContext _context;

        public TestController(TourAgencyContext context)
        {
            _context = context;
        }

        // GET api/<controller>
        public IEnumerable<Test> Get(int limit, int offset)
        {
            limit %= 1000;

            var test = _context.Test.ToList().Select(Test.CreateFromDb);
            return _context.Test.ToList().Select(Test.CreateFromDb);
        }

        [HttpGet]
        public bool SetVoit(string title, int indexId)
        {
            try
            {

                var a = _context.Test.FirstOrDefault(f => f.Question == title);
                if (a == null) return false;
                var models = new List<AnswerModel>();
                    
                try
                {
                    models = JsonConvert.DeserializeObject<List<AnswerModel>>(a.Answer) ?? new List<AnswerModel>();
                }
                catch (Exception e)
                {

                }
                var model = models.FirstOrDefault(f => f.TitleTest == title);
                if (model == null)
                {
                    model = new AnswerModel();
                    model.TitleTest = title;
                    model.IndexAnswer = indexId;
                    model.Voits = 1;
                    models.Add(model);
                    a.Answer = JsonConvert.SerializeObject(models);
                    //_context.Test.AddOrUpdate(a);
                }
                else
                {
                    model.Voits++;
                    a.Answer = JsonConvert.SerializeObject(models);
                    //_context.Test.AddOrUpdate(a);
                       
                }

                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }



        // GET api/<controller>/5
        public Test Get(long id)
        {
            var dbTest = _context.Test.Find(id);
            if (dbTest != null)
            {
                return Test.CreateFromDb(dbTest);
            }

            return null;
        }

        // POST api/<controller>
        public long Post([FromBody]Test value)
        {

            var dbTest = new StorageAPI.Models.Database.Test()
            {
                Answer = String.Empty,
                Question = value.Question,
                Responses = JsonConvert.SerializeObject(value.Responses),
                Title = value.Title,
                Lang = value.Lang,
            };

            _context.Test.Add(dbTest);
            _context.SaveChanges();
            return dbTest.Id;
        }

        // PUT api/<controller>/5
        public long Put(long id, [FromBody]Test value)
        {

            var dbTest = _context.Test.Find(id);

            if (dbTest != null)
            {
                dbTest.Answer = value.Answer;
                dbTest.Question = value.Question;
                dbTest.Responses = JsonConvert.SerializeObject(value.Responses);
                dbTest.Title = value.Title;
                dbTest.Lang = value.Lang;
                _context.SaveChanges();

                return dbTest.Id;
            }
           
            return -1;
        }

        // DELETE api/<controller>/5
        public void Delete(long id)
        {
            var dbTest = _context.Test.Find(id);

            if (dbTest == null) return;

            _context.Test.Remove(dbTest);
            _context.SaveChanges();
        }
    }
}
