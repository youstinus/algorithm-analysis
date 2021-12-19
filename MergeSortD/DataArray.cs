using System;
using System.IO;
using SortBase;

namespace MergeSortD
{
    public interface IDataArray
    {
        int Start { get; set; }
        int End { get; set; }
        int Length { get; }
        FileStream Fs { get; set; }
        FileStream FsTmp { get; set; }
        void PopulateData(Numbers[] numbers);
        Numbers First();
        Numbers Next();
        DataArray Head();
        DataArray Tail();
        DataArray MergeSortArray();
        bool CheckSorted();
        void Print(int n);
        void PrintFromTo(int from, int to);
    }

    public class DataArray : IDataArray
    {
        private int _counter;

        public int Start { get; set; }
        public int End { get; set; }
        public int Length { get; }
        public FileStream Fs { get; set; }
        public FileStream FsTmp { get; set; }

        private Numbers this[int index]
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
            FsTmp = new FileStream("tmpArray.dat", FileMode.Create, FileAccess.ReadWrite);

            Length = n;
            Start = 0;
            End = n;
            var rand = new Random(seed);

            for (var i = 0; i < Length; i++)
                this[i] = new Numbers(rand.NextDouble(), rand.Next());
        }

        public DataArray(int start, int end, DataArray array)//6
        {
            Fs = array.Fs;
            FsTmp = array.FsTmp;
            Length = end - start;
            Start = start;
            End = end;

            Counter.Add(6);
        }

        public DataArray(Numbers[] numbers)
        {
            Length = numbers.Length;
            PopulateData(numbers);
        }

        public void PopulateData(Numbers[] numbers)
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
                }

                Fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                FsTmp = new FileStream("tmpArray.dat", FileMode.Create, FileAccess.ReadWrite);
                Start = 0;
                End = Length;
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public Numbers First()//2
        {
            _counter = Start;
            return Next();
        }

        public Numbers Next()//3
        {
            return _counter < End ? this[_counter++] : null;
        }

        public DataArray Head()//7
        {
            var length = (Length + 1) / 2;//3
            var start = Start;//1
            var end = Start + length;//2

            Counter.Add(7);
            return new DataArray(start, end, this);
        }

        public DataArray Tail()//7
        {
            var length = (Length + 1) / 2;
            var start = Start + length;
            var end = End;

            Counter.Add(7);
            return new DataArray(start, end, this);
        }

        public DataArray MergeSortArray()//5
        {
            if (Length <= 1) return this;

            var left = Head().MergeSortArray();
            var right = Tail().MergeSortArray();
            var merged = MergeArray(left, right);

            Counter.Add(5);
            return merged;
        }

        private DataArray MergeArray(DataArray a, DataArray b)
        {
            var count = a.Length + b.Length;//3
            var start = a.Start;//1
            var end = b.End;//1

            var a1 = a.First();//3
            var b1 = b.First();//3

            for (var i = 0; i < count; i++)//count
            {
                Numbers tmp = null;//1
                if (a1 < b1)//10
                {
                    tmp = a1;//1
                    a1 = a.Next();//4
                }
                else if (a1 > b1)//10
                {
                    tmp = b1;//1
                    b1 = b.Next();//4
                }

                SaveTemporary(tmp, i);//6
                Counter.Add(28);
            }

            Override(start, end);

            return new DataArray(start, end, a);
        }

        private void SaveTemporary(Numbers tmp, int i)//6
        {
            var dataDouble = BitConverter.GetBytes(tmp.First);
            var dataInt = BitConverter.GetBytes(tmp.Second);
            FsTmp.Seek(12 * i, SeekOrigin.Begin);
            FsTmp.Write(dataDouble, 0, 8);
            FsTmp.Write(dataInt, 0, 4);
        }

        private void Override(int start, int end)//7
        {
            var count = end - start;
            var data = new byte[12 * count];
            FsTmp.Seek(0, SeekOrigin.Begin);
            FsTmp.Read(data, 0, 12 * count);
            Fs.Seek(12 * start, SeekOrigin.Begin);
            Fs.Write(data, 0, 12 * count);

            Counter.Add(7);
        }

        public bool CheckSorted()
        {
            var tmp = First();

            for (var i = 1; i < Length; i++)
            {
                var next = Next();
                if (tmp > next)
                    return false;

                tmp = next;
            }

            return true;
        }
        public void Dispose()
        {
            Fs.Close();
            FsTmp.Close();
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
