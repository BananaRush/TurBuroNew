using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourAgency.Model
{
    #region XmlSerialisation
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ValCurs
    {

        private ValCursValute[] valuteField;

        private string nameField;

        private string dateField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Valute")]
        public ValCursValute[] Valute
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

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ValCursValute
    {

        private ushort numCodeField;

        private string charCodeField;

        private ushort nominalField;

        private string nameField;

        private string valueField;

        private string idField;

        /// <remarks/>
        public ushort NumCode
        {
            get
            {
                return this.numCodeField;
            }
            set
            {
                this.numCodeField = value;
            }
        }

        /// <remarks/>
        public string CharCode
        {
            get
            {
                return this.charCodeField;
            }
            set
            {
                this.charCodeField = value;
            }
        }

        /// <remarks/>
        public ushort Nominal
        {
            get
            {
                return this.nominalField;
            }
            set
            {
                this.nominalField = value;
            }
        }

        /// <remarks/>
        public string Name
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
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }
        #endregion

    public class Currency
    {
        private char _Symbol;
        public char Symbol
        {
            get { return _Symbol; }
            set
            {
                _Symbol = value;
            }
        }

        private int _Value;
        public int Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
            }
        }

        private bool _IsDisplayed;
        public bool IsDisplayed
        {
            get { return _IsDisplayed; }
            set
            {
                _IsDisplayed = value;
            }
        }
    }

}
