using System;
using IAS;

namespace Symulator_IAS.Example
{
    using IASWord = Int64;
    using IASInstruction = UInt32;

    class ExampleCodes : IAS_Codes
    {
        public static IASWord[] Zad1SumaOd1DoN() => new IASWord[]
        {
            Word(1), // n = M(0) <>         // 0
            Word(1),                        // 1 // const 1 = M(1)
            Word(0), // i = M(2)            // 2
            Word(0), // x = M(3)            // 3
            Word(
                Instruction(LOAD_M, 0),     // 4L // AC = n ; while(n - i >= 0)
                Instruction(SUB_M, 2)       // 4R // AC = AC - i
            ),
            Word(
                Instruction(JUMP_P_L, 6),   // 5L // if(AC >= 0) jump to 6L
                Instruction(JUMP_R, 5)      // 5R // else done = inf loop = jump to 5R
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
                Instruction(JUMP_L, 4)      // 8R // end while
            )
        };

        public static IASWord[] Zad1SumaOd1DoNPoprawne() => new IASWord[]
        {
            Word(1), // n = M(0) <>         // 0
            Word(0), // i = M(1)            // 1
            Word(0), // x = M(2)            // 2
            Word(1),                        // 3 // const 1 = M(3)
            Word(
                Instruction(LOAD_M, 0),     // 4L // AC = n
                Instruction(SUB_M, 3)       // 4R // AC--
            ),
            Word(
                Instruction(STOR_M, 0),     // 5L // n = AC 
                Instruction(LOAD_M, 0)      // 5R // AC = n ; while(n - i >= 0)

            ),
            Word(
                Instruction(SUB_M, 1),       // 6L // AC = AC - i
                Instruction(JUMP_P_R, 7)     // 6R // if(AC >= 0) jump to 7R
            ),
            Word(
                Instruction(JUMP_L, 7),      // 7L // else done = inf loop = jump to 7L
                Instruction(LOAD_M, 1)       // 7R // AC = i
            ),
            Word(
                Instruction(ADD_M, 3),       // 8L // AC++
                Instruction(STOR_M, 1)       // 8R // i = AC
            ),
            Word(
                Instruction(ADD_M, 2),       // 9L // AC += x
                Instruction(STOR_M, 2)       // 9R // x = AC
            ),
            Word(
                Instruction(JUMP_R, 5),      // 10L // end while
                0
            )
        };

        public static IASWord[] Zad2SumaLiczbOd1DoNzUzyciemWzoru() => new IASWord[]
        {
            Word(1), // n = M(0) <>         // 0
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
                Instruction(JUMP_L, 6),     // 6L // done = inf loop = jump to 6L
                0
            )
        };

        public static IASWord[] Zad3Silnia() => new IASWord[]
        {
            Word(1), // n = M(0) <>         // 0
            Word(1),                        // 1 // const 1 = M(1)
            Word(0), // i = M(2)            // 2
            Word(1), // x = M(3)            // 3
            Word(
                Instruction(LOAD_M, 0),     // 4L // AC = n
                Instruction(STOR_M, 2)      // 4R // i = AC
            ),
            Word(
                Instruction(LOAD_MQ_M, 3),  // 5L // MQ = x // do
                Instruction(MUL_M, 2)       // 5R // MQ *= i
            ),
            Word(
                Instruction(LOAD_MQ),       // 6L // AC = MQ
                Instruction(STOR_M, 3)      // 6R // x = AC
            ),
            Word(
                Instruction(LOAD_M, 2),     // 7L // AC = i
                Instruction(SUB_M, 1)       // 7R // AC --
            ),
            Word(
                Instruction(STOR_M, 2),     // 8L // i = AC
                Instruction(SUB_M, 1)       // 8R // AC--
            ),
            Word(
                Instruction(SUB_M, 1),      // 9L // AC--
                Instruction(JUMP_P_L, 5)    // 9R // while(AC >= 0) jump to 5L
            ),
            Word(
                Instruction(JUMP_L, 10),    // 10L // done = inf loop = jump to 10L
                0
            )
        };

