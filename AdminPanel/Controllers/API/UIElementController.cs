using ModelData.Models;
using ModelData.Models.Database;
using StorageAPI.Models.Database;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace AdminPanel.Controllers.API
{
    public class UIElementController : ApiController
    {
        [HttpPost]
        public async Task<IEnumerable<UIElementModel>> Get(DataComModel model)
        {
            using (var context = new TourAgencyContext())
            {
                return await context.UIElements
                    .AsNoTracking()
                    .Where(r => r.Language == model.Language)
                    .ToListAsync();
            }
        }
    }
}