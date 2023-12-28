using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace UnionFind
{
    public class QuickFind<T>
    {
        private int[] sets;
        private Dictionary<T, int> map;

        public QuickFind(IEnumerable<T> items)
        {
            map = new Dictionary<T, int>(items.Count());

            int index = 0;
            foreach (var item in items)
            {
                map.Add(item, index);
                index++;
            }

            //initialize sets[] with values same as index
            sets = new int[map.Count];
            for (int i = 0; i < sets.Length; i++)
            {
                sets[i] = i;
            }
        }

        public int Find(T p) => sets[map[p]];
 
        public bool Union(T p, T q)
        {
            if (!map.ContainsKey(p) || !map.ContainsKey(q)) throw new ArgumentException("Key(s) not in map");
            int setToChange = Find(p);
            int newSet = Find(q);

            for (int i = 0; i < map.Count; i++)
            {
                if (sets[i] == setToChange)
                {
                    sets[i] = newSet;
                }
            }

            return true;
        }

        public bool AreConnected(T p, T q)
            => Find(p) == Find(q);
        
    }
}
