using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace JLQ.Common
{
    //多值映射表。一个key，映射多个value。类似Dictionary<Key, List<Value>>
    public class MultiMap<U, T> : SortedList
    {
        public MultiMap()
        {
        }

        public object this[U key]
        {
            get
            {
                if (this.IndexOfKey(key) >= 0)
                {
                    List<T> o = (List<T>)base[key];
                    return o;
                }
                return null;
            }

            set
            {
                if (this.IndexOfKey(key) >= 0)
                {
                    List<T> o = (List<T>)this[key];
                    o.Add((T)value);
                    return;
                }
                List<T> ol = new List<T>();
                ol.Add((T)value);
                base[key] = ol;
            }
        }
    }
}
