using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using StorageAPI.Models;
using StorageAPI.Models.Database;
using Slider = StorageAPI.Models.Slider;
using Test = StorageAPI.Models.Test;

namespace StorageAPI.Controllers
{
    public class TestController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<Test> Get(int limit, int offset)
        {
            limit %= 1000;
            using (var context = new TourAgencyDataBase())
            {
                var test = context.Test.ToList().Select(Test.CreateFromDb);
                return context.Test.ToList().Select(Test.CreateFromDb);
            }
        }

        [HttpGet]
        public bool SetVoit([FromUri]string title, [FromUri]int indexId)
        {
            try
            {
                using (var context = new TourAgencyDataBase())
                {
                    var a = context.Test.FirstOrDefault(f => f.Question == title);
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
                        context.Test.AddOrUpdate(a);
                    }
                    else
                    {
                        model.Voits++;
                        a.Answer = JsonConvert.SerializeObject(models);
                        context.Test.AddOrUpdate(a);
                    }
                    context.SaveChanges();
                    return true;

                }

            }
            catch
            {
                return false;
            }
        }



        // GET api/<controller>/5
        public Test Get(long id)
        {
            using (var context = new TourAgencyDataBase())
            {
                var dbTest = context.Test.Find(id);
                if (dbTest != null)
                {
                    return Test.CreateFromDb(dbTest);
                }
            }

            return null;
        }

        // POST api/<controller>
        public long Post([FromBody]Test value)
        {
            using (var context = new TourAgencyDataBase())
            {
                var dbTest = new StorageAPI.Models.Database.Test()
                {
                    Answer = String.Empty,
                    Question = value.Question,
                    Responses = JsonConvert.SerializeObject(value.Responses),
                    Title = value.Title,
                    Lang = value.Lang,
                };
                
                context.Test.Add(dbTest);
                context.SaveChanges();
                return dbTest.Id;
            }
        }

        // PUT api/<controller>/5
        public long Put(long id, [FromBody]Test value)
        {
            using (var context = new TourAgencyDataBase())
            {
                var dbTest = context.Test.Find(id);

                if (dbTest != null)
                {
                    dbTest.Answer = value.Answer;
                    dbTest.Question = value.Question;
                    dbTest.Responses = JsonConvert.SerializeObject(value.Responses);
                    dbTest.Title = value.Title;
                    dbTest.Lang = value.Lang;
                    context.SaveChanges();

                    return dbTest.Id;
                }
            }

            return -1;
        }

        // DELETE api/<controller>/5
        public void Delete(long id)
        {
            using (var context = new TourAgencyDataBase())
            {
                var dbTest = context.Test.Find(id);

                if (dbTest == null) return;

                context.Test.Remove(dbTest);
                context.SaveChanges();
            }
        }
    }
}