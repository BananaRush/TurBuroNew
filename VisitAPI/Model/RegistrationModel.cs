using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitAPI.Model
{
    public class RegistrationModel
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password1")]
        public string Password1 { get; set; }

        [JsonProperty("password2")]
        public string Password2 { get; set; }

        [JsonProperty("tos")]
        public string Conditions { get; set; }
    }
}
