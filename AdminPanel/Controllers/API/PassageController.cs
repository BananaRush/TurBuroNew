using ModelData.Model.Database;
using StorageAPI.Models.Database;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace AdminPanel.Controllers.API
{
    public class PassageController : ApiController
    {
        [HttpGet]
        public async Task<List<PassageImage>> Get(int id)
        {
            using (var context = new TourAgencyContext())
            {
                return await context.PassageImages
                    .AsNoTracking()
                    .Include(r=>r.imageLists)
                    .Where(r=>r.ButtonId == id)
                    .ToListAsync();
            }
        }
    }
}