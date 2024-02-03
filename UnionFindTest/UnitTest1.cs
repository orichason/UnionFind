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
        public void EasyPathTest() => QuickestPathTest(['a', 'b', 'c', 'd', 'e'], [('a', 'b'), ('c', 'd'), ('d', 'a')], 1);
        //[DataRow(new char[] { 'a', 'b', 'c', 'd', 'z' }, new (char, char)[] { ('a', 'b')} , 1)]

        public void QuickestPathTest(char[] items, (char, char)[] unions, int seed)
        {
            List<TestUnion> slowUnionList = GetAllPermutations(items, unions);

            for (int i = 0; i < slowUnionList.Count; i++)
            {
                int totalCountForUnion = 0;
                int lowestCount = 0;

                for (int j = 0; j < items.Length; j++)
                {
                    int tempCount = Find(slowUnionList[i], items[j]);
                    totalCountForUnion += tempCount;
                }

                if(totalCountForUnion < lowestCount)
                {
                    lowestCount = totalCountForUnion;
                }
            }

        }

        private List<TestUnion> GetAllPermutations(char[] items, (char, char)[] unions)
        {
            TestUnion quickUnion = new(items);
            List<TestUnion> slowUnionList = new((int)Math.Pow(2, unions.Length));

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
        private TestUnion GetUnions(TestUnion testUnion, (char, char) unions)
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


        public void SlowUnion(TestUnion testUnion, char p, char q)
        {
            //TODO: breakpoint and make sure everything is unioning correctly and then compare children counts when done
            if (!testUnion.map.ContainsKey(p) || !testUnion.map.ContainsKey(q)) throw new ArgumentException("Key(s) not in map");
            int setToChange = testUnion.Find(p);
            int newSet = testUnion.Find(q);

            testUnion.parents[setToChange].Value = newSet;
            testUnion.parents[newSet].SubTreeCount += testUnion.parents[setToChange].SubTreeCount;
        
        }
        int Find(TestUnion union, char p)
        {
            int current = union.parents[union.map[p]].Value;
            int count = 0;

            while (union.parents[current].Value != current)
            {
                current = union.parents[current].Value;
                count++;
            }

            return count;
        }
    }
}