using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace StorageAPI.Models
{
    public class ExtendedUser : User
    {
        private string _password;

        public string Password
        {
            get { return _password; }
            set
            {
                PasswordHash = Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(value)));
                _password = value;
            }
        }

        public static ExtendedUser CreateFromDbWithTests(Database.User dbUser)
        {
            var user = new ExtendedUser()
            {
                Id = dbUser.Id,
                Login = dbUser.Login,
                Name = dbUser.Name,
                FinishedTests = FinishedTest.CreateFromDb(dbUser.User_Test.ToArray())//no passwords but in the case use _password = dbUser.Password
            };
            return user;
        }
    }
}