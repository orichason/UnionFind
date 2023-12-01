namespace UnionFind
{
    internal class Program
    {
        static void Main(string[] args)
        {
            char[] items = new char[] { 'a', 'b', 'c', 'd', 'z', 'e', 'f', 'g', 'h', 'i' };

            QuickUnion<char> quickUnion = new(items);

            quickUnion.Union('b', 'a');
            quickUnion.Union('d', 'c');
            quickUnion.Union('c', 'a');
            quickUnion.Union('a', 'z');

           // quickUnion.Union('i', 'h');
            //quickUnion.Union('g', 'f');
            //quickUnion.Union('h', 'e');
            //quickUnion.Union('f', 'e');
            //quickUnion.Union('i', 'd');
            quickUnion.Union('a', 'z');


            for (char i = 'a'; i < 'i'; i++)
            {
                for (char j = i; j < 'i'; j++)
                {
                    Console.WriteLine(quickUnion.Union(i, j));
                }
            }
            ;
        }
    }
}