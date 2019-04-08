using StorageAPI.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace TourAgency.Model.ModelWebApi
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

       
    }
}