using System.Globalization;
using System.Net.Quic;

using UnionFind;

using TestUnion = UnionFind.QuickUnion<char>;

namespace UnionFindTest
{
    [TestClass]
    public class QuickFindTest
    {
        [TestMethod]
        [DataRow(new int[] { 1, 2, 3, 4, 5, 6 }, 2, 4)]
        [DataRow(new int[] { 4, 10, 33, 99 }, 10, 99)]
        [DataRow(new int[] { 10, 41, 2 }, 10, 41)]
        
        public void SetsMatchTest(int[] items, int indexToSet, int newIndex)
        {
            QuickFind<int> quickFind = new(items);

            quickFind.Union(indexToSet, newIndex);

            Assert.IsTrue(quickFind.AreConnected(indexToSet, newIndex));
        }
    }

    [TestClass]
    
    public class QuickUnionTest
    {
        [TestMethod]
        [DataRow(new int[] { 6, 5, 4, 3, 2, 1 }, 2, 4)]
        [DataRow(new int[] { 6, 1000, 3313, 199 }, 199, 6)]
        [DataRow(new int[] { 10, 41, 2, 99, 0 }, 10, 41)]
        
        public void SetsMatchTest(int[] items, int indexToSet, int newIndex)
        {
            QuickUnion<int> quickUnion = new(items);

            quickUnion.Union(indexToSet, newIndex);

            Assert.IsTrue(quickUnion.Find(indexToSet) == quickUnion.Find(newIndex));
        }


        [TestMethod]
        public void EasyPathTest() => QuickestPathTest(['a', 'b', 'c', 'd', 'e'], [('a', 'b'), ('a', 'd'), ('c', 'a')]);

        [TestMethod]
        [DataRow(100, 4, 1)]
        [DataRow(50, 8, 2)]
        [DataRow(20, 7, 10)]
        [DataRow(200, 10, 6)]
        [DataRow(10, 5, 4)]

        public void BigPathTest(int dataSize, int unionCount, int seed)
        {
            Random randall = new(seed);
            char[] set = new char[dataSize];

            for (int i = 0; i < dataSize; i++)
            {
                set[i] = (char)i;
            }

            (char, char)[] unions = new ValueTuple<char, char>[unionCount];

            for (int i = 0; i < unionCount; i++)
            {
                unions[i] = ((char)randall.Next(dataSize), (char)randall.Next(dataSize));
            }

            QuickestPathTest(set, unions);
        }

        private void QuickestPathTest(char[] items, (char, char)[] unions)
        {
            List<TestUnion> slowUnionList = GetAllPermutations(items, unions);

            int lowestCount = int.MaxValue;

            for (int i = 0; i < slowUnionList.Count; i++)
            {
                int totalCountForUnion = GetTreeCount(slowUnionList[i]);

                if (totalCountForUnion < lowestCount)
                {
                    lowestCount = totalCountForUnion;
                }
            }

            TestUnion quickUnion = new(items);
            for (int i = 0; i < unions.Length; i++)
            {
                quickUnion.Union(unions[i].Item1, unions[i].Item2);
            }

            int quickUnionTreeCount = GetTreeCount(quickUnion);

            Assert.IsTrue(quickUnionTreeCount == lowestCount);
        }

        private int GetTreeCount(TestUnion union)
        {
            int totalCountForUnion = 0;

            foreach (var item in union.map.Keys)
            {
                totalCountForUnion += Find(union, item);

            }

            return totalCountForUnion;
        }

        /// <summary>
        /// Returns a list of all permutations of unions passed in
        /// </summary>
        /// <param name="items"></param>
        /// <param name="unions"></param>
        /// <returns></returns>
        private List<TestUnion> GetAllPermutations(char[] items, (char, char)[] unions)
        {
            List<TestUnion> slowUnionList = new((int)Math.Pow(2, unions.Length)) { new TestUnion(items) };

            for (int i = 0; i < unions.Length; i++)
            {
                int rowSize = (int)Math.Pow(2, i);
                for (int j = 0; j < rowSize; j++)
                {
                    TestUnion union = slowUnionList[j];
                    TestUnion temp = GetUnions(union, unions[i]);

                    slowUnionList[j] = union;
                    slowUnionList.Add(temp);
                }
            }

            return slowUnionList;
        }
        /// <summary>
        /// Overrides the given testUnion, and returns another permutation of the same union.
        /// </summary>
        /// <param name="testUnion"></param>
        /// <param name="unions"></param>
        /// <returns></returns>
        private TestUnion GenerateChildUnions(TestUnion testUnion, (char, char) unions)
        {
            var unionOne = testUnion;
            var unionTwo = testUnion.Clone();


            Union(unionOne, unions.Item1, unions.Item2);
            Union(unionTwo, unions.Item2, unions.Item1);

            //only returning unionTwo because unionOne is referenced to testUnion (which is passed in and modified)
            return unionTwo;
        }

        private void Union(TestUnion testUnion, char p, char q)
           => testUnion.parents[testUnion.Find(p)].Value = testUnion.Find(q);

        /// <summary>
        /// Returns how many iterations it takes to get to parent 
        /// </summary>
        /// <param name="union"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        private int Find(TestUnion union, char p)
        {
            int current = union.map[p];
            int count = 1;

            while (union.parents[current].Value != current)
            {
                current = union.parents[current].Value;
                count++;
            }

            return count;
        }
    }
}