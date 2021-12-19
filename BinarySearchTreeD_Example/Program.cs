using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BinarySearchTreeD_Example
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
            var k = 10;
            for (var i = 0; i < n; i++)
            {
                var s = RandomName(k);
                nameList.Add(s);
                strTree.Insert(s);
            }
            nameList.Add(RandomName(k));
            Console.WriteLine(" Binary Search Tree \n");
            strTree.Print();
            //Console.WriteLine("\n Search Test \n");
            //foreach (var s in nameList)
            //{
            // Console.Write(strTree.Contains(s).ToString() + " ");
            //}
            //Console.WriteLine("\n");
            Console.WriteLine("\n Search FILE Test \n");
            var filename = @"mydatatree.dat";
            strTree.WriteToFile(filename, n);
            using (var fs = new FileStream(filename, FileMode.Open,
                FileAccess.ReadWrite))
            {
                foreach (var s in nameList)
                {
                    Console.Write(strTree.FileContains(fs, s).ToString() + " ");
                }
                Console.WriteLine("\n");
            }
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
