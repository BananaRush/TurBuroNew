using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using StorageAPI.Models;

namespace TourAgency.Model.ModelWebApi
{
    public class FinishedTest : Test
    {
        public double Result { get; set; }
        public DateTime PassingStamp { get; set; }

    }
}