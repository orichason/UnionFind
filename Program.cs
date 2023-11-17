namespace UnionFind
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] items = new string[] { "Bob", "Sue", "Joe", "Sally"};

            QuickFind<string> quickFind = new(items);

            quickFind.Union("Bob", "Sue");
        }
    }
}