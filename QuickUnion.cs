using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnionFind
{
    internal class QuickUnion<T>
    {
        private int[] parents;
        private Dictionary<T, int> map;

        public QuickUnion(IEnumerable<T> items)
        {
            map = new Dictionary<T, int>(items.Count());

            int index = 0;
            foreach (var item in items)
            {
                map.Add(item, index);
                index++;
            }

            //initialize sets[] with values same as index
            parents = new int[map.Count];
            for (int i = 0; i < parents.Length; i++)
            {
                parents[i] = i;
            }
        }

        public int Find(T p)
        {
            int current = parents[map[p]];

            while (parents[current] != current)
            {
                current = parents[current];
            }

            return current;
        }

        public bool Union(T p, T q)
        {
            if (!map.ContainsKey(p) || !map.ContainsKey(q)) throw new ArgumentException("Key(s) not in map");

            for (int i = 0; i < parents.Length; i++)
            {
                if (parents[i] == setToChange)
                {
                    parents[i] = newSet;
                }
            }
        }
    }
}
