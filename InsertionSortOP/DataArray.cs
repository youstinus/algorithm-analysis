using System;
using SortBase;

namespace InsertionSortOP
{
    public interface IDataArray
    {
        int Length { get; }
        Numbers this[int index] { get; set; }
        void InsertionSortArray();
        bool CheckSorted();
        void Print(int n);
        void PrintFromTo(int from, int to);
    }

    public class DataArray : IDataArray
    {
        private readonly Numbers[] _data;

        public int Length { get; }

        public Numbers this[int index]
        {
            get => _data[index];
            set => _data[index] = value;
        }

        public DataArray(int n, int seed)
        {
            _data = new Numbers[n];
            var rand = new Random(seed);
            Length = n;

            for (var i = 0; i < Length; i++)
            {
                _data[i] = new Numbers(rand.NextDouble(), rand.Next());
            }
        }

        public DataArray(Numbers[] numbers)
        {
            _data = numbers;
            Length = _data.Length;
        }

        public void InsertionSortArray()
        {
            for (var i = 1; i < Length; i++)
            {
                var j = i;
                var tmp = this[i];

                while (j > 0 && this[j - 1] > tmp)//10
                {
                    this[j] = this[j - 1];
                    j--;
                    Counter.Add(15);
                }

                this[j] = tmp;
                Counter.Add(4);
            }
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
