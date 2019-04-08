using Ninject.Modules;
using StorageAPI.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminPanel.Utilit
{
    public class NinjectRegistrations : NinjectModule
    {
        public override void Load()
        {
            Bind<TourAgencyContext>().To<TourAgencyContext>();
        }
    }
}