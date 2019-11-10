using System;
using IAS;

namespace Symulator_IAS.Example
{
    class ExampleCodes : IAS_Codes
    {
        public static ulong[] Zad1PoprawioneZNowymiSkokami() => new ulong[]
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

        public static ulong[] Zad1Poprawne() => new ulong[]
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

        public static ulong[] Zad2() => new ulong[]
        {
            Word(4), // <>                  // 0 N
            Word(1),                        // 1, W
            Word(
                Instruction(LOAD_M, 0),     // 2L
                Instruction(ADD_M, 1)       // 2R
                ),
            Word(
                Instruction(STOR_M, 1),     // 3L
                Instruction(LOAD_MQ_M, 1)   // 3R
                ),
            Word(
                Instruction(MUL_M, 0),      // 4L
                Instruction(LOAD_MQ)        // 4R
                ),
            Word(
                Instruction(RSH),           // 5L
                Instruction(STOR_M, 1)      // 5R
                ),
            Word(
                Instruction(JUMP_L, 6),     // 6L
                0
                )
        };

        public static ulong[] Zad3() => new ulong[]
        {
            Word(5), // <>                  // 0 N
            Word(1),                        // 1 ++
            Word(0),                        // 2 i
            Word(1),                        // 3 X
            Word(
                Instruction(LOAD_M, 0),     // 4L
                Instruction(STOR_M, 2)      // 4R
            ),
            Word(
                Instruction(LOAD_MQ_M, 3),  // 5L
                Instruction(MUL_M, 2)       // 5R
            ),
            Word(
                Instruction(LOAD_MQ),       // 6L
                Instruction(STOR_M, 3)      // 6R
            ),
            Word(
                Instruction(LOAD_M, 2),     // 7L
                Instruction(SUB_M, 1)       // 7R
            ),
            Word(
                Instruction(STOR_M, 2),     // 8L
                Instruction(SUB_M, 1)       // 8R
            ),
            Word(
                Instruction(SUB_M, 1),      // 9L
                Instruction(JUMP_P_L, 5)    // 9R
            ),
            Word(
                Instruction(JUMP_L, 10),    // 10L
                0
            )
        };

        public static ulong[] Fibonacci() => new ulong[]
        {
            Word(5), // n <>                // 0
            Word(1), // a                   // 1
            Word(1), // b                   // 2
            Word(0), // i                   // 3
            Word(0), // tmp                 // 4
            Word(
                Instruction(LOAD_M, 0),     // 5L
                Instruction(SUB_M, 14)      // 5R
            ),
            Word(
                Instruction(STOR_M, 3),     // 6L
                Instruction(JUMP_P_R, 7)    // 6R
            ),
            Word(
                Instruction(JUMP_L, 7),     // 7L
                Instruction(LOAD_M, 1)      // 7R
            ),
            Word(
                Instruction(ADD_M, 2),      // 8L
                Instruction(STOR_M, 4)      // 8R
            ),
            Word(
                Instruction(LOAD_M, 2),     // 9L
                Instruction(STOR_M, 1)      // 9R
            ),
            Word(
                Instruction(LOAD_M, 4),      // 10L
                Instruction(STOR_M, 2)       // 10R
            ),
            Word(
                Instruction(LOAD_M, 3),      // 11L
                Instruction(SUB_M, 13)       // 11R
            ),
            Word(
                Instruction(STOR_M, 3),      // 12L
                Instruction(JUMP_R, 6)       // 12R
            ),
            Word(1),                         // 13 = 1
            Word(3),                         // 14 = 3
        };

        public static ulong[] Squere() => new ulong[]
        {
            Word(5), // n <>                // 0
            Word(
                Instruction(LOAD_MQ_M, 0),  // 1L
                Instruction(MUL_M, 0)       // 1R
            ),
            Word(
                Instruction(LOAD_MQ),       // 2L
                Instruction(STOR_M, 0)      // 2R
            ),
            Word(
                Instruction(JUMP_L, 3),     // 3L
                0
            )
        };

        public static ulong[] EuclideanAlgorithm() => new ulong[]
        {
            Word(35), // n <>                // 0
            Word(10), // m <>                // 1
            Word(0),  // tmp                 // 2
            Word(
                Instruction(LOAD_M, 1),      // 3L
                Instruction(SUB_M, 9)        // 3R
            ),
            Word(
                Instruction(JUMP_P_L, 5),    // 4L
                Instruction(JUMP_R, 4)       // 4R
            ),
            Word(
                Instruction(LOAD_M, 1),      // 5L
                Instruction(STOR_M, 2)       // 5R
            ),
            Word(
                Instruction(LOAD_M, 0),      // 6L
                Instruction(DIV_M, 1)        // 6R
            ),
            Word(
                Instruction(STOR_M, 1),      // 7L
                Instruction(LOAD_M, 2)       // 7R
            ),
            Word(
                Instruction(STOR_M, 0),      // 8L
                Instruction(JUMP_L, 3)       // 8R
            ),

            Word(1),                         // 9 = 1
        };

        public static ulong[] SumSquereTo() => new ulong[]
        {
            Word(5), // n <>                // 0 // n = <>
            Word(0),                        // 1 // Sum = 0
            Word(
                Instruction(LOAD_M, 14),    // 2L
                Instruction(STOR_M, 18)     // 2R // set return address from Squere()
            ),
            Word(
                Instruction(LOAD_M, 0),     // 3L // n - 1 >= 0
                Instruction(SUB_M, 13)      // 3R
            ),
            Word(
                Instruction(JUMP_P_L, 5),   // 4L // if (n - 1 >= 0) while
                Instruction(JUMP_R, 4)      // 4R // else halt
            ),
            Word(
                Instruction(LOAD_M, 0),     // 5L // while start
                Instruction(STOR_M, 19)     // 5R
            ),
            Word(
                Instruction(JUMP_L, 20),    // 6L // Squere(n)
                0                           // 6R // CALL FN
            ),
            Word(
                Instruction(LOAD_M, 1),     // 7L // returnSquere
                Instruction(ADD_M, 19)      // 7R
            ),
            Word(
                Instruction(STOR_M, 1),     // 8L
                Instruction(LOAD_M, 0)      // 8R // n--
            ),
            Word(
                Instruction(SUB_M, 13),     // 9L
                Instruction(STOR_M, 0)      // 9R
            ),
            Word(
                Instruction(JUMP_L, 3),     // 10L // end while
                0                           // 10R
            ),
            Word(),                         // 11
            Word(),                         // 12
            Word(1),                        // 13 // const m[13] = 1
            Word(7),                        // 14 // const m[14] = 7 -> goto returnSquere
            Word(),                         // 15
            Word(),                         // 16
            Word(),                         // 17
            Word(-1), // return             // 18 // function Squere(n) => n^2
            Word(0),  // arg n and result   // 19
            Word(
                Instruction(LOAD_MQ_M, 19), // 20L // m[19] = Squere(n) -> /AC = this.address/ STOR_M 18; /AC = n/ STOR_M 19; JUMP_L 20;
                Instruction(MUL_M, 19)      // 20R
            ),
            Word(
                Instruction(LOAD_MQ),       // 21L
                Instruction(STOR_M, 19)     // 21R
            ),
            Word(
                Instruction(JUMP_M_L, 18),   // 22L // return
                0
            )
        };
    }
}
