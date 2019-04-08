using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanelTurBuro.Controllers
{
    public class EditorController : Controller
    {
        private readonly TourAgencyContext _context;

        public EditorController(TourAgencyContext context)
        {
            _context = context;
        }

        // GET: Editor
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddInformation()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddButton(NewsModel model, string action)
        {
            long Id = 0;
            NewsModel elm = null;

            if (action == "Add")
            {
                _context.NewsModel.Add(new NewsModel() { Text = "Пункт", IconUri = "" });
            }

            if (action == "Save")
            {
                if(model != null)
                {
                    elm = await _context.NewsModel.FindAsync(model.Id);
                    if(elm != null)
                    {
                        Id = elm.Id;
                        elm.Text = model.Text;
                        elm.ContentType = model.ContentType;
                    }
                }
            }

            if (action == "Delete")
            {
                if(model != null)
                {
                    elm = await _context.NewsModel.FindAsync(model.Id);
                    if (elm != null)
                        _context.NewsModel.Remove(elm);
                }
            }

            await _context.SaveChangesAsync();

            if(Id == 0)
            {
                elm = _context.NewsModel.LastOrDefault();
                if(elm != null)
                    Id = elm.Id;
            }

            return RedirectToAction("MenuEditor", "Home", new { @id = Id });
        }

        // Кнопки

        public IActionResult AddImage(int? id, int? idContent)
        {
            return View();
        }

        public IActionResult AddNewLents(int? id, int? idContent)
        {
            return View();
        }

        public async Task<IActionResult> AddPage(int? id, int? idContent)
        {
            Tuple<Information, int> model = null;
            Information elm = null;

            if (id != null && idContent != null)
            {
                // Редактирование в кнопке
                elm = _context.Information.Where(r => r.Id == idContent).FirstOrDefault();
                model = new Tuple<Information, int>(elm, id.Value);
            }
            
            if(id != null && idContent == null)
            {
                elm = await _context.Information.FindAsync(id);
                model = new Tuple<Information, int>(elm, id.Value);
                // Редактирование без кнопки
            }

            if (id == null && idContent == null)
            {
                // Простое добавление
            }


            return View("AddInformation", model);
        }

        public IActionResult AddPdf(int? id, int? idContent)
        {
            return View();
        }

        public IActionResult AddPresentation(int? id, int? idContent)
        {
            return View();
        }

        public IActionResult AddSection(int? id, int? idContent)
        {
            return View();
        }

        public IActionResult AddUrl(int? id, int? idContent)
        {
            return View();
        }

        public IActionResult AddVideo(int? id, int? idContent)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddInfo(Information model, long IdBtn)
        {
            int? _id = null;
            long? _idBtn = null;

            if (model.Id == 0)
            {
                model.DateTime = DateTime.Now;
                _context.Information.Add(model);
                await _context.SaveChangesAsync();
                Information elm = _context.Information.LastOrDefault();
               
                if (elm != null && IdBtn != 0)
                {
                    _id = elm.Id;
                    NewsModel elm1 = await _context.NewsModel.FindAsync(IdBtn);
                    if(elm1 != null)
                    {
                        elm1.IdContent = elm.Id;
                        _idBtn = elm1.Id;
                    }
                }
            }
            else
            {
                Information old = await _context.Information.FindAsync(model.Id);
                if(old != null)
                {
                    old.Title = model.Title;
                    old.Info = model.Info;
                    old.IsPublick = model.IsPublick;
                    old.DateTime = DateTime.Now;
                    _id = model.Id;

                    if(IdBtn != 0)
                    {
                        _idBtn = IdBtn;
                    }
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("AddPage", new { id = _idBtn, idContent = _id});
        }

        [HttpPost]
        public async Task<IActionResult> InfoPublic(int id)
        {
            Information elm = await _context.Information.FindAsync(id);

            if(elm != null)
            {
                elm.IsPublick = !elm.IsPublick;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("AddPage", new { @id = id });
        }
    }
}