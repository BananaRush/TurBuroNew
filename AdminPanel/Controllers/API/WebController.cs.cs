using ModelData.Model.Database;
using StorageAPI.Models.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace AdminPanel.Controllers.API
{
    public class WebBrowserController : ApiController
    { 
        [HttpGet]
        public async Task<UrlInfo> Get(int id)
        {
            using (var context = new TourAgencyContext())
            {
                return await context.UrlInfos.Include(r=>r.UrlListInfos).FirstOrDefaultAsync(r=>r.ButtonId == id);
            }
        }
    }
}