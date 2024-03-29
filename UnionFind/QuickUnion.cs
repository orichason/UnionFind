﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace UnionFind
{
    public class QuickUnion<T>
    {
        public struct Node
        {
            public int Value;
            public int SubTreeCount;
            public override string ToString() => $"Parent : {Value}";
        }

        public Node[] parents;
        public Dictionary<T, int> map;

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
            parents = new Node[map.Count];
            for (int i = 0; i < parents.Length; i++)
            {
                parents[i].Value = i;
                parents[i].SubTreeCount = 1;
            }
        }

        private QuickUnion(QuickUnion<T> quickUnion)
        {
            this.map = quickUnion.map;

            this.parents = (Node[])quickUnion.parents.Clone();
        }

        public int Find(T p)
        {
            int current = parents[map[p]].Value; 

            while (parents[current].Value != current)
            {
                current = parents[current].Value;
            }

            return current;
        }

        public bool Union(T p, T q)
        {
            if (!map.ContainsKey(p) || !map.ContainsKey(q)) throw new ArgumentException("Key(s) not in map");

            int pRoot = Find(p);
            int qRoot = Find(q);

            if (parents[pRoot].SubTreeCount > parents[qRoot].SubTreeCount) return Union(qRoot, pRoot);
            
            return Union(pRoot, qRoot);
        }

        private bool Union(int setToChange, int newSet)
        {
            if(newSet == setToChange) return false;

            parents[setToChange].Value = newSet;
            parents[newSet].SubTreeCount += parents[setToChange].SubTreeCount;

            return true;
        }

        public QuickUnion<T> Clone()
        {
            return new(this);
        }        
    }
}
