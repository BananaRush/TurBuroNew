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
    public class ValCursValuteModel
    {

        /// <remarks/>
        public ushort NumCode { get; set; }

        /// <remarks/>
        public string CharCode { get; set; }

        /// <remarks/>
        public ushort Nominal { get; set; }

        /// <remarks/>
        public string Name { get; set; }

        /// <remarks/>
        public string Value { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ID { get; set; }
    }
}
