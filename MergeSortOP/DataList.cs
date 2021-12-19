using System;
using SortBase;

namespace MergeSortOP
{
    public interface IDataList
    {
        int Length { get; }
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
                _currentNode.NextNode = new LinkedListNodeCustom(numbers);
                _currentNode = _currentNode.NextNode;
            }
        }

        public DataList(Numbers[] numbersList)//5+4*Length
        {
            Length = numbersList.Length;//1
            var numbers = numbersList[0];//1

            _headNode = new LinkedListNodeCustom(numbers);//1
            _currentNode = _headNode;//1

            for (var i = 1; i < numbersList.Length; i++)//Length
            {
                numbers = numbersList[i];//1

                _currentNode.NextNode = new LinkedListNodeCustom(numbers);//1
                _currentNode = _currentNode.NextNode;//1
            }

            _currentNode.NextNode = null;
            Counter.Add(4+4*Length+1);
        }

        public Numbers First()//2
        {
            _currentNode = _headNode;
            return _currentNode.Numbers;
        }

        public Numbers Next()//3
        {
            _currentNode = _currentNode.NextNode;
            return _currentNode?.Numbers;
        }

        public DataList Head()
        {
            var length = (Length + 1) / 2;//3
            var head = new Numbers[length];//1
            head[0] = First();//2

            for (var i = 1; i < length; i++)//length
                head[i] = Next();//4length

            Counter.Add(6+5*length+1);
            return new DataList(head);//5+4*Length
        }

        public DataList Tail()
        {
            var length = Length - (Length + 1) / 2;//5
            var tail = new Numbers[length];//1
            tail[0] = Next();//3

            for (var i = 1; i < length; i++)//length
                tail[i] = Next();//4

            Counter.Add(10+5*length);
            return new DataList(tail);//5+4*Length
        }

        public DataList MergeSortList()
        {
            if (Length <= 1) return this;//2

            var left = Head().MergeSortList();
            var right = Tail().MergeSortList();
            var merged = MergeList(left, right);

            Counter.Add(5);
            return merged;
        }

        private static DataList MergeList(DataList a, DataList b)
        {
            var count = a.Length + b.Length;//3
            var numbers = new Numbers[count];//1
            var a1 = a.First();//5
            var b1 = b.First();//5
            
            for (var i = 0; i < numbers.Length; i++)//count+1
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

            Counter.Add();
            return new DataList(numbers);//5+4*Length
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

        private class LinkedListNodeCustom
        {
            public LinkedListNodeCustom NextNode { get; set; }
            public Numbers Numbers { get; }
            public LinkedListNodeCustom(Numbers numbers)
            {
                Numbers = numbers;
            }
        }
    }
}
