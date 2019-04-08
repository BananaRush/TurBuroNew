using ModelData.Model.Database;
using ModelData.Models.Database;
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
    public class InfoController : ApiController
    {
        [HttpGet]
        public async Task<Information> Get(int id)
        {
            using (var context = new TourAgencyContext())
            {
                return await context.Information.FirstOrDefaultAsync(r => r.ButtonId == id &&
                                    string.IsNullOrEmpty(r.Cat) &&
                                    r.IsPublick);
            }
        }

        [HttpGet]
        public async Task<Information> GetId(int id)
        {
            using (var context = new TourAgencyContext())
            {
                return await context.Information.FirstOrDefaultAsync(r=>r.Id == id);
            }
        }

        [HttpGet]
        public async Task<List<Information>> GetIdSection(int id)
        {
            using (var context = new TourAgencyContext())
            {
                try
                {
                    Section elm = await context.Sections.Include(r => r.Disciples).FirstOrDefaultAsync(r => r.Id == id);

                    if (elm != null)
                    {
                        return elm.Disciples.Where(r => r.IsPublick == true).Select(z=> new Information()
                        {
                            Id = z.Id,
                            DateTime = z.DateTime,
                            SectionsId = z.SectionsId,
                            Info = z.Info,
                            Title = z.Title
                        }).ToList();
                    }
                }
                catch(Exception el)
                {
 
                }

                return null;
            }
        }

        [HttpGet]
        public async Task<List<Information>> GetLents(int id)
        {
            try
            {
                using (var context = new TourAgencyContext())
                {
                    return await context.Information.Where(r => r.ButtonId == id && 
                                                            r.Cat == "ListModel" && 
                                                            r.IsPublick == true).ToListAsync();
                }
            }
            catch
            {
                return null;
            }
        }
    }
}