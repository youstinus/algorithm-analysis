using System;
using System.Collections.Generic;
using System.IO;
using SortBase;

namespace MergeSortOP
{
    class Program
    {
        static void Main(string[] args)
        {
            ManualTesting();

            Console.WriteLine("\n --=< Done >=--");
            Console.ReadKey();
        }

        private static void ManualTesting()
        {
            var seed = (int)DateTime.Now.Ticks & 0x0000FFFF;

            Console.Write(" Type file name or leave blank to generate data: ");
            var file = Console.ReadLine();

            var n = 1000;
            if (string.IsNullOrWhiteSpace(file))
            {
                Console.Write("\n Data count? [1000]: ");
                var number = Console.ReadLine();
                n = string.IsNullOrWhiteSpace(number) ? 1000 : Convert.ToInt32(number);
            }

            Console.Write(" Print out Count? [0]: ");
            var line = Console.ReadLine();
            var count = string.IsNullOrWhiteSpace(line) ? 0 : Convert.ToInt32(line);

            bool correctArraySort;
            bool correctListSort;

            if (!string.IsNullOrWhiteSpace(file))
            {
                correctArraySort = TestArrayFromFile(file, count);
                correctListSort = TestListFromFile(file, count);
            }
            else
            {
                correctArraySort = TestArray(n, seed, count);
                correctListSort = TestList(n, seed, count);
            }

            Console.WriteLine("\n Array {0}", correctArraySort ? "CORRECT" : "NOT CORRECT");
            Console.WriteLine(" List  {0}", correctListSort ? "CORRECT" : "NOT CORRECT");
        }

        private static bool TestArrayFromFile(string path, int count)
        {
            var numbers = ReadFile(path);
            var dataArray = new DataArray(numbers);
            Console.WriteLine("\n Merge sort against Array inside RAM. Data from file \n");
            dataArray.Print(count);
            //dataArray.PrintFromTo(5, 15);

            var watch = System.Diagnostics.Stopwatch.StartNew();

            var sortedArray = dataArray.MergeSortArray();

            watch.Stop();
            var elapsed = watch.ElapsedMilliseconds / 1000.0;
            Console.WriteLine("Elapsed: {0}s\n", elapsed);

            sortedArray.Print(count);
            //sortedArray.PrintFromTo(5, 15);
            var correct = sortedArray.CheckSorted();
            Console.WriteLine(correct ? " CORRECT!!!" : " INCORRECT???");
            return correct;
        }

        private static bool TestListFromFile(string path, int count)
        {
            var numbers = ReadFile(path);
            var dataList = new DataList(numbers);
            Console.WriteLine("\n Merge sort against LinkedList inside RAM. Data from file \n");
            dataList.Print(count);
            //dataList.PrintFromTo(5, 15);

            var watch = System.Diagnostics.Stopwatch.StartNew();

            var sortedList = dataList.MergeSortList();

            watch.Stop();
            var elapsed = watch.ElapsedMilliseconds / 1000.0;
            Console.WriteLine("Elapsed: {0}s\n", elapsed);

            sortedList.Print(count);
            //sortedList.PrintFromTo(5, 15);
            var correct = sortedList.CheckSorted();
            Console.WriteLine(correct ? " CORRECT!!!" : " INCORRECT???");
            return correct;
        }

        private static bool TestArray(int n, int seed, int count)
        {
            var dataArray = new DataArray(n, seed);
            Console.WriteLine("\n Merge sort against Array inside RAM. Data generated \n");
            dataArray.Print(count);
            //dataArray.PrintFromTo(5, 15);

            Counter.Reset();
            var watch = System.Diagnostics.Stopwatch.StartNew();

            var sortedArray = dataArray.MergeSortArray();

            watch.Stop();
            var elapsed = watch.ElapsedMilliseconds / 1000.0;
            Console.WriteLine("Elapsed: {0}s", elapsed);
            Console.WriteLine("Operations: {0}\n", Counter.Get());

            sortedArray.Print(count);
            //sortedArray.PrintFromTo(5, 15);
            var correct = sortedArray.CheckSorted();
            Console.WriteLine(correct ? " CORRECT!!!" : " INCORRECT???");
            return correct;
        }

        private static bool TestList(int n, int seed, int count)
        {
            var dataList = new DataList(n, seed);
            Console.WriteLine("\n Merge sort against LinkedList inside RAM. Data generated \n");
            dataList.Print(count);
            //dataList.PrintFromTo(5, 15);

            var watch = System.Diagnostics.Stopwatch.StartNew();

            var sortedList = dataList.MergeSortList();

            watch.Stop();
            var elapsed = watch.ElapsedMilliseconds / 1000.0;
            Console.WriteLine(" Elapsed: {0}s\n", elapsed);

            sortedList.Print(count);
            //sortedList.PrintFromTo(5, 15);
            var correct = sortedList.CheckSorted();
            Console.WriteLine(correct ? " CORRECT!!!" : " INCORRECT???");
            return correct;
        }

        private static Numbers[] ReadFile(string path)
        {
            if (path.EndsWith(".txt"))
                return ReadTextFile(path);

            var numbersList = new List<Numbers>();

            using (var reader = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                var buffer = new byte[12];

                while (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    reader.Read(buffer, 0, 12);
                    var first = BitConverter.ToDouble(buffer, 0);
                    var second = BitConverter.ToInt32(buffer, 8);
                    var numbers = new Numbers(first, second);
                    numbersList.Add(numbers);
                }
            }

            return numbersList.ToArray();
        }

        private static Numbers[] ReadTextFile(string path)
        {
            var numbersList = new List<Numbers>();

            using (var reader = new StreamReader(File.Open(path, FileMode.Open)))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(' ');
                    var first = Convert.ToDouble(parts[0]);
                    var second = Convert.ToInt32(parts[1]);
                    var numbers = new Numbers(first, second);
                    numbersList.Add(numbers);
                }
            }

            return numbersList.ToArray();
        }
    }
}
