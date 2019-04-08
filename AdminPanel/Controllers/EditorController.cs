//Auctor Chirin Alexander Olegovoch - 2018
using ModelData.Model.Database;
using StorageAPI.Models.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Linq;
using StorageAPI.Models;
using ModelData.Models.Database;
using ModelData.Utilits;
using System.IO;
using AdminPanel.App_Start;
using ModelData.Interface;

namespace AdminPanel.Controllers
{
    public class EditorController : Controller
    {
        private readonly TourAgencyContext _context;
        int LengthInfo = 200;

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
                _context.NewsModel.Add(new NewsModel()
                {
                    Text = "Пункт",
                    IconUri = "",
                    Language = DataConfig.Language.CodeLang
                });
            }

            if (action == "Save")
            {
                if (model != null)
                {
                    elm = await _context.NewsModel.FindAsync(model.Id);
                    if (elm != null)
                    {
                        Id = elm.Id;
                        elm.Text = model.Text;
                        elm.ContentType = model.ContentType;
                    }
                }
            }

            if (action == "Delete")
            {
                if (model != null)
                {
                    elm = await _context.NewsModel.FindAsync(model.Id);
                    if (elm != null)
                        _context.NewsModel.Remove(elm);
                }
            }

            await _context.SaveChangesAsync();

            if (Id == 0)
            {
                elm = _context.NewsModel.ToList().LastOrDefault();
                if (elm != null)
                    Id = elm.Id;
            }

