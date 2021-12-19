using System;
using System.IO;
using System.Text;

namespace NumberPairGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var seed = (int)DateTime.Now.Ticks & 0x0000FFFF;
            
            Console.WriteLine(" Data Generator" +
                              "\n Creates <name>.txt text file." +
                              "\n Creates <name>.dat binary file." +
                              "\n Text file contains <Number> lines with <double> <int> separated by space." +
                              "\n Binary file contains <Number> pairs of <double><int> 12 bytes each.");
            Console.Write(" Number: ");
            var count = Convert.ToInt32(Console.ReadLine());
            Console.Write(" Name without extension: ");
            var name = Console.ReadLine();

            GenerateBinaryFile(count, name, seed);
            GenerateTxtFile(count, name, seed);

            Console.WriteLine($"\n Generated {name}.txt and {name}.dat");
            Console.WriteLine(" --=< Done >=--");
            //Console.ReadKey();
        }

        static void GenerateTxtFile(int count, string path, int seed)
        {
            var random = new Random(seed);
            using (var writer = new StreamWriter(File.Open(path + ".txt", FileMode.Create), Encoding.ASCII))
            {
                for (var i = 0; i < count; i++)
                {
                    writer.WriteLine("{0} {1}", random.NextDouble(), random.Next());
                }
            }
        }

        static void GenerateBinaryFile(int count, string path, int seed)
        {
            var random = new Random(seed);
            using (var writer = new BinaryWriter(File.Open(path + ".dat", FileMode.Create)))
            {
                for (var j = 0; j < count; j++)
                {
                    writer.Write(random.NextDouble());
                    writer.Write(random.Next());
                }
            }
        }
    }
}
