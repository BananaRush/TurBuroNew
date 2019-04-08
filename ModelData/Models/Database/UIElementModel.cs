using ModelData.Interface;
using System.Collections.Generic;

namespace ModelData.Models.Database
{
    public enum UIElementType : int
    {
        NavigationFrame = 0,
        Survey,
        Slider,
        Weather,
        Currency,
        Time,
        LiveBroadcast,
        News,
        Language,
        FeatureModule,
        FeatureFrame,
        FeatureLoop,
        BackButton,
        Logotype,
        ButtonNav,
        Still,
        VideoGuide
    };

    public class UIElementModel : ILnagModel
    {
        public int Id { get; set; }
        public string Top { get; set; }
        public string Left { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public int? ButtonNavId { get; set; }
        public bool IsNavVisibility { get; set; }
        public string FileImg { get; set; }
        public string ImageName { get; set; }
        public string ColorText { get; set; }
        public string Background { get; set; }
        public string FontFamily { get; set; }
        public int FontSize { get; set; }
        public string Language { get; set; }
        public UIElementType ElementType { get; set; } 
        public List<TerminalDataModel> ListTerminal { get; set; }

        public UIElementModel()
        {
            ListTerminal = new List<TerminalDataModel>();
        }
    }
}