            return RedirectToAction("MenuEditor", "Home", new { @id = Id });
        }

        // Кнопки
        public async Task<ActionResult> AddImage(int id)
        {
            Tuple<List<PassageImage>, int?> model = null;
            List<PassageImage> list = null;

            if(id != null)
            {
                list = await _context.PassageImages.Include(r=>r.imageLists).Where(r => r.ButtonId == id).ToListAsync();
            }

            model = new Tuple<List<PassageImage>, int?>(list, id);

            return View(model);
        }

        public async Task<ActionResult> AddNewLents(int id)
        {
            Tuple<List<Information>, int> model = null;
            List<Information> list = null;

            list = await _context.Information.Where(r=>r.ButtonId == id && r.Cat == "ListModel").ToListAsync();
            if(list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].Info = HtmlClear.ClearHtml(list[i].Info);
                    if (!string.IsNullOrEmpty(list[i].Info) && list[i].Info.Length > LengthInfo)
                    {
                        list[i].Info = list[i].Info.Substring(0, LengthInfo) + "...";
                    }
                }
            }

            model = new Tuple<List<Information>, int>(list, id);

            return View(model);
        }

        public async Task<ActionResult> AddInfoNewLent(int? id, int IdBtn)
        {
            Tuple<Information, int> model = null;
            Information information = null;

            if(id.HasValue)
            {
                information = await _context.Information.FirstOrDefaultAsync(r=>r.Id == id);
            }

            model = new Tuple<Information, int>(information, IdBtn);

            return View(model);
        }

        public async Task<ActionResult> AddPage(int? id)
        {
            Tuple<Information, int?> model = null;
            Information elm = null;
            if (id != null)
            {
                // Редактирование в кнопке
                elm = await _context.Information.FirstOrDefaultAsync(r=>r.ButtonId == id && string.IsNullOrEmpty(r.Cat));
            }

            model = new Tuple<Information, int?>(elm, id);

            return View("AddInformation", model);
        }

        public ActionResult AddPdf(int? id, int? idContent)
        {
            return View();
        }

        public ActionResult AddPresentation(int? id, int? idContent)
        {
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> AddSection(int? id)
        {
            Tuple<List<Section>, int?> model = null;
            List<Section> list = null;

            if (id != null)
            {
                list = await _context.Sections.Where(r => r.ButtonId == id).Include(r => r.Children).ToListAsync();
            }

            model = new Tuple<List<Section>, int?>(list, id);

            return View(model);
        }

        public async Task<ActionResult> AddUrl(int? id)
        {
            Tuple<UrlInfo, int?> model = null;
            UrlInfo elm = null;
            if (id != null)
            {
                elm = await _context.UrlInfos.Include(r=>r.UrlListInfos).FirstOrDefaultAsync(r=>r.ButtonId == id);
            }

            model = new Tuple<UrlInfo, int?>(elm, id);

            return View(model);
        }

        public async Task<ActionResult> AddVideo(int id)
        {
            Tuple<VideoModel, List<string>, int> model = null;
            VideoModel elm = null;
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "Video");
            List<string> files = Directory.GetFiles(path, "*", SearchOption.AllDirectories).ToList();

            elm = await _context.VideoModels.FirstOrDefaultAsync(r => r.ButtonId == id);
            model = new Tuple<VideoModel, List<string>, int>(elm, files, id);

            return View(model);
        }

        public async Task<ActionResult> AddInformationGeneral(int? id)
        {
            Tuple<Information, int?> model = null;
            Information elm = null;

            if(id != null)
            {
                elm = await _context.Information.FirstOrDefaultAsync(r=>r.Id == id);
            }

            model = new Tuple<Information, int?>(elm, id);

            return View(model);
        }

        [HttpPost, ValidateInput(false)]
        public async Task<ActionResult> AddInfoGeneral(Information model)
        {
            model.DateTime = DateTime.Now;
            int index = 0;

            if (model.Id == 0)
            {
                _context.Information.Add(model);
                await _context.SaveChangesAsync();
                index =  _context.Information.ToList().Last().Id;
            }
            else
            {
                index = model.Id;
                Information old = await _context.Information.FirstOrDefaultAsync(r => r.Id == model.Id);
                if (old != null)
                {
                    old.Title = model.Title;
                    old.Info = model.Info;
                    old.DateTime = model.DateTime;
                    //_context.Entry(old).CurrentValues.SetValues(model);
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("AddInformationGeneral", new { id = index });
        }

        [HttpPost, ValidateInput(false)]
        public async Task<ActionResult> AddInfo(Information model)
        {
            model.DateTime = DateTime.Now;
            if (model.Id == 0)
            {
                if (model.ButtonId.HasValue)
                {
                    NewsModel btn = await _context.NewsModel.FindAsync(model.ButtonId.Value);
                    btn?.ListInformation.Add(model);
                }
                else
                {
                    _context.Information.Add(model);
                }
            }
            else
            {
                Information old = await _context.Information.FirstOrDefaultAsync(r=>r.Id == model.Id);
                if(old != null)
                {
                    _context.Entry(old).CurrentValues.SetValues(model);
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("AddPage", new { id = model.ButtonId });
        }

        [HttpPost]
        public async Task<ActionResult> SetUrl(UrlInfo model)
        {
            if (model.Id == 0)
            {
                if (model.ButtonId.HasValue)
                {
                    NewsModel btn = await _context.NewsModel.FindAsync(model.ButtonId.Value);
                    btn?.ListUrlInfo.Add(model);
                }
                else
                {
                    _context.UrlInfos.Add(model);
                }
            }
            else
            {
                UrlInfo old = await _context.UrlInfos.Include(r=>r.UrlListInfos).FirstOrDefaultAsync(r => r.Id == model.Id);

                if (old != null)
                {
                    _context.Entry(old).CurrentValues.SetValues(model);
                    old.UrlListInfos.Clear();
                    old.UrlListInfos.AddRange(model.UrlListInfos);
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("AddUrl", new { id = model.ButtonId });
        }

        [HttpPost]
        public async Task<ActionResult> CreateAlbom(string Name, int Id)
        {
            NewsModel btn = await _context.NewsModel.FirstOrDefaultAsync(r=>r.Id == Id);
            
            if(btn != null)
            {
                btn.ListPassageImages.Add(new PassageImage()
                {
                    Title = Name
                });

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("AddImage", new { id = Id });
        }

        [HttpPost]
        public async Task<ActionResult> InfoPublicGeneral(int id)
        {
            Information elm = await _context.Information.FindAsync(id);
            int index = id;

            if (elm != null)
            {
                elm.IsPublick = !elm.IsPublick;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("AddInformationGeneral", new { @id = index });
        }

        [HttpPost]
        public async Task<ActionResult> InfoPublic(int id)
        {
            Information elm = await _context.Information.FindAsync(id);
            int index = id;

            if (elm != null)
            {
                elm.IsPublick = !elm.IsPublick;
                await _context.SaveChangesAsync();

                if(elm.ButtonId.HasValue)
                {
                    index = elm.ButtonId.Value;
                }
            }

            return RedirectToAction("AddPage", new { @id = index });
        }

        public async Task<ActionResult> DeleteAlbom(int Id)
        {
            PassageImage passageImage = await _context.PassageImages.Include(r=>r.imageLists).FirstOrDefaultAsync(r=>r.Id == Id);
            int index = 0;

            if(passageImage != null)
            {
                string path = string.Empty;

                for (int i = 0; i < passageImage.imageLists.Count; i++)
                {
                    path = Server.MapPath("~/Files/Image/" + passageImage.imageLists[i].ImgUrl);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }

                index = passageImage.ButtonId.Value;
                _context.PassageImages.Remove(passageImage);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("AddImage", new { id = index });
        }

        public async Task<ActionResult> DeleteImage(int id, params int[] idimg)
        {
            PassageImage passageImage = await _context.PassageImages.FirstOrDefaultAsync(r => r.Id == id);
            int index = 0;
            string path = string.Empty;

            if (passageImage != null)
            {
                index = passageImage.ButtonId.Value;
            }

            List<ImageList> listImg = new List<ImageList>();

            foreach(var item in idimg)
            {
                ImageList image = await _context.ImageLists.FirstOrDefaultAsync(r=>r.Id == item);
                if(image != null)
                {
                    listImg.Add(image);
                }
            }

            _context.ImageLists.RemoveRange(listImg);

            for (int i = 0; i < listImg.Count; i++)
            {
                path = Server.MapPath("~/Files/Image/" + listImg[i].ImgUrl);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("AddImage", new { id = index });
        }

        public async Task<ActionResult> SetImage(int Id, HttpPostedFileBase image = null)
        {
            PassageImage passageImage = await _context.PassageImages.FirstOrDefaultAsync(r => r.Id == Id);
            int index = 0;

            if(passageImage != null)
            {
                index = passageImage.ButtonId.Value;

                if(image != null)
                {
                    // получаем имя файла
                    string fileName = System.IO.Path.GetFileName(image.FileName);
                    // сохраняем файл в папку Files в проекте
                    image.SaveAs(Server.MapPath("~/Files/Image/" + fileName));

                    passageImage.imageLists.Add(new ImageList()
                    {
                        ImgUrl = fileName
                    });

                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("AddImage", new { id = index });
        }

        public async Task<ActionResult> SetEditSection(int IdBtn, int Id, string Name)
        {
            Section section = await _context.Sections.FirstOrDefaultAsync(r=>r.Id == Id);
            if(section != null)
            {
                section.Header = Name;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("AddSection", new { id = IdBtn });
        }

        [HttpPost]
        public async Task<ActionResult> SetSection(int IdBtn, int Id, string Name)
        {
            NewsModel news = await _context.NewsModel.FirstOrDefaultAsync(r=>r.Id == IdBtn);

            if(Id == 0)
            {
                if(news != null)
                {
                    news.ListSections.Add(new Section()
                    {
                         Header = Name
                    });
                }
            }
            else
            {
                Section section = await _context.Sections.FirstOrDefaultAsync(r=>r.Id == Id);
                if(section != null)
                {
                    section.Children.Add(new Section()
                    {
                        Header = Name
                    });
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("AddSection", new { id = IdBtn });
        }

        public async Task<ActionResult> DeleteSection(int IdBtn, int Id)
        {
            Section section = await _context.Sections.FirstOrDefaultAsync(r=>r.Id == Id);

            if(section != null)
            {
                DeleteSections(section);
                _context.Sections.Remove(section);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("AddSection", new { id = IdBtn });
        }

        private void DeleteSections(Section section)
        {
            for (int i = 0; i < section.Children.Count; i++)
            {
                DeleteSections(section.Children[i]);
                _context.Sections.Remove(section.Children[i]);
            }
        }

        public async Task<ActionResult> AddInfoSection(int id)
        {
            Tuple<Information, List<Section>, int> tuple = null;
            List<Section> list = await _context.Sections
                .Where(r => r.ButtonId == id)
                .Include(r => r.Children)
                .ToListAsync();

            tuple = new Tuple<Information, List<Section>, int>(null, list, id);

            return View(tuple);
        }

        public async Task<ActionResult> ListSectionInfo(int id)
        {
            Tuple<List<Information>, List<Section>, int> model = null;
            List<Information> infolost = null;
            NewsModel news = await _context.NewsModel.Include(r => r.ListSections).FirstOrDefaultAsync(r => r.Id == id);

            if (news != null)
            {
                infolost = new List<Information>();
                for (int i = 0; i < news.ListSections.Count; i++)
                {
                    LoadInformation(infolost, news.ListSections[i]);
                    if(news.ListSections[i].Disciples.Count != 0)
                    {
                        infolost.AddRange(news.ListSections[i].Disciples);
                    }
                }
            }

            for(int i = 0; i < infolost.Count; i++)
            {
                infolost[i].Info = HtmlClear.ClearHtml(infolost[i].Info);
                if (!string.IsNullOrEmpty(infolost[i].Info) && infolost[i].Info.Length > LengthInfo)
                {
                    infolost[i].Info = infolost[i].Info.Substring(0, LengthInfo) + "...";
                }
            }

            model = new Tuple<List<Information>, List<Section>, int>(infolost, news?.ListSections, id);

            return View(model);
        }

        private void LoadInformation(List<Information> list, Section section)
        {
            for (int i = 0; i < section.Children.Count(); i++)
            {
 
                _context.Entry(section.Children[i]).Collection("Disciples").Load();
                if (section.Children[i].Disciples.Count != 0)
                {
                    list.AddRange(section.Children[i].Disciples);
                }

                LoadInformation(list, section.Children[i]);
            }
        }

        [HttpPost, ValidateInput(false)]
        public async Task<ActionResult> AddInfoSection(Information model)
        {
            int? index = model.ButtonId;
            model.ButtonId = null;
            model.DateTime = DateTime.Now;
            if (model.Id == 0)
            {
                Section section = await _context.Sections.FirstOrDefaultAsync(r=>r.Id == model.SectionsId);
                if(section != null)
                {
                    model.SectionsId = null;
                    section.Disciples.Add(model);
                }
            }
            else
            {
                Information old = await _context.Information.FirstOrDefaultAsync(r => r.Id == model.Id);
                if (old != null)
                {
                    _context.Entry(old).CurrentValues.SetValues(model);
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("ListSectionInfo", new { id = index });
        }

        public async Task<ActionResult> EditInfoSection(int id, int idBtn)
        {
            Tuple<Information, List<Section>, int> tuple = null;
            Information elm = null;
            List<Section> list = await _context.Sections
                .Where(r => r.ButtonId == idBtn)
                .Include(r => r.Children)
                .ToListAsync();

            elm = await _context.Information.FirstOrDefaultAsync(r=>r.Id == id);

            tuple = new Tuple<Information, List<Section>, int>(elm, list, idBtn);

            return View("AddInfoSection", tuple);
        }

        public async Task<ActionResult> InfoPublicSection(int id,  int IdBtn, bool flagEdit)
        {
            Information elm = await _context.Information.FindAsync(id);
            int index = id;

            if (elm != null)
            {
                elm.IsPublick = !elm.IsPublick;
                await _context.SaveChangesAsync();
            }

            if(flagEdit)
            {
                return RedirectToAction("ListSectionInfo", new { @id = IdBtn });
            }

            return RedirectToAction("EditInfoSection", new { @id = id, @IdBtn = IdBtn });

        }

        public async Task<ActionResult> DeleteSectionInfo(int id, int IdBtn)
        {
            Information information = await _context.Information.FirstOrDefaultAsync(r=>r.Id == id);

            if(information != null)
            {
                _context.Information.Remove(information);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ListSectionInfo", new { @id = IdBtn });
        }

        public async Task<ActionResult> DeleteNewLent(int id, int IdBtn)
        {
            Information information = await _context.Information.FirstOrDefaultAsync(r => r.Id == id);

            if (information != null)
            {
                _context.Information.Remove(information);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("AddNewLents", new { @id = IdBtn });
        }

        public async Task<ActionResult> InfoPublicNewLent(int id, int IdBtn, bool flagEdit)
        {
            Information elm = await _context.Information.FindAsync(id);
            int index = id;

            if (elm != null)
            {
                elm.IsPublick = !elm.IsPublick;
                await _context.SaveChangesAsync();
            }

            if (flagEdit)
            {
                return RedirectToAction("AddNewLents", new { @id = IdBtn });
            }

            return RedirectToAction("AddInfoNewLent", new { @id = id, @IdBtn = IdBtn });

        }

        [HttpPost, ValidateInput(false)]
        public async Task<ActionResult> AddInfoNewLents(Information model)
        {
            int? index = model.ButtonId;
            model.DateTime = DateTime.Now;
            model.Cat = "ListModel";

            if (model.Id == 0)
            {
                if(index.HasValue)
                {
                    NewsModel btn = await _context.NewsModel.FirstOrDefaultAsync(r=>r.Id == index);
                    if(btn != null)
                    {
                        model.ButtonId = null;
                        btn.ListInformation.Add(model);
                    }
                }
            }
            else
            {
                Information old = await _context.Information.FirstOrDefaultAsync(r => r.Id == model.Id);
                if (old != null)
                {
                    _context.Entry(old).CurrentValues.SetValues(model);
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("AddNewLents", new { @id = index });
        }

        public async Task<ActionResult> SetVideo(VideoModel model)
        {
            int index = model.ButtonId.Value;
            string dsf = Request.Url.AbsolutePath;
            if (model.Id == 0)
            {
                NewsModel elm = await _context.NewsModel.FirstOrDefaultAsync(r=>r.Id == index);

                if(elm != null)
                {
                    model.ButtonId = null;
                    elm.ListVideo.Add(model);
                }
            }
            else
            {
                VideoModel old = await _context.VideoModels.FirstOrDefaultAsync(r=>r.Id == model.Id);
                if(old != null)
                {
                    _context.Entry(old).CurrentValues.SetValues(model);
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("AddVideo", new { @id = index });
        }

        public async Task<ActionResult> MainPageEdit()
        {
            Tuple<List<UIElementModel>, List<NewsModel>> list = null;

            List<UIElementModel> elmList = await _context.UIElements.ToListAsync();
            List<NewsModel> btnList = await _context.NewsModel.ToListAsync();
            list = new Tuple<List<UIElementModel>, List<NewsModel>>(elmList.Where(DataConfig.SortDataS).ToList(), btnList.Where(DataConfig.SortData).ToList());

            return View(list);
        }

        [HttpPost]
        public async Task<ActionResult> SavaElementConfig(List<UIElementModel> model)
        {
            List<UIElementModel> old = await _context.UIElements.Where(r=>r.Language == DataConfig.Language.CodeLang).ToListAsync();
            try
            {
                if (old == null || old.Count == 0 && model != null)
                {
                    model.ForEach((e) => 
                    {
                        e.Language = DataConfig.Language.CodeLang;
                        if(!string.IsNullOrEmpty(e.FileImg))
                        {
                            e.ImageName = Utilit.SaveImageBase64.Save(e.ImageName, e.FileImg);
                            e.FileImg = string.Empty;
                        }
                    });
                    _context.UIElements.AddRange(model);
                }
                else
                {
                    if (model != null)
                    {
                        UIElementModel uIElementModel = null;
                        for (int i = 0; i < model.Count; i++)
                        {
                            // Сохраняем картинку
                            if (!string.IsNullOrEmpty(model[i].ImageName))
                                model[i].ImageName = Utilit.SaveImageBase64.Save(model[i].ImageName, model[i].FileImg);
                            else if (!string.IsNullOrEmpty(model[i].FileImg))
                                model[i].ImageName = model[i].FileImg.TrimStart('\x5C');

                            model[i].FileImg = string.Empty;
                            model[i].Language = DataConfig.Language.CodeLang;

                            if (model[i].Id == 0)
                            {
                                _context.UIElements.Add(model[i]);
                            }
                            else
                            {
                                uIElementModel = old.Where(r => r.Id == model[i].Id).FirstOrDefault();
                                if (uIElementModel != null)
                                {
                                    _context.Entry(uIElementModel).CurrentValues.SetValues(model[i]);
                                }
                            }
                        }

                        for (int i = 0; i < old.Count; i++)
                        {
                            if (model.Where(r => r.Id == old[i].Id).FirstOrDefault() == null)
                            {
                                _context.UIElements.Remove(old[i]);
                            }
                        }
                    }
                    else
                    {
                        _context.UIElements.RemoveRange(old);
                    }
                }
            }
            catch(Exception el)
            {

            }

            await _context.SaveChangesAsync();

            return Content("MainPageEdit");
        }

        private void CurrectPathImg(List<UIElementModel> model)
        {
            char code = '\x5C';
            if(model != null)
            {
                for(int i = 0; i < model.Count; i++)
                {
                    model[i].ImageName = model[i].ImageName.TrimStart(code);
                }
            }
        }

        [HttpGet]
        public async Task<ActionResult> VideoGuide()
        {
            Tuple<VideoGuideModel, List<string>> model = null;
            VideoGuideModel elm = await _context.VideoGuideModels.FirstOrDefaultAsync();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "Video");
            List<string> files = Directory.GetFiles(path, "*", SearchOption.AllDirectories).ToList();
            model = new Tuple<VideoGuideModel, List<string>>(elm, files);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> SetVideoGuide(VideoGuideModel model)
        {
            VideoGuideModel old = await _context.VideoGuideModels.FirstOrDefaultAsync();
            if(old == null)
            {
                _context.VideoGuideModels.Add(model);
            }
            else
            {
                old.Cat = model.Cat;
                old.Path = model.Path;
                old.Header = model.Header;
            }
   
            await _context.SaveChangesAsync();

            Task fillTable = new Task(() =>
            {
                string path = Path.Combine(AppContext.BaseDirectory, "FileLoad");
                try
                {
                    System.IO.File.Copy(model.Path, Path.Combine(path, Path.GetFileName(model.Path)), true);
                }
                catch (Exception el)
                {

                }
            });

            fillTable.Start();

            return RedirectToAction("VideoGuide");
        }

        [HttpGet]
        public async Task<ActionResult> Slider()
        {
            Tuple<List<StorageAPI.Models.Database.Slider>, List<string>> model = null;
            List<StorageAPI.Models.Database.Slider> list = await _context.Slider.ToListAsync();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "Video");
            List<string> files = Directory.GetFiles(path, "*", SearchOption.AllDirectories).ToList();
            list = list.OrderBy(u => u.Number).ToList();
            model = new Tuple<List<StorageAPI.Models.Database.Slider>, List<string>>(list, files);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> AddSlider(StorageAPI.Models.Database.Slider model, string action, HttpPostedFileBase image = null)
        {
            if(model == null)
                return RedirectToAction("Slider");
            if (action == "Save")
            {
                if(model.ContentType == SliderContentType.Image)
                {
                    if(image != null || !string.IsNullOrEmpty(model.Content))
                    {
                        if(image != null)
                        {
                            string fileName = System.IO.Path.GetFileName(image.FileName);
                            // сохраняем файл в папку Files в проекте
                            image.SaveAs(Server.MapPath("~/Files/Image/" + fileName));
                            model.Content = Server.MapPath("~/Files/Image/" + fileName);
                        }


                        if (model.Id == 0)
                        {
                            int number = _context.Slider.ToList().Count + 1;
                            model.Number = number;
                            _context.Slider.Add(model);
                        }
                        else
                        {
                            StorageAPI.Models.Database.Slider old = await _context.Slider.FirstOrDefaultAsync(r=>r.Id == model.Id);
                            if(old != null)
                            {
                                _context.Entry(old).CurrentValues.SetValues(model);
                            }
                        }
                    }
                }

                if(model.ContentType == SliderContentType.Video)
                {
                    if (!string.IsNullOrEmpty(model.Content) && System.IO.File.Exists(model.Content))
                    {
                        if(model.Id == 0)
                        {
                            int number = _context.Slider.ToList().Count + 1;
                            model.Number = number;
                            _context.Slider.Add(model);
                        }
                        else
                        {
                            StorageAPI.Models.Database.Slider old = await _context.Slider.FirstOrDefaultAsync(r => r.Id == model.Id);
                            if (old != null)
                            {
                                _context.Entry(old).CurrentValues.SetValues(model);
                            }
                        }
                    }
                }

                if(!string.IsNullOrEmpty(model.Content))
                {
                    Task fillTable = new Task(() =>
                    {
                        string path = Path.Combine(AppContext.BaseDirectory, "FileLoad");
                        try
                        {
                            System.IO.File.Copy(model.Content, Path.Combine(path, Path.GetFileName(model.Content)), true);
                        }
                        catch (Exception el)
                        {

                        }
                    });

                    fillTable.Start();
                }
            }

            if (action == "Delete")
            {
                StorageAPI.Models.Database.Slider del = await _context.Slider.FirstOrDefaultAsync(r => r.Id == model.Id);
                if(del != null)
                {
                    _context.Slider.Remove(del);
                    await _context.SaveChangesAsync();

                    List<StorageAPI.Models.Database.Slider> list = await _context.Slider.ToListAsync();
                    if(list != null)
                    {
                        list = list.OrderBy(i => i.Number).ToList();
                        for (int i = 0; i < list.Count; i++)
                        {
                            list[i].Number = i + 1;
                        }
                    }
                }
            }

            if(action == "Up")
            {
                StorageAPI.Models.Database.Slider elm1 = await _context.Slider.FirstOrDefaultAsync(r => r.Id == model.Id);
                if(elm1 != null && elm1.Number > 1)
                {
                    StorageAPI.Models.Database.Slider elm2 = await _context.Slider.FirstOrDefaultAsync(r => r.Number == elm1.Number - 1);
                    if(elm2 != null)
                    {
                        elm2.Number = elm1.Number;
                        elm1.Number -= 1;
                    }
                }
            }

            if (action == "Dowen")
            {
                StorageAPI.Models.Database.Slider elment1 = await _context.Slider.FirstOrDefaultAsync(r => r.Id == model.Id);
                int number = _context.Slider.ToList().Count;
                if (elment1 != null && elment1.Number < number)
                {
                    StorageAPI.Models.Database.Slider elm2 = await _context.Slider.FirstOrDefaultAsync(r => r.Number == elment1.Number + 1);
                    if (elm2 != null)
                    {
                        elm2.Number = elment1.Number;
                        elment1.Number += 1;
                    }
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Slider");
        }
    }
}