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
    public class SectionController : ApiController
    {
        [HttpGet]
        public async Task<List<Section>> Get(int id)
        {
            List<Section> list = null;

            using (var context = new TourAgencyContext())
            {
                list = await context.Sections.Include(r=>r.Children).Where(r => r.ButtonId == id).ToListAsync();

                for(int i = 0; i < list.Count; i++)
                {
                    list[i].Disciples = null;
                    LoadSection(list[i].Children);
                }
            }

            return list;
        }

        // Загружаем модель
        private void LoadSection(List<Section> section)
        {
            for (int i = 0; i < section.Count(); i++)
            {
                section[i].Disciples = null;
                LoadSection(section[i].Children);
            }
        }
    }
}