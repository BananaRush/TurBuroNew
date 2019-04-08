using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using Microsoft.Ajax.Utilities;
using StorageAPI.Models;
using StorageAPI.Models.Database;
using User = StorageAPI.Models.User;

namespace StorageAPI.Controllers
{
    public class UserController : ApiController
    {
        // GET api/<controller>/5
        [HttpGet]
        public User Auth([FromUri]string login, [FromUri]string password)
        {
            using (var context = new TourAgencyContext())
            {
                var hash = Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(password)));
                var dbUser = context.User.FirstOrDefault(x => x.Login == login && x.Password == hash);

                if (dbUser != null)
                {
                    return StorageAPI.Models.User.CreateFromDb(dbUser);
                }
            }

            return null;
        }

        [HttpGet]
        public ExtendedUser Get([FromUri]long id)
        {
            using (var context = new TourAgencyContext())
            {
                var dbUser = context.User.Find(id);

                if (dbUser != null)
                {
                    return StorageAPI.Models.ExtendedUser.CreateFromDbWithTests(dbUser);
                }
            }

            return null;
        }

        [HttpPost]
        // POST api/<controller>
        public long Post([FromBody]ExtendedUser value)
        {
            using (var context = new TourAgencyContext())
            {
                if (context.User.Any(x => x.Login == value.Login))
                {
                    return -1;
                }

                StorageAPI.Models.Database.User user = new Models.Database.User
                {
                    Login = value.Login,
                    Name = value.Name,
                    Password = value.PasswordHash
                };

                context.User.Add(user);
                context.SaveChanges();

                return user.Id;
            }
        }

        // PUT api/<controller>/5
        [HttpPut]
        public long Put([FromUri]long id, [FromBody]ExtendedUser value)
        {
            using (var context = new TourAgencyContext())
            {
                var dbUser = context.User.Find(id);

                if (dbUser == null)
                {
                    return -1;
                }

                dbUser.Name = value.Name;
                dbUser.Password = value.PasswordHash;

                context.SaveChanges();
                return dbUser.Id;
            }
        }

        [HttpPut]
        public long TestResultAdd([FromBody]AccomplishedTestParams value)
        {
            using (var context = new TourAgencyContext())
            {
                if (!context.User.Any(x => x.Id == value.UserId) && !context.Test.Any(x => x.Id == value.TestId))
                {
                    return -1;
                }

                var testRecord = new User_Test
                {
                    UserId = value.UserId,
                    TestId = value.TestId,
                    Result = value.Result,
                    PassingStamp = DateTime.Now
                };

                context.User_Test.Add(testRecord);
                context.SaveChanges();

                return testRecord.UserId;
            }
        }

        // DELETE api/<controller>/5
        public void Delete(long id)
        {
            using (var context = new TourAgencyContext())
            {
                context.User.Find(id).IfNotNull( x => context.User.Remove(x));
                context.SaveChanges();
            }
        }
    }
}