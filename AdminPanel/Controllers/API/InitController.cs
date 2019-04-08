using ModelData.DataCom;
using ModelData.Model.Database;
using ModelData.Models.Database;
using StorageAPI.Models.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
namespace AdminPanel.Controllers.API
{
    public class InitController : ApiController
    {
        [HttpGet]
        public string Install()
        {
            try
            {
                using (TourAgencyContext context = new TourAgencyContext())
                {


                    context.LanguageModels.AddRange(new List<LanguageModel>() {
                new LanguageModel()
                {
                     Name = "Русский",
                     CodeLang = "RU",
                     IsActive = true
                },
                new LanguageModel()
                {
                     Name = "Aнглиский",
                     CodeLang = "EN",
                     IsActive = false
                },
                new LanguageModel()
                {
                     Name = "Китайский",
                     CodeLang = "CN",
                     IsActive = false
                }
            });

                context.TerminalsModels.Add(new TerminalsModel()
                {
                    TerminalId = Guid.Parse("0f8fad5b-d9cb-469f-a165-70867728950e"),
                    Name = "LoacalHost",
                    IsAutorizate = true
                });

                    // сохраняем 
                    context.SaveChanges();
                }
                return "Ок";
            }
            catch(Exception e)
            {
                return e.ToString();
            }
        }
    }
}