using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Controls.Currency
{
    public class CurrencyModel
    {
        public char Symbol { get; set; }
        public int Value { get; set; }
        public bool IsDisplayed { get; set; }
    }
}