        public static IASWord[] Fibonacci() => new IASWord[]
        {
            Word(1), // n = M(0) <>         // 0
            Word(1), // a = M(1)            // 1
            Word(1), // b = M(2)            // 2
            Word(0), // c = M(3)            // 3
            Word(0), // i = M(4)            // 4
            Word(1),                        // 5 // const 1 = M(5)
            Word(3),                        // 6 // const 3 = M(6)
            Word(
                Instruction(LOAD_M, 0),     // 7L // AC = n
                Instruction(SUB_M, 6)       // 7R // AC -= 3
            ),
            Word(
                Instruction(STOR_M, 4),     // 8L // i = AC
                Instruction(JUMP_P_R, 9)    // 8R // if(AC >= 0) jump to 9R; while(i >= 0)
            ),
            Word(
                Instruction(JUMP_L, 9),     // 9L // else done = inf loop = jump to 9L
                Instruction(LOAD_M, 1)      // 9R // AC = a
            ),
            Word(
                Instruction(ADD_M, 2),      // 10L // AC += b
                Instruction(STOR_M, 3)      // 10R // c = AC
            ),
            Word(
                Instruction(LOAD_M, 2),     // 11L // AC = b
                Instruction(STOR_M, 1)      // 11R // a = AC
            ),
            Word(
                Instruction(LOAD_M, 3),      // 12L // AC = c
                Instruction(STOR_M, 2)       // 12R // b = AC
            ),
            Word(
                Instruction(LOAD_M, 4),      // 13L // AC = i
                Instruction(SUB_M, 5)        // 13R // AC--
            ),
            Word(
                Instruction(STOR_M, 4),      // 14L // i = AC
                Instruction(JUMP_R, 8)       // 14R // end while
            ),
            
        };

        public static IASWord[] Squere() => new IASWord[]
        {
            Word(1), // n = M(0) <>         // 0
            Word(
                Instruction(LOAD_MQ_M, 0),  // 1L // MQ = n
                Instruction(MUL_M, 0)       // 1R // MQ *= n
            ),
            Word(
                Instruction(LOAD_MQ),       // 2L // AC = MQ
                Instruction(STOR_M, 0)      // 2R // n = AC
            ),
            Word(
                Instruction(JUMP_L, 3),     // 3L // done = inf loop = jump to 3L
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
                Instruction(JUMP_R, 5)       // 5R // else done = inf loop = jump to 5R
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
                Instruction(JUMP_L, 4)       // 9R // end while
            )
        };

        public static IASWord[] SumSquereTo() => new IASWord[]
        {
            Word(1), // n = M(0) <>         // 0
            Word(0), // sum = M(1)          // 1
            Word(1),                        // 2 // const 1 = M(2)
            Word(11),// ret = M(3)          // 3 // Squere() return address (Left) = const 11
            Word(20),// call = M(4)         // 4 // Squere() address
            Word(0), // arg n = M(5)        // 5 // arg n for Squere()
            Word(0), // res = M(6)          // 6 // Squere(n) result
            Word(
                Instruction(LOAD_M, 0),     // 7L // AC = n ; while(n - 1 >= 0)
                Instruction(SUB_M, 2)       // 7R // AC--
            ),
            Word(
                Instruction(JUMP_P_L, 9),   // 8L // if (n - 1 >= 0) jump to 9L
                Instruction(JUMP_R, 8)      // 8R // else done = inf loop = jump to 5R
            ),
            Word(
                Instruction(LOAD_M, 0),     // 9L // AC = n
                Instruction(STOR_M, 5)      // 9R // arg n = AC
            ),
            Word(
                Instruction(JUMP_M_L, 4),   // 10L // Squere(n) = jump to call
                0                           // 10R
            ),
            Word(
                Instruction(LOAD_M, 1),     // 11L // AC = sum
                Instruction(ADD_M, 6)       // 11R // AC += res
            ),
            Word(
                Instruction(STOR_M, 1),     // 12L // sum = AC
                Instruction(LOAD_M, 0)      // 12R // AC = n
            ),
            Word(
                Instruction(SUB_M, 2),      // 13L // AC--
                Instruction(STOR_M, 0)      // 13R // n = AC
            ),
            Word(
                Instruction(JUMP_L, 7),     // 14L // end while
                0                           // 14R
            ),
            Word(),                         // 15
            Word(),                         // 16
            Word(),                         // 17
            Word(),                         // 18
            Word(),                         // 19
            Word(
                Instruction(LOAD_MQ_M, 5),  // 20L // MQ = arg n
                Instruction(MUL_M, 5)       // 20R // MQ *= arg n
            ),
            Word(
                Instruction(LOAD_MQ),       // 21L // AC = MQ
                Instruction(STOR_M, 6)      // 21R // ret = AC
            ),
            Word(
                Instruction(JUMP_M_L, 3),   // 22L // return = jump to ret
                0
            )
        };

        public static IASWord[] DivideByZero() => new IASWord[]
        {
            Word(0),                        // 0 // const 0 = M(0)
            Word(
                Instruction(DIV_M, 0),      // 1L // MQ /= 0
                0
            ),
        };

    }
}
