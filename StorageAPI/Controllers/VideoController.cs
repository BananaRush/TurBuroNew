using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using StorageAPI.Models.Database;

namespace StorageAPI.Controllers
{
    public class VideoController : ApiController
    {
        public string GetFirstBisy()
        {
            using (var context = new TourAgencyDataBase())
            {
                try
                {

               return context.VideoConnects.FirstOrDefault(f => !f.IsBusy)?.Name;

                }
                catch
                {
                    return null;
                }
            }
        }

       [HttpGet]
        public async Task<bool> MakeConnect(string name)
        {
            using (var context = new TourAgencyDataBase())
            {
                try
                {
                    var admin = context.VideoConnects.FirstOrDefault(f => f.Name == name);
                    if (admin == null) return false;
                    admin.IsBusy = true;
                   await context.SaveChangesAsync();
                    return true;
                }
                catch
                {
                    return false;
                }
                
            }
        }

        [HttpGet]
        public async Task CallEnd(string name)
        {
            using (var context = new TourAgencyDataBase())
            {
                try
                {
                    var admin = context.VideoConnects.FirstOrDefault(f => f.Name == name);
                    if (admin == null) return;
                    admin.IsBusy = false;
                    await context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                  
                }
            }
        }

        [HttpGet]
        public async Task<bool> AddNewAdmin(string name)
        {
            using (var context = new TourAgencyDataBase())
            {
                try
                {
                    VideoConnect admin = new VideoConnect();
                    admin.Name = name;
                    admin.IsBusy = false;
                    context.VideoConnects.Add(admin);
                    await context.SaveChangesAsync();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        [HttpGet]
        public async Task AdminLeave(string name)
        {
            using (var context = new TourAgencyDataBase())
            {
                try
                {
                    var admin = context.VideoConnects.FirstOrDefault(f => f.Name == name);
                    if(admin ==null) return;
                    context.VideoConnects.Remove(admin);
                    await context.SaveChangesAsync();
                }
                catch
                {
                }
            }
        }
    }
}