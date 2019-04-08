using System.Collections.Generic;

namespace ModelData.Models.Database
{
    public class SurveyModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<SurveyValueModel> ListOption { get; set; }
        public string Language { get; set; }
        public SurveyModel()
        {
            ListOption = new List<SurveyValueModel>();
        }
    }
}
