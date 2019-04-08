using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StorageAPI.Models;
using StorageAPI.Models.Database;
//using Microsoft.Ajax.Utilities;
using User = StorageAPI.Models.User;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdminPanelTurBuro.Controllers.API
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly TourAgencyContext _context;

        public UserController(TourAgencyContext context)
        {
            _context = context;
        }

        // GET api/<controller>/5
        [HttpGet]
        public User Auth(string login, string password)
        {

            var hash = Convert.ToBase64String(MD5.Create().ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));
            var dbUser = _context.User.FirstOrDefault(x => x.Login == login && x.Password == hash);

            if (dbUser != null)
            {
                return StorageAPI.Models.User.CreateFromDb(dbUser);
            }
            return null;
        }

        [HttpGet]
        public ExtendedUser Get(long id)
        {
            var dbUser = _context.User.Find(id);

            if (dbUser != null)
            {
                return StorageAPI.Models.ExtendedUser.CreateFromDbWithTests(dbUser);
            }

            return null;
        }

        [HttpPost]
        // POST api/<controller>
        public long Post([FromBody]ExtendedUser value)
        {
            if (_context.User.Any(x => x.Login == value.Login))
            {
                return -1;
            }

            StorageAPI.Models.Database.User user = new StorageAPI.Models.Database.User
            {
                Login = value.Login,
                Name = value.Name,
                Password = value.PasswordHash
            };

            _context.User.Add(user);
            _context.SaveChanges();

            return user.Id;    
        }

        // PUT api/<controller>/5
        [HttpPut]
        public long Put(long id, [FromBody]ExtendedUser value)
        {
            var dbUser = _context.User.Find(id);

            if (dbUser == null)
            {
                return -1;
            }

            dbUser.Name = value.Name;
            dbUser.Password = value.PasswordHash;

            _context.SaveChanges();
            return dbUser.Id;
        }

        [HttpPut]
        public long TestResultAdd([FromBody]AccomplishedTestParams value)
        {
            if (!_context.User.Any(x => x.Id == value.UserId) && !_context.Test.Any(x => x.Id == value.TestId))
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

            _context.User_Test.Add(testRecord);
            _context.SaveChanges();

            return testRecord.UserId;
            
        }

        // DELETE api/<controller>/5
        public void Delete(long id)
        {
            //context.User.Find(id).IfNotNull(x => context.User.Remove(x));
            StorageAPI.Models.Database.User elm = _context.User.Find(id);

            if(elm != null)
            {
                _context.User.Remove(elm);
            }

            _context.SaveChanges(); 
        }
    }
}
