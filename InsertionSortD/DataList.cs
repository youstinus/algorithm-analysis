using System;
using System.IO;
using SortBase;

namespace InsertionSortD
{
    public interface IDataList
    {
        int Length { get; }
        FileStream Fs { get; set; }
        Numbers First();
        Numbers Next();
        Numbers Prev();
        void Swap();
        void Write(Numbers numbers);
        void InsertionSortList();
        bool CheckSorted();
        void Print(int n);
        void PrintFromTo(int from, int to);
    }

    public class DataList : IDataList
    {
        private int _prevNode;
        private int _currentNode;
        private int _nextNode;

        private int _currentNode2;
        private int _nextNode2;

        private int _counter;

        public int Length { get; }
        public FileStream Fs { get; set; }

        public DataList(int n, int seed)
        {
            Length = n;
            const string filename = "list.dat";
            var rand = new Random(seed);

            if (File.Exists(filename))
                File.Delete(filename);

            try
            {
                using (var writer = new BinaryWriter(File.Open(filename, FileMode.Create)))
                {
                    writer.Write(4);

                    for (var i = 0; i < Length; i++)
                    {
                        writer.Write(i * 20 - 16); // pointer to previous
                        writer.Write(rand.NextDouble());
                        writer.Write(rand.Next());
                        writer.Write((i + 1) * 20 + 4); // pointer to next
                    }
                }

                Fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public DataList(Numbers[] numbers)
        {
            const string filename = "list.dat";

            if (File.Exists(filename))
                File.Delete(filename);

            try
            {
                using (var writer = new BinaryWriter(File.Open(filename, FileMode.Create)))
                {
                    writer.Write(4);

                    for (var i = 0; i < numbers.Length; i++)
                    {
                        var dataDouble = BitConverter.GetBytes(numbers[i].First);
                        var dataInt = BitConverter.GetBytes(numbers[i].Second);

                        writer.Write(i * 20 - 16); // pointer to previous
                        writer.Write(dataDouble);
                        writer.Write(dataInt);
                        writer.Write((i + 1) * 20 + 4); // pointer to next
                    }

                    Length = numbers.Length;
                }

                Fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public Numbers First()//17
        {
            _counter = 0; // what initial counter
            var data = new byte[20];
            Fs.Seek(0, SeekOrigin.Begin);
            Fs.Read(data, 0, 4);
            _currentNode = BitConverter.ToInt32(data, 0);

            _currentNode2 = _currentNode;
            _nextNode2 = _currentNode;

            Fs.Seek(_currentNode, SeekOrigin.Begin);
            Fs.Read(data, 0, 20);

            _prevNode = BitConverter.ToInt32(data, 0);
            var dataDouble = BitConverter.ToDouble(data, 4);
            var dataInt = BitConverter.ToInt32(data, 12);
            var result = new Numbers(dataDouble, dataInt);
            _nextNode = BitConverter.ToInt32(data, 16);

            Counter.Add(17);
            return result;
        }

        public Numbers Next()//17
        {
            if (_counter >= Length - 1)
                return null;

            var data = new byte[20];
            Fs.Seek(_nextNode, SeekOrigin.Begin);
            Fs.Read(data, 0, 20);
            _currentNode = _nextNode;

            _currentNode2 = _currentNode;
            _nextNode2 = _currentNode;

            _prevNode = BitConverter.ToInt32(data, 0);
            var dataDouble = BitConverter.ToDouble(data, 4);
            var dataInt = BitConverter.ToInt32(data, 12);
            var result = new Numbers(dataDouble, dataInt);
            _nextNode = BitConverter.ToInt32(data, 16);

            Counter.Add(17);
            _counter++;
            return result;
        }

        public Numbers Prev()//12
        {
            if (_prevNode <= 0)
                return null;

            var data = new byte[20];
            Fs.Seek(_prevNode, SeekOrigin.Begin);
            Fs.Read(data, 0, 20);
            _currentNode2 = _prevNode;

            _prevNode = BitConverter.ToInt32(data, 0);
            var dataDouble = BitConverter.ToDouble(data, 4);
            var dataInt = BitConverter.ToInt32(data, 12);
            var result = new Numbers(dataDouble, dataInt);
            _nextNode2 = BitConverter.ToInt32(data, 16);

            Counter.Add(12);
            return result;
        }

        public void Swap()//12
        {
            var data1 = new byte[12];
            var data2 = new byte[12];
            Fs.Seek(_nextNode2 + 4, SeekOrigin.Begin);
            Fs.Read(data1, 0, 12);
            Fs.Seek(_currentNode2 + 4, SeekOrigin.Begin);
            Fs.Read(data2, 0, 12);
            Fs.Seek(_currentNode2 + 4, SeekOrigin.Begin);
            Fs.Write(data1, 0, 12);
            Fs.Seek(_nextNode2 + 4, SeekOrigin.Begin);
            Fs.Write(data2, 0, 12);
            _nextNode2 = _currentNode2;

            Counter.Add(12);
        }

        public void Write(Numbers numbers)//6
        {
            Fs.Seek(_nextNode2 + 4, SeekOrigin.Begin);
            var doubleData = BitConverter.GetBytes(numbers.First);
            var intData = BitConverter.GetBytes(numbers.Second);
            Fs.Write(doubleData, 0, 8);
            Fs.Write(intData, 0, 4);
            Counter.Add(6);
        }

        public void InsertionSortList()
        {
            var first = First();//1
            for (var current = first; current != null; current = Next())
            {
                var tmp = current;
                var prev = Prev();

                while (prev != null && prev > tmp)//10
                {
                    Swap();
                    prev = Prev();
                    Counter.Add(15);
                }

                Counter.Add(5);
                Write(tmp);
            }
            Counter.Add(3);
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
            {
                Console.Write("\n{0}", number);
            }

            var last = Math.Min(to, Length);

            for (var i = 1; i < last; i++)
            {
                number = Next();
                if (i >= from)
                {
                    Console.Write("\n{0}", number);
                }
            }

            Console.WriteLine();
        }
    }
}
