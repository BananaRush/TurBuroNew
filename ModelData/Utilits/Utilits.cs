using ModelData.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelData.Utilits
{
    static public class Utilits
    {
        static public void SetParnets(List<Section> list, Section parnet)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (parnet != null)
                {
                    list[i].Parent = parnet;
                }

                SetParnets(list[i].Children, list[i]);
            }
        }
    }
}
