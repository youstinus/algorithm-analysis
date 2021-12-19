using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using SortBase;

namespace HashTableSearchOP
{
    public class HashTable
    {
        private readonly DataList[] _table;
        private readonly int _length;
        private int _itemCount;
        private readonly SHA256 _sha;

        public HashTable(int size)
        {
            _length = size;
            _table = new DataList[size];
            _sha = SHA256.Create();
        }

        public int GetTableSize()
        {
            return _table.Length;
        }

        public int GetElementCount()
        {
            return _itemCount;
        }

        public int GetUsedSpacesCount()
        {
            return _table.Count(node => node != null);
        }

        public string MoreThanOneItemHash()
        {
            var node = _table.First(x => x != null && x.GetChainCount() > 1);
            return node.PrintOutChain();
        }

        public string Get(string key)
        {
            var position = GetPosition(key);//10
            var list = GetList(position);//10
            var current = list.First();//2
            Counter.Add(22);

            while (current != null)
            {
                if (current.GetKey() == key)
                    return current.GetValue();

                current = list.Next();
                Counter.Add(5);
            }

            return null;
        }

        public void Add(string key, string value)
        {
            if (value == null || key == null)//2
                throw new InvalidOperationException("Key or value is null in put(Key key, Value value)");

            var position = GetPosition(key);//12
            var list = GetList(position);//5
            list.Add(key, value);//12
            _itemCount++;//1
            Counter.Add(30);
        }

        public void Update(string key, string value)
        {
            if (value == null || key == null)
                throw new InvalidOperationException("Key or value is null in put(Key key, Value value)");

            var position = GetPosition(key);
            var list = GetList(position);
            var updated = list.Update(key, value);

            if (!updated)
                _itemCount++;
        }

        public string Delete(string key)
        {
            var position = GetPosition(key);
            var list = GetList(position);
            return list.Delete(key);
        }

        private int GetPosition(string key)//12
        {
            var position = Hash2(key) % _length;
            return Math.Abs(position);
        }

        private DataList GetList(int position)//5
        {
            var list = _table[position];
            if (list != null)
                return list;

            list = new DataList();
            _table[position] = list;
            return list;
        }

        private int Hash(string key)
        {
            return key.GetHashCode();
        }

        private int Hash2(string value)//5
        {
            var hashed = _sha.ComputeHash(Encoding.UTF8.GetBytes(value));
            return BitConverter.ToInt32(hashed, 0);
        }
    }
}