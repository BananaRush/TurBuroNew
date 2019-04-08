
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelData.Models.Database
{
    public class SurveyValueModel
    {
        public int Id { get; set; }
        public string Option { get; set; }
        public int OptionCout { get; set; }

        public int? SurveyId { get; set; }
        // Секция 
        [ForeignKey("SurveyId")]
        public SurveyModel Survey { get; set; }
    }
}
