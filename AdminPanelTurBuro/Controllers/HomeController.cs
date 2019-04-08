using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdminPanelTurBuro.Models;
using StorageAPI.Models.Database;
using StorageAPI.Models;
using ModelData.Model.Database;
using System.Data.Entity;

namespace AdminPanelTurBuro.Controllers
{
    public class HomeController : Controller
    {
        private readonly TourAgencyContext _context;

        public HomeController(TourAgencyContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MenuEditor(int? Id)
        {
            Tuple<IEnumerable<News>, News> model = null;
            IEnumerable<News> list = _context.NewsModel.ToList().Select(News.CreateFromDb);
            News elm = null;

            if (Id != null)
            {
                elm = list.Where(w => w.Id == Id).FirstOrDefault();
            }
            else
            {
                elm = list.FirstOrDefault();
            }

            model = new Tuple<IEnumerable<News>, News>(list, elm);

            return View(model);
        }

        public IActionResult InformationEdit()
        {
            List<Information> list = _context.Information.ToList();

            if(list != null)
            {
                for(int i = 0; i < list.Count; i++)
                {
                    if(list[i].Info.Length > 250)
                    {
                        list[i].Info = list[i].Info.Substring(0, 249);
                        list[i].Info += "...";
                    }
                }
            }

            return View(list);
        }

        public IActionResult PresentationEditor()
        {
            return View();
        }

        public IActionResult BrowserEditor()
        {
            return View();
        }

        //

        //public IActionResult PageBtn(int id, int idContext)
        //{
        //    return RedirectToAction("AddPage", "Editor", new { @Id = id, @IdContext = idContext });
        //}

        //public IActionResult UriBtn(int id, int idContext)
        //{
        //    return RedirectToAction("AddUrl", "Editor", new { @Id = id, @IdContext = idContext });
        //}

        //public IActionResult PdfBtn(int id, int idContext)
        //{
        //    return RedirectToAction("AddPdf", "Editor", new { @Id = id, @IdContext = idContext });
        //}

        //public IActionResult PresentationBtn(int id, int idContext)
        //{
        //    return RedirectToAction("AddPresentation", "Editor", new { @Id = id, @IdContext = idContext });
        //}

        //public IActionResult ImageBtn(int id, int idContext)
        //{
        //    return RedirectToAction("AddImage", "Editor", new { @Id = id, @IdContext = idContext });
        //}

        //public IActionResult SectionBtn(int id, int idContext)
        //{
        //    return RedirectToAction("AddSection", "Editor", new { @Id = id, @IdContext = idContext });
        //}

        //public IActionResult NewLentsBtn(int id, int idContext)
        //{
        //    return RedirectToAction("AddNewLents", "Editor", new { @Id = id, @IdContext = idContext });
        //}

        //public IActionResult VideoBtn(int id, int idContext)
        //{
        //    return RedirectToAction("AddVideo", "Editor", new { @Id = id, @IdContext = idContext });
        //}

    }
}
