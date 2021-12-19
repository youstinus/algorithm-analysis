using System;
using SortBase;

namespace InsertionSortOP
{
    public interface IDataList
    {
        int Length { get; }
        DataList.LinkedListNodeCustom First();
        DataList.LinkedListNodeCustom Next();
        void InsertionSortList();
        bool CheckSorted();
        void Print(int n);
        void PrintFromTo(int from, int to);
    }

    public class DataList : IDataList
    {
        private readonly LinkedListNodeCustom _headNode;
        private LinkedListNodeCustom _currentNode;

        public int Length { get; }

        public DataList(int n, int seed)
        {
            Length = n;
            var rand = new Random(seed);
            var first = rand.NextDouble();
            var second = rand.Next();
            var numbers = new Numbers(first, second);
            _headNode = new LinkedListNodeCustom(numbers);
            _currentNode = _headNode;

            for (var i = 1; i < Length; i++)
            {
                first = rand.NextDouble();
                second = rand.Next();
                numbers = new Numbers(first, second);
                var newNode = new LinkedListNodeCustom(numbers);
                newNode.PrevNode = _currentNode;
                _currentNode.NextNode = newNode;
                _currentNode = _currentNode.NextNode;
            }
        }

        public DataList(Numbers[] numbersList)
        {
            Length = numbersList.Length;
            var numbers = numbersList[0];
            _headNode = new LinkedListNodeCustom(numbers);
            _currentNode = _headNode;

            for (var i = 1; i < numbersList.Length; i++)
            {
                numbers = numbersList[i];
                var newNode = new LinkedListNodeCustom(numbers);
                newNode.PrevNode = _currentNode;
                _currentNode.NextNode = newNode;
                _currentNode = _currentNode.NextNode;
            }
        }

        public LinkedListNodeCustom First()//2
        {
            _currentNode = _headNode;
            return _currentNode;
        }

        public LinkedListNodeCustom Next()//2
        {
            _currentNode = _currentNode.NextNode;
            return _currentNode;
        }

        public void InsertionSortList()
        {
            var first = First();//2
            for (var current = first; current != null; current = current.NextNode)
            {
                var node = current;
                var tmp = current.Numbers;

                while (node.PrevNode != null && node.PrevNode.Numbers > tmp)
                {
                    node.Numbers = node.PrevNode.Numbers;
                    node = node.PrevNode;
                    Counter.Add(15);
                }

                node.Numbers = tmp;
                Counter.Add(5);
            }
            Counter.Add(2);
        }

        public bool CheckSorted()
        {
            var tmp = First();

            for (var i = 1; i < Length; i++)
            {
                var next = Next();
                if (tmp.Numbers > next.Numbers)
                    return false;

                tmp = next;
            }

            return true;
        }

        public void Print(int n)
        {
            if (n < 1)
                return;

            var number = First();
            Console.Write("\n{0}", number.Numbers);
            var last = Math.Min(n, Length);

            for (var i = 1; i < last; i++)
            {
                number = Next();
                Console.Write("\n{0}", number.Numbers);
            }

            Console.WriteLine();
        }

        public void PrintFromTo(int from, int to)
        {
            var number = First();

            if (from <= 0)
                Console.Write("\n{0}", number.Numbers);

            var last = Math.Min(to, Length);

            for (var i = 1; i < last; i++)
            {
                number = Next();

                if (i >= from)
                    Console.Write("\n{0}", number.Numbers);
            }

            Console.WriteLine();
        }

        public class LinkedListNodeCustom
        {
            public LinkedListNodeCustom NextNode { get; set; }
            public LinkedListNodeCustom PrevNode { get; set; }
            public Numbers Numbers { get; set; }
            public LinkedListNodeCustom(Numbers numbers)
            {
                Numbers = numbers;
            }
        }
    }
}
