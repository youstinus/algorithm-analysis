using System;
using SortBase;

namespace MergeSortOP
{
    public interface IDataArray
    {
        int Length { get; }
        Numbers this[int index] { get; }
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
        private readonly Numbers[] _data;
        private int _counter;

        public int Length { get; }
        public Numbers this[int index] => _data[index];

        public DataArray(int n, int seed)
        {
            _data = new Numbers[n];
            Length = n;
            var rand = new Random(seed);

            for (var i = 0; i < Length; i++)
                _data[i] = new Numbers(rand.NextDouble(), rand.Next());

            Counter.Add(1847 + 2 + Length * 50);
        }

        public DataArray(Numbers[] numbers)
        {
            _data = numbers;
            Length = _data.Length;
        }
        
        public Numbers First()//4
        {
            _counter = 0;
            return Next();
        }

        public Numbers Next()//3
        {
            return _counter < Length ? _data[_counter++] : null;
        }

        public DataArray Head()
        {
            var length = (Length + 1) / 2;
            var head = new Numbers[length];

            for (var i = 0; i < length; i++)//length+1
                head[i] = this[i];//2length

            Counter.Add(3 + 1 + (length * 3 + 1) + 2);
            return new DataArray(head);//2
        }

        public DataArray Tail()
        {
            var length = (Length) / 2;//2
            var length2 = (Length + 1) / 2;//3
            var tail = new Numbers[length];//1
            var k = 0;//1

            for (var i = length2; i < Length; i++)//length-length2+1
                tail[k++] = this[i];//3(length-length2)

            Counter.Add(7 + (Length - length2) * 4 + 1 + 2);
            return new DataArray(tail);//2
        }

        public DataArray MergeSortArray()
        {
            if (Length <= 1) return this;

            var left = Head().MergeSortArray();
            var right = Tail().MergeSortArray();
            var merged = MergeArray(left, right);

            Counter.Add(5);
            return merged;
        }

        private static DataArray MergeArray(DataArray a, DataArray b)
        {
            var count = a.Length + b.Length;//3
            var numbers = new Numbers[count];//1
            var a1 = a.First();//5
            var b1 = b.First();//5

            for (var i = 0; i < count; i++)//count+1
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

                Counter.Add(22);
                numbers[i] = tmp;//2
            }

            Counter.Add(14+2+1);
            return new DataArray(numbers);//2
        }

        public bool CheckSorted()
        {
            for (var i = 1; i < Length; i++)
                if (this[i - 1] > this[i])
                    return false;

            return true;
        }

        public void Print(int n)
        {
            if (n < 1)
                return;

            for (var i = 0; i < n; i++)
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
