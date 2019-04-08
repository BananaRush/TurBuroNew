using System;

namespace TourAgency.Model.ModelWebApi
{
    public partial class User_Test
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public long TestId { get; set; }

        public float? Result { get; set; }

        public DateTime PassingStamp { get; set; }

        //public virtual Test Test { get; set; }

        //public virtual User User { get; set; }
    }
}
