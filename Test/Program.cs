using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            int MinValue = int.MinValue;

            long a = (uint) MinValue;
            Console.WriteLine(a);
            Console.WriteLine(int.MaxValue);
            Console.WriteLine(int.MinValue);
        }
    }
}
