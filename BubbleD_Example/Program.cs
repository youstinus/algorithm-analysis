using System;
using System.IO;

namespace BubbleD_Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var seed = (int)DateTime.Now.Ticks & 0x0000FFFF;

            // Antras etapas
            Test_File_Array_List(seed);
            Console.ReadKey();
        }
        public static void BubbleSort(DataArray items)
        {
            double prevdata, currentdata;
            for (var i = items.Length - 1; i >= 0; i--)
            {
                currentdata = items[0];
                for (var j = 1; j <= i; j++)
                {
                    prevdata = currentdata;
                    currentdata = items[j];
                    if (prevdata > currentdata)
                    {
                        items.Swap(j, currentdata, prevdata);
                        currentdata = prevdata;
                    }
                }
            }
        }
        public static void BubbleSort(DataList items)
        {
            double prevdata, currentdata;
            for (var i = items.Length - 1; i >= 0; i--)
            {
                currentdata = items.Head();
                for (var j = 1; j <= i; j++)
                {
                    prevdata = currentdata;
                    currentdata = items.Next();
                    if (prevdata > currentdata)
                    {
                        items.Swap(currentdata, prevdata);
                        currentdata = prevdata;
                    }
                }
            }
        }
        public static void Test_File_Array_List(int seed)
        {
            var n = 12;
            string filename;
            filename = @"mydataarray.dat";
            var myfilearray = new MyFileArray(filename, n, seed);
            using (myfilearray.fs = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite))
            {
                Console.WriteLine("\n FILE ARRAY \n");
                myfilearray.Print(n);
                BubbleSort(myfilearray);
                myfilearray.Print(n);
            }
            filename = @"mydatalist.dat";
            var myfilelist = new MyFileList(filename, n, seed);
            using (myfilelist.fs = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite))
            {
                Console.WriteLine("\n FILE LIST \n");
                myfilelist.Print(n);
                BubbleSort(myfilelist);
                myfilelist.Print(n);
            }
        }
    }
    abstract class DataArray
    {
        protected int length;
        public int Length { get { return length; } }
        public abstract double this[int index] { get; }
        public abstract void Swap(int j, double a, double b);
        public void Print(int n)
        {
            for (var i = 0; i < n; i++)
                Console.Write(" {0:F5} ", this[i]);
            Console.WriteLine();
        }
    }
    abstract class DataList
    {
        protected int length;
        public int Length => length;
        public abstract double Head();
        public abstract double Next();
        public abstract void Swap(double a, double b);

        public void Print(int n)
        {
            Console.Write(" {0:F5} ", Head());
            for (var i = 1; i < n; i++)
                Console.Write(" {0:F5} ", Next());
            Console.WriteLine();
        }
    }
}
