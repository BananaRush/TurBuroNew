using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace StorageAPI.Models
{
    public class User
    {
        public long? Id { get; set; }
        public string Login { get; set; }
        internal string PasswordHash { get; set; }
        public string Name { get; set; }
        public FinishedTest[] FinishedTests { get; set; }

        public static User CreateFromDb(Database.User dbUser)
        {
            var user = new User
            {
                Id = dbUser.Id,
                Login = dbUser.Login,
                Name = dbUser.Name,
                FinishedTests = new FinishedTest[0],
                PasswordHash = dbUser.Password
            };
            return user;
        }
    }
}