using IAS;
using Symulator_IAS.Examples;

namespace Symulator_IAS
{
    class Program : IAS_Codes
    {
        static void Main(string[] args)
        {
            Example_OS os = new Example_OS();

            os.Run();
        }
    }
}
