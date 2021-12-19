using System;
using System.IO;

namespace BubbleD_Example
{
    class MyFileArray : DataArray
    {
        public MyFileArray(string filename, int n, int seed)
        {
            var data = new double[n];
            length = n;
            var rand = new Random(seed);
            for (var i = 0; i < length; i++)
            {
                data[i] = rand.NextDouble();
            }

            if (File.Exists(filename)) File.Delete(filename);
            try
            {
                using (var writer = new BinaryWriter(File.Open(filename,
                    FileMode.Create)))
                {
                    for (var j = 0; j < length; j++)
                        writer.Write(data[j]);
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public FileStream fs { get; set; }

        public override double this[int index]
        {
            get
            {
                var data = new byte[8];
                fs.Seek(8 * index, SeekOrigin.Begin);
                fs.Read(data, 0, 8);
                var result = BitConverter.ToDouble(data, 0);
                return result;
            }
        }

        public override void Swap(int j, double a, double b)
        {
            var data = new byte[16];
            BitConverter.GetBytes(a).CopyTo(data, 0);
            BitConverter.GetBytes(b).CopyTo(data, 8);
            fs.Seek(8 * (j - 1), SeekOrigin.Begin);
            fs.Write(data, 0, 16);
        }
    }
}
