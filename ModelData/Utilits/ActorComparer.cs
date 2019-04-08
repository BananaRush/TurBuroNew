using ModelData.DataCom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelData.Utilits
{
    public class ActorComparer : IEqualityComparer<FileModel>
    {
        bool IEqualityComparer<FileModel>.Equals(FileModel x, FileModel y)
        {
            return (x.Name.Equals(y.Name) && x.Size.Equals(y.Size));
        }

        int IEqualityComparer<FileModel>.GetHashCode(FileModel obj)
        {
            if (Object.ReferenceEquals(obj, null))
                return 0;

            return obj.Name.GetHashCode();
        }
    }
}
