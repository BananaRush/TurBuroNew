using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSHIM
{
    class ListMod<T> : List<T>
    {

        public event EventHandler Changed;

        public new void Add(T item)
        {
            base.Add(item);
            Changed(this, null);
        }

        public new void AddRange(IEnumerable<T> item)
        {
            base.AddRange(item);
            Changed(this, null);
        }

        public new void Clear()
        {
            base.Clear();
            Changed(this, null);
        }

        public new bool Remove(T item)
        {
            bool isflag = base.Remove(item);
            Changed(this, null);
            return isflag;
        }

        public new int RemoveAll(Predicate<T> match)
        {
            int val = base.RemoveAll(match);
            Changed(this, null);
            return val;
        }

        public new void RemoveAt(int index)
        {
            base.RemoveAt(index);
            Changed(this, null);
        }

        public void Changeds()
        {
            Changed(this, null);
        }
    }
}
