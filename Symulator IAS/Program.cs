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
                x += i; // 0 + 1 + 2 + 3 = 6
            }

            for (i = 0; i <= n;) {
                i++;
                y += i; // 1 + 2 + 3 + 4 + 5 = 15
            }

            Console.WriteLine($"Wynik Prawidłowy: {x}");
            Console.WriteLine($"Wynik nie prawidłowy: {y}");

            IAS a = new IAS(Zad1Poprawne());

            a.ManualJumpTo(4); // zaczynamy od 4

            Console.WriteLine(a.ToString(4));

            while(Console.ReadKey().KeyChar != 'a')
            {
                a.Step();

                Console.WriteLine(a.ToString(4));
            }
        }

        static ulong[] Zad1Dzialajace() => new ulong[] 
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
                Instruction(JUMP_P_M_L, 11),// 5L w lini 11 adres do skoku do 6
                Instruction(JUMP_M_R, 10)   // 5R w lini 10 adres do skoku do 5 czyli oo loop
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

        static ulong[] Zad1PoprawioneZNowymiSkokami() => new ulong[]
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
                Instruction(JUMP_P_L, 6),   // 5L
                Instruction(JUMP_R, 5)      // 5R
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
                Instruction(JUMP_L, 4)      // 8R
            )
        };

        static ulong[] Zad1Poprawne() => new ulong[]
        {
            Word(4), // <>                  // 0 N
            Word(1),                        // 1 ++
            Word(0),                        // 2 i
            Word(0),                        // 3 X
            Word(
                Instruction(LOAD_M, 0),     // 4L *
                Instruction(SUB_M, 1)       // 4R *
            ),
            Word(
                Instruction(SUB_M, 1),      // 5L *
                Instruction(STOR_M, 0)      // 5R *
            ),
            Word(
                Instruction(LOAD_M, 0),     // 6L
                Instruction(SUB_M, 2)       // 6R
            ),
            Word(
                Instruction(JUMP_P_L, 8),   // 7L
                Instruction(JUMP_R, 7)      // 7R
            ),
            Word(
                Instruction(LOAD_M, 2),     // 8L
                Instruction(ADD_M, 1)       // 8R
            ),
            Word(
                Instruction(STOR_M, 2),     // 9L
                Instruction(ADD_M, 3)       // 9R
            ),
            Word(
                Instruction(STOR_M, 3),     // 10L
                Instruction(JUMP_L, 6)      // 10R
            )
        };
    }
}
