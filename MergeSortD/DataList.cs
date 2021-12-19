using System;
using System.IO;
using SortBase;

namespace MergeSortD
{
    public interface IDataList
    {
        int Start { get; set; }
        int End { get; set; }
        int Length { get; }
        FileStream Fs { get; set; }
        FileStream FsTmp { get; set; }
        void PopulateData(Numbers[] numbers);
        Numbers First();
        Numbers Next();
        DataList Head();
        DataList Tail();
        DataList MergeSortList();
        bool CheckSorted();
        void Print(int n);
        void PrintFromTo(int from, int to);
    }

    public class DataList : IDataList
    {
        private int _currentNode;
        private int _nextNode;
        private int _counter;

        public int Start { get; set; }
        public int End { get; set; }
        public int Length { get; }
        public FileStream Fs { get; set; }
        public FileStream FsTmp { get; set; }

        public DataList(int start, int end, DataList array)//6
        {
            Fs = array.Fs;
            FsTmp = array.FsTmp;
            Length = end - start;
            Start = start;
            End = end;
            Counter.Add(6);
        }

        public DataList(int n, int seed)
        {
            var rand = new Random(seed);
            var numbers = new Numbers[n];

            for (var i = 0; i < n; i++)
            {
                var dataDouble = rand.NextDouble();
                var dataInt = rand.Next();
                var numberPair = new Numbers(dataDouble, dataInt);
                numbers[i] = numberPair;
            }

            Length = n;
            PopulateData(numbers);
        }

        public DataList(Numbers[] numbers)
        {
            Length = numbers.Length;
            PopulateData(numbers);
        }

        public void PopulateData(Numbers[] numbers)
        {
            const string filename = "list.dat";
            const string filenameTmp = "tmpList.dat";

            if (File.Exists(filename))
                File.Delete(filename);

            try
            {
                using (var writer = new BinaryWriter(File.Open(filename, FileMode.Create)))
                {
                    writer.Write(4);
                    var counter = 1;

                    foreach (var num in numbers)
                    {
                        var dataDouble = BitConverter.GetBytes(num.First);
                        var dataInt = BitConverter.GetBytes(num.Second);
                        writer.Write(dataDouble);
                        writer.Write(dataInt);
                        writer.Write(counter * 16 + 4);
                        counter++;
                    }
                }

                Fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                FsTmp = new FileStream(filenameTmp, FileMode.Create, FileAccess.ReadWrite);
                Start = 0;
                End = Length;
            }
            catch (IOException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public Numbers First()//12
        {
            _counter = 1;
            var data = new byte[16];
            Fs.Seek(0, SeekOrigin.Begin);
            Fs.Read(data, 0, 4);
            _currentNode = BitConverter.ToInt32(data, 0);
            Fs.Seek(_currentNode, SeekOrigin.Begin);
            Fs.Read(data, 0, 16);
            var dataDouble = BitConverter.ToDouble(data, 0);
            var dataInt = BitConverter.ToInt32(data, 8);
            var result = new Numbers(dataDouble, dataInt);
            _nextNode = BitConverter.ToInt32(data, 12);

            for (var i = 0; i < Start; i++)//Start+1
                result = Next();//Start

            Counter.Add(12+Start+1);
            return result;
        }

        public Numbers Next()//12
        {
            if (_counter >= End) return null;

            var data = new byte[16];
            Fs.Seek(_nextNode, SeekOrigin.Begin);
            Fs.Read(data, 0, 16);
            _currentNode = _nextNode;
            var dataDouble = BitConverter.ToDouble(data, 0);
            var dataInt = BitConverter.ToInt32(data, 8);
            var result = new Numbers(dataDouble, dataInt);
            _nextNode = BitConverter.ToInt32(data, 12);
            _counter++;

            Counter.Add(12);
            return result;
        }

        public DataList Head()//7
        {
            var length = (Length + 1) / 2;
            var start = Start;
            var end = Start + length;

            Counter.Add(7);
            return new DataList(start, end, this);
        }

        public DataList Tail()//7
        {
            var length = (Length + 1) / 2;
            var start = Start + length;
            var end = End;

            Counter.Add(7);
            return new DataList(start, end, this);
        }

        public DataList MergeSortList()//5
        {
            if (Length <= 1) return this;

            var left = Head().MergeSortList();
            var right = Tail().MergeSortList();
            var merged = MergeList(left, right);

            Counter.Add(5);
            return merged;
        }

        private DataList MergeList(DataList a, DataList b)
        {
            var count = a.Length + b.Length;//3
            var start = a.Start;//1
            var end = b.End;//1

            var a1 = a.First();//1
            var b1 = b.First();//1

            for (var i = 0; i < count; i++)//count
            {
                Numbers tmp = null;//1
                if (a1 < b1)//10
                {
                    tmp = a1;//1
                    a1 = a.Next();//12
                }
                else if (a1 > b1)//10
                {
                    tmp = b1;//1
                    b1 = b.Next();//12
                }

                SaveTemporary(tmp, i);//6
                Counter.Add(36);
            }

            OverrideData(start, end);

            return new DataList(start, end, a);
        }

        private void SaveTemporary(Numbers tmp, int i)//6
        {
            var dataDouble = BitConverter.GetBytes(tmp.First);
            var dataInt = BitConverter.GetBytes(tmp.Second);
            FsTmp.Seek(12 * i, SeekOrigin.Begin);
            FsTmp.Write(dataDouble, 0, 8);
            FsTmp.Write(dataInt, 0, 4);
        }

        private void OverrideData(int start, int end)
        {
            var data = new byte[12];//1
            FsTmp.Seek(0, SeekOrigin.Begin);//1

            for (var i = start; i < end; i++)//end-start+1
            {
                FsTmp.Read(data, 0, 12);
                Fs.Seek(16 * i + 4, SeekOrigin.Begin);
                Fs.Write(data, 0, 12);
            }

            Counter.Add(2+(end-start+1)*4);
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

            var number = First();
            Console.Write("\n{0}", number);
            var last = Math.Min(n, Length);

            for (var i = 1; i < last; i++)
            {
                number = Next();
                Console.Write("\n{0}", number);
            }

            Console.WriteLine();
        }

        public void PrintFromTo(int from, int to)
        {
            var number = First();
            if (from <= 0)
                Console.Write("\n{0}", number);

            var last = Math.Min(to, Length);

            for (var i = 1; i < last; i++)
            {
                number = Next();

                if (i >= from)
                    Console.Write("\n{0}", number);
            }

            Console.WriteLine();
        }
    }
}
