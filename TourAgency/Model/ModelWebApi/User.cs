using System.Collections.Generic;

namespace TourAgency.Model.ModelWebApi
{
    public partial class User
    {
        public long? Id { get; set; }
        public string Login { get; set; }
        internal string PasswordHash { get; set; }
        public string Name { get; set; }
        public FinishedTest[] FinishedTests { get; set; }
    }
}
