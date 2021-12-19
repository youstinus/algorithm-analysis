using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace StringGenerator
{
    class Program
    {
        private static Random _random = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);

        static void Main(string[] args)
        {
            Console.WriteLine(" String file Generator" +
                              "\n Creates <name>.txt text file." +
                              "\n Creates <name>.dat binary file." +
                              "\n Text file contains <Number> lines with <string> <string> separated by space." +
                              "\n Binary file contains <Number> key value pairs of <string><string>");
            Console.Write(" Number: ");
            var count = Convert.ToInt32(Console.ReadLine());
            Console.Write(" Name without extension: ");
            var name = Console.ReadLine();

            GenerateBinaryFile(count, name);
            GenerateTxtFile(count, name);

            Console.WriteLine($"\n Generated {name}.txt and {name}.dat");
            Console.WriteLine(" --=< Done >=--");
        }
        
        static void GenerateTxtFile(int count, string path)
        {
            using (var writer = new StreamWriter(File.Open(path + ".txt", FileMode.Create), Encoding.ASCII))
            {
                for (var i = 0; i < count; i++)
                {
                    writer.WriteLine("{0} {1}", GetUniqueString2(12), GetUniqueString2(12));
                }
            }
        }

        static void GenerateBinaryFile(int count, string path)
        {
            using (var writer = new BinaryWriter(File.Open(path + ".dat", FileMode.Create)))
            {
                for (var j = 0; j < count; j++)
                {
                    var value = GetUniqueString2(24);
                    writer.Write(value);
                }
            }
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
    }
}
