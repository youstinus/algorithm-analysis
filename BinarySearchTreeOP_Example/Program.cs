using System;
using System.Collections.Generic;
using System.Text;

namespace BinarySearchTreeOP_Example
{
    class Program
    {
        static Random random;
        static void Main(string[] args)
        {
            var nameList = new List<string>();
            var strTree = new BinarySearchTree();
            var seed = (int)DateTime.Now.Ticks & 0x0000FFFF;
            random = new Random(seed);
            var n = 15;
            for (var i = 0; i < n; i++)
            {
                var s = RandomName(10);
                nameList.Add(s);
                strTree.Insert(s);
            }
            nameList.Add(RandomName(10));
            Console.WriteLine(" Binary Search Tree \n");
            strTree.Print();
            Console.WriteLine("\n Search Test \n");
            foreach (var s in nameList)
            {
                Console.Write(strTree.Contains(s).ToString() + " ");
            }
            Console.WriteLine("\n");
            Console.ReadKey();
        }
        static string RandomName(int size)
        {
            var builder = new StringBuilder();
            var ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 *
                                                                random.NextDouble() + 65)));
            builder.Append(ch);
            for (var i = 1; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 *
                                                               random.NextDouble() + 97)));
                builder.Append(ch);
            }
            return builder.ToString();
        }
    }

}
