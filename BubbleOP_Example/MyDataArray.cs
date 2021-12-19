using System;

namespace BubbleOP_Example
{
    class MyDataArray : DataArray

    {
        double[] data;
        public MyDataArray(int n, int seed)
        {
            data = new double[n];
            length = n;
            var rand = new Random(seed);
            for (var i = 0; i < length; i++)
            {
                data[i] = rand.NextDouble();
            }
        }
        public override double this[int index]
        {
            get { return data[index]; }
        }
        public override void Swap(int j, double a, double b)
        {
            data[j - 1] = a;
            data[j] = b;
        }
    }
}
