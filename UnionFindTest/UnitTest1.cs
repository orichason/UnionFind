using UnionFind;

namespace UnionFindTest
{
    [TestClass]
    public class QuickFindTest
    {
        [TestMethod]
        [DataRow(new int[] {1, 2, 3, 4, 5, 6}, 2, 4)]
        [DataRow(new int[] {4, 10, 33, 99 }, 10, 99)]
        [DataRow(new int[] {10, 41, 2}, 10, 41)]
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
        [DataRow(new int[] { 10, 41, 2, 99, 0}, 10, 41)]

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
            QuickUnion<char> quickUnion = new(items);

            int count = 0;
            while(count < numberOfUnions)
            { 
                int pIndex = random.Next(items.Length);
                int qIndex = random.Next(items.Length);
                if(pIndex == qIndex)
                {
                    count--;
                    continue;
                }
                quickUnion.Union(items[pIndex], items[qIndex]);

                count++;
            }
            
            //figure out a way to compare to other trees or a way to to determine if union is being efficient
        }
    }
}