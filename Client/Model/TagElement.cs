using ModelData.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model
{
    class TagElement
    {
        public int Id { get; set; }
        public bool IsNavVisibility { get; set; }
        public int HashItemData { get; set; }
        public UIElementModel UIElementModel { get; set; }
        public TagElement(int Id, int HashItemData, bool IsNavVisibility = false, UIElementModel uIElementModel = null)
        {
            this.Id = Id;
            this.HashItemData = HashItemData;
            this.IsNavVisibility = IsNavVisibility;
            this.UIElementModel = uIElementModel;
        }
    }
}
