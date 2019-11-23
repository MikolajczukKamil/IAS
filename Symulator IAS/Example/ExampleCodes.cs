using System;
using IAS;

namespace Symulator_IAS.Example
{
    using IASWord = Int64;
    using IASInstruction = UInt32;

    class ExampleCodes : IAS_Codes
    {
        public static IASWord[] Zad1Poprawione() => new IASWord[]
        {
            Word(4), // n = M(0) <>         // 0
            Word(1),                        // 1 // const 1 = M(1)
            Word(0), // i = M(2)            // 2
            Word(0), // x = M(3)            // 3
            Word(
                Instruction(LOAD_M, 0),     // 4L // AC = n ; while(n - i >= 0)
                Instruction(SUB_M, 2)       // 4R // AC = AC - i
            ),
            Word(
                Instruction(JUMP_P_L, 6),   // 5L // if(AC >= 0) jump to 6L
                Instruction(JUMP_R, 5)      // 5R // else koniec = inf loop = jump to 5R
            ),
            Word(
                Instruction(LOAD_M, 2),     // 6L // AC = i
                Instruction(ADD_M, 1)       // 6R // AC++
            ),
            Word(
                Instruction(STOR_M, 2),     // 7L // i = AC
                Instruction(ADD_M, 3)       // 7R // AC += x
            ),
            Word(
                Instruction(STOR_M, 3),     // 8L // x = AC
                Instruction(JUMP_L, 4)      // 8R // end while;
            )
        };

        public static IASWord[] Zad1Poprawne() => new IASWord[]
        {
            // * Poprawka jako słowo 4 i 5
            Word(4), // n = M(0) <>         // 0
            Word(1),                        // 1 // const 1 = M(1)
            Word(0), // i = M(2)            // 2
            Word(0), // x = M(3)            // 3
            Word(
                Instruction(LOAD_M, 0),     // 4L // AC = n  *
                Instruction(SUB_M, 1)       // 4R // AC --   *
            ),
            Word(
                Instruction(STOR_M, 0),      // 5L // n = AC *
                Instruction(JUMP_L, 6)       // 5R // *
            ),
            Word(
                Instruction(LOAD_M, 0),     // 6L // AC = n ; while(n - i >= 0)
                Instruction(SUB_M, 2)       // 6R // AC = AC - i
            ),
            Word(
                Instruction(JUMP_P_L, 6+2), // 7L // if(AC >= 0) jump to 8L
                Instruction(JUMP_R, 5+2)    // 7R // else koniec = inf loop = jump to 7R
            ),
            Word(
                Instruction(LOAD_M, 2),     // 8L // AC = i
                Instruction(ADD_M, 1)       // 8R // AC++
            ),
            Word(
                Instruction(STOR_M, 2),     // 9L // i = AC
                Instruction(ADD_M, 3)       // 9R // AC += x
            ),
            Word(
                Instruction(STOR_M, 3),     // 10L // x = AC
                Instruction(JUMP_L, 4+2)    // 10R // end while;
            )
        };

        public static IASWord[] Zad2() => new IASWord[]
        {
            Word(4), // n = M(0) <>         // 0
            Word(1), // wynik = M(1)        // 1
            Word(
                Instruction(LOAD_M, 0),     // 2L // AC = n
                Instruction(ADD_M, 1)       // 2R // AC += wynik
            ),
            Word(
                Instruction(STOR_M, 1),     // 3L // wynik = AC
                Instruction(LOAD_MQ_M, 1)   // 3R // MQ = wynik
            ),
            Word(
                Instruction(MUL_M, 0),      // 4L // MQ *= n
                Instruction(LOAD_MQ)        // 4R // AC = MQ
            ),
            Word(
                Instruction(RSH),           // 5L // AC >>= 1
                Instruction(STOR_M, 1)      // 5R // wynik = AC
            ),
            Word(
                Instruction(JUMP_L, 6),     // 6L // koniec = inf loop = jump to 6L
                0
            )
        };

        public static IASWord[] Zad3() => new IASWord[]
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

        public static IASWord[] Fibonacci() => new IASWord[]
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

        public static IASWord[] Squere() => new IASWord[]
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

        public static IASWord[] EuclideanAlgorithm() => new IASWord[]
        {
            Word(35), // n = M(0) <>         // 0
            Word(10), // m = M(1) <>         // 1
            Word(0),  // tmp = M(2)          // 2
            Word(1),                         // 3 // const 1 = M(3)
            Word(
                Instruction(LOAD_M, 1),      // 4L // AC = m ; while(m - 1 >= 0)
                Instruction(SUB_M, 3)        // 4R // AC --
            ),
            Word(
                Instruction(JUMP_P_L, 6),    // 5L // if(AC >= 0) jump to 6L
                Instruction(JUMP_R, 5)       // 5R // else koniec = inf loop = jump to 5R
            ),
            Word(
                Instruction(LOAD_M, 1),      // 6L // AC = m
                Instruction(STOR_M, 2)       // 6R // tmp = AC
            ),
            Word(
                Instruction(LOAD_M, 0),      // 7L // AC = n
                Instruction(DIV_M, 1)        // 7R // AC = AC % m
            ),
            Word(
                Instruction(STOR_M, 1),      // 8L // m = AC
                Instruction(LOAD_M, 2)       // 8R // AC = tmp
            ),
            Word(
                Instruction(STOR_M, 0),      // 9L // n = AC
                Instruction(JUMP_L, 4)       // 9R // end while;
            )
        };

        public static IASWord[] SumSquereTo() => new IASWord[]
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
