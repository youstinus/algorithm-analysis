using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using SortBase;

namespace HashTableSearchD
{
    public class HashTable
    {
        private readonly SHA256 _sha;
        private readonly int _length;
        private int _freePointer;
        private int _itemCount;
        private int _chainCount;
        private readonly FileStream _fs;

        public HashTable(int size)
        {
            _length = size;
            _freePointer = _length;
            _sha = SHA256.Create();
            _fs = new FileStream("table.dat", FileMode.Create, FileAccess.ReadWrite);
        }
       
        public void Add(string key, string value)
        {
            if (value == null || key == null)
                throw new InvalidOperationException("Key or value is null in put(Key key, Value value)");

            var position = GetPosition(key);//12
            _fs.Seek(position * 28, SeekOrigin.Begin);
            var pointer = new byte[4];
            var byteKey = new byte[12];
            var byteValue = new byte[12];
            _fs.Read(byteKey, 0, 12);
            _fs.Read(byteValue, 0, 12);
            _fs.Read(pointer, 0, 4);
            var currentKey = Encoding.Default.GetString(byteKey, 0, 12).Replace("\u0000", "");
            var currentValue = Encoding.Default.GetString(byteValue, 0, 12).Replace("\u0000", "");
            var nextPointer = BitConverter.ToInt32(pointer, 0);

            if (string.IsNullOrWhiteSpace(currentKey))
            {
                _fs.Seek(position * 28, SeekOrigin.Begin);
                _fs.Write(Encoding.ASCII.GetBytes(key), 0, 12);
                _fs.Write(Encoding.ASCII.GetBytes(value), 0, 12);
                Counter.Add(6);
            }
            else if (!string.IsNullOrWhiteSpace(currentKey) && nextPointer <= 0)
            {
                _fs.Seek(position * 28 + 24, SeekOrigin.Begin);
                _fs.Write(BitConverter.GetBytes(_freePointer), 0, 4);
                _fs.Seek(_freePointer * 28, SeekOrigin.Begin);
                _fs.Write(Encoding.ASCII.GetBytes(key), 0, 12);
                _fs.Write(Encoding.ASCII.GetBytes(value), 0, 12);
                _freePointer++;
                Counter.Add(11);
            }
            else if (!string.IsNullOrWhiteSpace(currentKey) && nextPointer > 0)
            {
                var bytu = new byte[4];
                var tmp = nextPointer;
                var pointer1 = nextPointer;
                while (pointer1 > 0)
                {
                    _fs.Seek(pointer1 * 28 + 24, SeekOrigin.Begin);
                    _fs.Read(bytu, 0, 4);
                    tmp = pointer1;
                    pointer1 = BitConverter.ToInt32(bytu, 0);
                    Counter.Add(6);
                }
                _fs.Seek(tmp * 28 + 24, SeekOrigin.Begin);
                _fs.Write(BitConverter.GetBytes(_freePointer), 0, 4);
                _fs.Seek(_freePointer * 28, SeekOrigin.Begin);
                _fs.Write(Encoding.ASCII.GetBytes(key), 0, 12);
                _fs.Write(Encoding.ASCII.GetBytes(value), 0, 12);
                _freePointer++;
                Counter.Add(14);
            }

            Counter.Add(30);
            _itemCount++;
        }

        public string Get(string key)
        {
            var pointer = GetPosition(key);//12
            _fs.Seek(pointer * 28, SeekOrigin.Begin);
            var bytes = new byte[28];

            var pointer2 = new byte[4];
            var byteKey = new byte[12];
            var byteValue = new byte[12];
            _fs.Read(byteKey, 0, 12);
            _fs.Read(byteValue, 0, 12);
            _fs.Read(pointer2, 0, 4);

            var currentKey = Encoding.Default.GetString(byteKey, 0, 12).Replace("\u0000", "");
            var currentValue = Encoding.Default.GetString(byteValue, 0, 12).Replace("\u0000", "");
            var nextPointer = BitConverter.ToInt32(pointer2, 0);

            Counter.Add(30);
            if (!string.IsNullOrWhiteSpace(currentKey) && currentKey == key)
            {
                return currentValue;
            }

            while (nextPointer > 0)
            {
                _fs.Seek(nextPointer * 28, SeekOrigin.Begin);
                pointer2 = new byte[4];
                byteKey = new byte[12];
                byteValue = new byte[12];
                _fs.Read(byteKey, 0, 12);
                _fs.Read(byteValue, 0, 12);
                _fs.Read(pointer2, 0, 4);

                currentKey = Encoding.Default.GetString(byteKey, 0, 12).Replace("\u0000", "");
                currentValue = Encoding.Default.GetString(byteValue, 0, 12).Replace("\u0000", "");
                nextPointer = BitConverter.ToInt32(pointer2, 0);
                
                Counter.Add(17);
                if (!string.IsNullOrWhiteSpace(currentKey) && currentKey == key)
                {
                    return currentValue;
                }
            }

            return null;
        }

        public void Dispose()
        {
            _fs.Close();
            _fs.Dispose();
        }
        
        private int GetPosition(string key)
        {
            var position = Hash2(key) % _length;
            return Math.Abs(position);
        }

        private int Hash(string value)
        {
            return value.GetHashCode();
        }

        private int Hash2(string value)
        {
            var hashed = _sha.ComputeHash(Encoding.ASCII.GetBytes(value));
            return BitConverter.ToInt32(hashed, 0);
        }

        public int GetChainsCount()
        {
            var count = 0;
            _fs.Seek(0, SeekOrigin.Begin);
            var bytes = new byte[12];
            for (var i = 0; i < _length; i++)
            {
                _fs.Read(bytes, 0, 12);
                var currentKey = Encoding.Default.GetString(bytes, 0, 12).Replace("\u0000", "");
                if (!string.IsNullOrWhiteSpace(currentKey))
                {
                    count++;
                }
            }

            _chainCount = count;
            return count;
        }

        public double AverageChainSize()
        {
            return _itemCount * 1.0 / _chainCount;
        }

        public int GetElementCount()
        {
            return _itemCount;
        }

        public int GetTableSize()
        {
            return _length;
        }
    }
}