using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace StorageAPI.Models
{
    public class Test
    {
        public long? Id { get; set; }
        public string Title { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public Lang Lang { get; set; }
        public List<Response> Responses { get; set; }

        public static Test CreateFromDb(Database.Test dbTest)
        {

            var test = new Test
            {
                
                Id = dbTest.Id,
                Answer = dbTest.Answer,
                Question = dbTest.Question,
                Responses = (List<Response>) JsonConvert.DeserializeObject(dbTest.Responses, typeof(List<Response>)),
                Title = dbTest.Title,
                Lang = dbTest.Lang
            };
            
            return test;
        }
    }

    public enum Lang
    {
        Ru,
        En
    }
}