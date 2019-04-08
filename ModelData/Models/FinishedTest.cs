using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace StorageAPI.Models
{
    public class FinishedTest : Test
    {
        public double Result { get; set; }
        public DateTime PassingStamp { get; set; }

        public static FinishedTest[] CreateFromDb(Database.User_Test[] dbTest)
        {
            return dbTest.Select(x =>
            {
                var test = new FinishedTest
                {
                    PassingStamp = x.PassingStamp,
                    Id = x.Test.Id,
                    Answer = x.Test.Answer,
                    Question = x.Test.Question,
                    Responses = (List<Response>) JsonConvert.DeserializeObject(x.Test.Responses, typeof(List<Response>)),
                    Title = x.Test.Title
                };
                if (x.Result.HasValue) test.Result = x.Result.Value;

                return test;

            }).ToArray();
        }
    }
}