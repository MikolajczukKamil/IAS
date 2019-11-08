using System;
using Symulator;

namespace Symulator_IAS
{
    class Program : OptCodes
    {
        static void Main(string[] args)
        {
            int x = 0, y= 0, n = 4, i;

            for (i = 0; i < n; i++) {
                x += i;
            }

            for (i = 0; i <= n;) {
                i++;
                y += i;
            }

            Console.WriteLine($"Wynik Prawidłowy: {x}");
            Console.WriteLine($"Wynik nie prawidłowy: {y}");

            ulong[] instructions = Zad1Poprawione();
            

            IAS a = new IAS(instructions);

            a.ManualJumpTo(4); // zaczynamy od 4

            Console.WriteLine(a.ToString(4));

            while(Console.ReadKey().KeyChar != 'a')
            {
                a.Step();

                Console.WriteLine(a.ToString(4));
            }
        }

        static ulong[] Zad1Poprawione() => new ulong[] 
        {
            Word(4), // <>                  // 0 N
            Word(1),                        // 1 ++
            Word(0),                        // 2 i
            Word(0),                        // 3 X
            Word(
                Instruction(LOAD_M, 0),     // 4L
                Instruction(SUB_M, 2)       // 4R
            ),
            Word(
                Instruction(JUMP_P_M_L, 11), // 5L w lini 11 adres do skoku do 6
                Instruction(JUMP_M_R, 10)  // 5R w lini 10 adres do skoku do 5 czyli oo loop
            ),
            Word(
                Instruction(LOAD_M, 2),     // 6L
                Instruction(ADD_M, 1)       // 6R
            ),
            Word(
                Instruction(STOR_M, 2),     // 7L
                Instruction(ADD_M, 3)       // 7R
            ),
            Word(
                Instruction(STOR_M, 3),     // 8L
                Instruction(JUMP_M_L, 9)    // 8R w 9 lini aders do skoku do 4
            ),

            Word(4),                        // 9 skok do 4
            Word(5),                        // 10 skok do 5
            Word(6),                        // 11 skok do 6
        };
    }
}
