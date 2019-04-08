//Auctor Chirin Alexander Olegovoch - 2018
using AdminPanel.App_Start;
using ModelData.Model.Database;
using ModelData.Models.Database;
using StorageAPI.Models;
using StorageAPI.Models.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{

    public class HomeController : Controller
    {
        private readonly TourAgencyContext _context;

        public HomeController(TourAgencyContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MenuEditor(int? Id)
        {
            Tuple<IEnumerable<News>, News> model = null;
            IEnumerable<News> list = _context.NewsModel
                .Where(DataConfig.SortData)
                .ToList()
                .Select(News.CreateFromDb);

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

        public ActionResult InformationEdit()
        {
            List<Information> list = _context.Information.ToList();

            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Info != null && list[i].Info.Length > 250)
                    {
                        list[i].Info = list[i].Info.Substring(0, 249);
                        list[i].Info += "...";
                    }
                }
            }

            return View(list);
        }

        public ActionResult PresentationEditor()
        {
            return View();
        }

        public ActionResult BrowserEditor()
        {
            return View();
        }

        public ActionResult LeftMenu ()
		{

            IEnumerable<NewsModel> list = _context.NewsModel
                .Include(r => r.ListTerminal)
                .ToList();

            IEnumerable<News> model = list
                .Where(DataConfig.SortData)
                .ToList()
                .Select(News.CreateFromDb);

            return PartialView(model);
        }

        public ActionResult CreateAlbumImage()
        {
            return PartialView();
        }

        public async Task<ActionResult> DeleteInfo(int id)
        {
            Information information = await _context.Information.FirstOrDefaultAsync(r=>r.Id == id);

            if(information != null)
            {
                _context.Information.Remove(information);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("InformationEdit");
        }

        [HttpPost]
        public async Task<ActionResult> PublickInfo(int id)
        {
            Information elm = await _context.Information.FindAsync(id);
            int index = id;

            if (elm != null)
            {
                elm.IsPublick = !elm.IsPublick;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("InformationEdit");
        }

        public async Task<ActionResult> CreateSurvey()
        {
            _context.Survey.Add(new SurveyModel() { Language = DataConfig.Language.CodeLang });
            await _context.SaveChangesAsync();

            return RedirectToAction("Survey");
        }

        public async Task<ActionResult> Survey()
        {
            List<SurveyModel> list = await _context.Survey
                .Include(r=>r.ListOption)
                .Where(r=>r.Language == DataConfig.Language.CodeLang)
                .ToListAsync();

            return View(list);
        }

        [HttpPost]
        public async Task<ActionResult> SaveSurvey(SurveyModel model)
        {
            SurveyModel old = await _context.Survey.Include(r=>r.ListOption).FirstOrDefaultAsync(r=>r.Id == model.Id);

            if(old != null)
            {
                model.Language = DataConfig.Language.CodeLang;
                _context.Entry(old).CurrentValues.SetValues(model);
                old.ListOption.Clear();

                if(model.ListOption != null)
                    old.ListOption.AddRange(model.ListOption);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Survey");
        }

        public async Task<ActionResult> DeleteSurvey(int Id)
        {
            SurveyModel elm = await _context.Survey.FirstOrDefaultAsync(r=>r.Id == Id);

            if(elm != null)
            {
                _context.Survey.Remove(elm);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Survey");
        }

        public ActionResult TypeFilling()
        {
            Tuple<List<LanguageModel>, List<TerminalsModel>> tuple = null;
            List<LanguageModel> langList = _context.LanguageModels.ToList();
            List<TerminalsModel> terminalsModels = _context.TerminalsModels.Where(r => r.IsAutorizate == true).ToList();
            tuple = new Tuple<List<LanguageModel>, List<TerminalsModel>>(langList, terminalsModels);
            DataConfig.Language = langList.FirstOrDefault(r=>r.IsActive);
            DataConfig.Terminals = terminalsModels.Where(r => r.IsSelect == true).ToList();
            return View(tuple);
        }

        public async Task<ActionResult> SetLang(int Id)
        {
            List<LanguageModel> language = await _context.LanguageModels.ToListAsync();

            if(language != null)
            {
                LanguageModel elm = language.FirstOrDefault(r=>r.Id == Id);
                if(elm != null)
                {
                    for (int i = 0; i < language.Count; i++)
                    {
                        language[i].IsActive = false;
                    }

                    elm.IsActive = true;

                    DataConfig.Language = elm;

                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> SetTerminal(List<TerminalsModel> Item2)
        {
            List<TerminalsModel> list = await _context.TerminalsModels.ToListAsync();
            if(list != null)
            {
                for(int i = 0; i < list.Count; i++)
                {
                    try
                    {
                        list[i].IsSelect = Item2.FirstOrDefault(r => r.Id == list[i].Id).IsSelect;

                    }
                    catch(Exception el)
                    {

                    }
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}