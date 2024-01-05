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
        [DataRow(new char[] { 'a', 'b', 'c', 'd', 'z' }, 10, 1)]

        public void QuickestPathTest(char[] items, int numberOfUnions, int seed)
        {
            Random random = new(seed);
            TestUnion quickUnion = new(items);

            for (int i = 0; i < numberOfUnions; i++)
            {
                int indexToSet = random.Next(items.Length);
                int newIndex = random.Next(items.Length);

                if (indexToSet == newIndex)
                {
                    i--;
                    continue;
                }

                quickUnion.Union(items[indexToSet], items[newIndex]);
            }

            List<TestUnion> slowUnionList = new((int)Math.Pow(2, numberOfUnions));

            for (int i = 0; i < slowUnionList.Capacity; i++)
            {
                // { 'a', 'b', 'c', 'd', 'z' }
                TestUnion temp = new(items);
                int unionIndex = i % items.Length;
                for (int j = 0, unionCount = 0; unionCount < numberOfUnions; j++, unionCount++)
                {
                    if(unionIndex == j)
                    {
                        unionCount--;
                        continue;
                    }
                    SlowUnion(temp, items[unionIndex], items[j % items.Length]);
                }
                slowUnionList.Add(temp);
            }

            for (int i = 0; i < slowUnionList.Count; i++)
            {
                for (int j = 0; j < items.Length; j++)
                {
                    if (slowUnionList[i].parents[j].SubTreeCount < quickUnion.parents[j].SubTreeCount)
                    {
                        Assert.Fail();
                    }
                }
            }
        }

        public void SlowUnion(TestUnion testUnion, char p, char q)
        {
            //TODO: breakpoint and make sure everything is unioning correctly and then compare children counts when done
            if (!testUnion.map.ContainsKey(p) || !testUnion.map.ContainsKey(q)) throw new ArgumentException("Key(s) not in map");
            int setToChange = testUnion.Find(p);
            int newSet = testUnion.Find(q);

            testUnion.parents[setToChange].Value = newSet;
            testUnion.parents[newSet].SubTreeCount += testUnion.parents[newSet].SubTreeCount;
           
        }
    }
}