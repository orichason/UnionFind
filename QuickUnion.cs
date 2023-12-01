using System;
using System.Collections.Generic;
using System.Drawing;
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

            //initialize parents[] with values same as index
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

        //TO DO: figure out a way to know how many children a node has because count is 0 when needed to be more
        private int Find(T p, out int count)
        {
            int current = parents[map[p]];
            count = 0;

            while (parents[current] != current)
            {
                current = parents[current];
                count++;
            }

            return current;
        }
        public bool Union(T p, T q)
        {
            if (!map.ContainsKey(p) || !map.ContainsKey(q)) throw new ArgumentException("Key(s) not in map");

            int pCount;
            int qCount;
            int setP = Find(p, out pCount);
            int setQ = Find(q, out qCount);

            if (pCount <= qCount) return Union(setP, setQ);
            
            return Union(setQ, setP);
        }

        private bool Union(int setToChange, int newSet)
        {
            if (newSet == setToChange) return false;

            parents[setToChange] = newSet;

            return true;
        }
    }
}
