using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using ModelData.Models;
using ModelData.Models.Database;
using StorageAPI.Models.Database;

namespace AdminPanel.Controllers.API
{
    public class SurveyController : ApiController
    {
        [HttpPost]
        public async Task<IEnumerable<SurveyModel>> Get(DataComModel model)
        {
            using (var context = new TourAgencyContext())
            {
                return await context.Survey
                    .AsNoTracking()
                    .Include(r=>r.ListOption)
                    .Where(r => r.Language == model.Language)
                    .ToListAsync();
            }
        }

        [HttpGet]
        public async Task<bool> OptionCout(int id)
        {
            try
            {
                using (var context = new TourAgencyContext())
                {
                    SurveyValueModel elm = await context.SurveyValue.FirstOrDefaultAsync(r => r.Id == id);
                    if (elm != null)
                    {
                        elm.OptionCout = ++elm.OptionCout;
                        await context.SaveChangesAsync();
                    }
                }

                return true;
            }
            catch(Exception el)
            {
                return false;
            }
        }
    }
}