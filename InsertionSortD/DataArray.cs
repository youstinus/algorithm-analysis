using System;
using System.IO;
using SortBase;

namespace InsertionSortD
{
    public interface IDataArray
    {
        int Start { get; set; }
        int End { get; set; }
        int Length { get; }
        FileStream Fs { get; set; }
        Numbers this[int index] { get; set; }
        void InsertionSortArray();
        bool CheckSorted();
        void Print(int n);
        void PrintFromTo(int from, int to);
    }

    public class DataArray : IDataArray
    {
        public int Start { get; set; }
        public int End { get; set; }
        public int Length { get; }
        public FileStream Fs { get; set; }

        public Numbers this[int index]
        {
            get
            {
                var dataDouble = new byte[8];
                var dataInt = new byte[4];
                Fs.Seek(12 * index, SeekOrigin.Begin);
                Fs.Read(dataDouble, 0, 8);
                Fs.Read(dataInt, 0, 4);
                var result = new Numbers(dataDouble, dataInt);
                return result;
            }
            set
            {
                var dataDouble = BitConverter.GetBytes(value.First);
                var dataInt = BitConverter.GetBytes(value.Second);
                Fs.Seek(12 * index, SeekOrigin.Begin);
                Fs.Write(dataDouble, 0, 8);
                Fs.Write(dataInt, 0, 4);
            }
        }

        public DataArray(int n, int seed)
        {
            Fs = new FileStream("array.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            Length = n;
            Start = 0;
            End = n;
            var rand = new Random(seed);

            for (var i = 0; i < Length; i++)
            {
                this[i] = new Numbers(rand.NextDouble(), rand.Next());
            }
        }

        public DataArray(Numbers[] numbers)
        {
            const string filename = "array.dat";

            if (File.Exists(filename))
                File.Delete(filename);

            try
            {
                using (var writer = new BinaryWriter(File.Open(filename, FileMode.Create)))
                {
                    foreach (var number in numbers)
                    {
                        var dataDouble = BitConverter.GetBytes(number.First);
                        var dataInt = BitConverter.GetBytes(number.Second);
                        writer.Write(dataDouble);
                        writer.Write(dataInt);
                    }

                    Length = numbers.Length;
                }

                Fs = new FileStream("array.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void InsertionSortArray()
        {
            for (var i = 1; i < Length; i++)
            {
                var j = i;
                var tmp = this[i];

                while (j > 0 && this[j - 1] > tmp)
                {
                    this[j] = this[j - 1];
                    j--;
                    Counter.Add(15);
                }

                this[j] = tmp;
                Counter.Add(5);
            }
        }

        public bool CheckSorted()
        {
            for (var i = 1; i < Length; i++)
                if (this[i - 1] > this[i])
                    return false;

            return true;
        }

        public void Dispose()
        {
            Fs.Close();
        }

        public void Print(int n)
        {
            if (n < 1)
                return;

            var last = Math.Min(n, Length);

            for (var i = 0; i < last; i++)
                Console.Write("\n{0}", this[i]);

            Console.WriteLine();
        }

        public void PrintFromTo(int from, int to)
        {
            var last = Math.Min(to, Length);

            for (var i = 0; i < last; i++)
                if (i >= from)
                    Console.Write("\n{0}", this[i]);

            Console.WriteLine();
        }
    }
}
