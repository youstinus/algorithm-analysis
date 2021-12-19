using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HashTableSearchD
{
    class Program
    {
        private static Random _random = new Random();
        public static void Main(string[] args)
        {
            Console.WriteLine("\n File name or leave blank to generate:");
            var file = Console.ReadLine();

            var keys = new List<string>();
            var values = new List<string>();

            if (!string.IsNullOrWhiteSpace(file))
            {
                ReadFile(file, keys, values);
                Console.WriteLine(" 1048576 Data uploaded from file");
            }
            else
            {
                keys = GetStringList(1048576);
                values = GetStringList(1048576);
                Console.WriteLine(" 1048576 Data generated");
            }

            Console.WriteLine("\n Manual testing? [y]: ");
            var line = Console.ReadLine();

            if (line == "y")
            {
                ManualTesting(keys, values);
            }
            else
            {
                AutoTesting(keys, values);
            }

            Console.WriteLine("\n --=< Done >=--");
            Console.ReadKey();
        }

        private static void AutoTesting(List<string> keys, List<string> values)
        {
            for (var i = 4; i < 21; i++)
            {
                var count = Convert.ToInt32(Math.Pow(2, i));
                Console.WriteLine($"\n\n Starting tests with {count}");
                StartHashTable(count, keys, values);
            }
        }

        private static void ManualTesting(List<string> keys, List<string> values)
        {
            Console.WriteLine("\n How many strings: [1-1048576]");
            var n = Convert.ToInt32(Console.ReadLine() ?? "1024");
            StartHashTable(n, keys, values);
        }

        private static void StartHashTable(int n, List<string> keys, List<string> values)
        {
            var table = new HashTable(n * 3);
            Console.WriteLine(" Creating table");

            for (var i = 0; i < n; i++)
            {
                table.Add(keys[i], values[i]);
            }
            
            Console.WriteLine($" Table created. Table count: {table.GetElementCount()}.");
            //Shuffle(keys);
            var startTime = DateTime.Now.Ticks;

            for (var i = 0; i < n; i++)
            {
                var found = table.Get(keys[i]);
                if (string.IsNullOrWhiteSpace(found))
                {
                    //throw new InvalidOperationException(" Element not found");
                    Console.Write("#");
                }
            }

            Console.WriteLine(" All values found.");
            var endTime = DateTime.Now.Ticks;

            var duration = endTime - startTime;
            Console.WriteLine(" Elapsed time " + new TimeSpan(duration).Milliseconds + "ms");
            Console.WriteLine($" Table size: {table.GetTableSize()}. Element count: {table.GetElementCount()}. Chain count: {table.GetChainsCount()}.");
            Console.WriteLine($" Average chain size:\n {table.AverageChainSize()}");
            table.Dispose();
        }

        public static List<string> GetStringList(int count)
        {
            var strings = new List<string>();

            for (var i = 0; i < count; i++)
            {
                strings.Add(GetUniqueString2(12));
                //strings.Add(GetUniqueString(12));
            }

            return strings;
        }

        public static string GetUniqueString(int size)
        {
            var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            var data = new byte[size];
            using (var crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }
            var result = new StringBuilder(size);
            foreach (var b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }

        public static string GetUniqueString2(int size)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[size];

            for (var i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[_random.Next(chars.Length)];
            }

            return new string(stringChars);
        }

        public static void Shuffle(IList<string> list)
        {
            var rng = new Random();
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        private static void ReadFile(string path, List<string> keys, List<string> values)
        {
            if (path.EndsWith(".txt"))
                ReadTextFile(path, keys, values);

            if (!File.Exists(path))
                throw new InvalidOperationException(" File not found");

            using (var reader = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                var buffer = new char[12];
                while (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    reader.Read(buffer, 0, 12);
                    keys.Add(new string(buffer));
                    reader.Read(buffer, 0, 12);
                    values.Add(new string(buffer));
                }
            }
        }

        private static void ReadTextFile(string path, List<string> keys, List<string> values)
        {
            using (var reader = new StreamReader(File.Open(path, FileMode.Open)))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(' ');
                    keys.Add(parts[0]);
                    values.Add(parts[1]);
                }
            }
        }
    }
}
