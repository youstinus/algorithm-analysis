using System;

namespace Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = new Test();
            test.StartTests();
            test.CloseFile();
            Console.ReadKey();
        }
    }
}
