using Microsoft.AspNetCore.Mvc;
using StorageAPI.Models;
using StorageAPI.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPanelTurBuro.Components
{
    public class MenuLeft : ViewComponent
    {
        private readonly TourAgencyContext _context;

        public MenuLeft(TourAgencyContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            IEnumerable<News> list = _context.NewsModel.ToList().Select(News.CreateFromDb);

            return View(list);
        }
    }
}
