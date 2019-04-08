using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Controls.Currency.Model
{
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute("ValCurs", Namespace = "", IsNullable = false)]
    public class ValCursModel
    {
        private ValCursValuteModel[] valuteField;

        private string nameField;

        private string dateField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Valute")]
        public ValCursValuteModel[] Valute
        {
            get
            {
                return this.valuteField;
            }
            set
            {
                this.valuteField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Date
        {
            get
            {
                return this.dateField;
            }
            set
            {
                this.dateField = value;
            }
        }
    }
}
