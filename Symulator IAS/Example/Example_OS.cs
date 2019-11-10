using System;
using IAS;

namespace Symulator_IAS
{
    class Example_OS : IAS_OptCodes
    {
        static ProgramOn_IAS[] programs = {
            new ProgramOn_IAS("Zadanie 1, suma liczb od 1 do n, działająca wersja z wykładu, wynik w m[3]", Zad1PoprawioneZNowymiSkokami(), 1, 4),
            new ProgramOn_IAS("Zadanie 1, suma liczb od 1 do n, poprawna wersja, wynik w m[3]", Zad1Poprawne(), 1, 4),
            new ProgramOn_IAS("Zadanie 2, wyrażenie n(n+1)/2, wynik w m[1]", Zad2(), 1, 2),
            new ProgramOn_IAS("Zadanie 3, n!, z wykładu, wynik w m[3]", Zad3(), 1, 4),
            new ProgramOn_IAS("N wyraz ciągu Fibonacciego, wynik w m[2]", Fibonacci(), 1, 5),
            new ProgramOn_IAS("NWD, algorytm Euklidesa, wynik w m[0]", EuclideanAlgorithm(), 2, 3)
        };

        public static void Run()
        {
            bool error = false;

            try
            {
                bool stop;

                do
                {
                    Console.Clear();
                    Console.WriteLine("Wycierz program do uruchomienia");
                    Console.WriteLine("Następnie naciśnij spację aby uruchomić kolejne polecenie");
                    Console.WriteLine("X aby wyjść");
                    Console.WriteLine("----------------------------------------------------------");

                    for (int i = 1; i <= programs.Length; i++)
                        Console.WriteLine($"{i}) {programs[i - 1].Name}");

                    Console.WriteLine("----------------------------------------------------------");

                    Console.Write("Twój wybór: ");

                    int option = -1;

                    try
                    {
                        option = Convert.ToInt32(Console.ReadLine());
                    }
                    catch (Exception) { }

                    stop = !(option > 0 && option <= programs.Length);

                    Console.Clear();

                    if (!stop)
                    {
                        ProgramOn_IAS program = programs[option - 1];

                        Console.WriteLine(program.Name);

                        int[] n = new int[program.Wariables];

                        for (int i = 0; i < program.Wariables; i++)
                        {
                            Console.Write($"m[{i}] = ");
                            n[i] = Convert.ToInt32(Console.ReadLine());
                        }

                        program.Reset(n);

                        IAS_Machine machine = program.Machine;

                        Console.WriteLine();
                        Console.WriteLine(machine.ToString(program.MemoryToShow));

                        char key = Console.ReadKey().KeyChar;

                        int counter = 0;

                        while (key != 'x' && key != 'X')
                        {
                            counter++;
                            machine.Step();

                            Console.WriteLine($"Krok: {counter}");
                            Console.WriteLine(machine.ToString(program.MemoryToShow));

                            key = Console.ReadKey().KeyChar;
                        }
                    }

                } while (!stop);
            }
            catch (Exception)
            {
                error = true;

                Console.Clear();

                Console.WriteLine("Maszyna IAS się zmęczyła wróć później, najprawdopodobniej zawiódł operator maszyny :(");
            }

            if (!error)
            {
                Console.Clear();

                Console.WriteLine("Maszyna IAS dziękuje za pamięć :)");
            }
        }

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

        static ulong[] Zad2() => new ulong[]
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

        static ulong[] Zad3() => new ulong[]
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

        static ulong[] Fibonacci() => new ulong[]
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

        static ulong[] EuclideanAlgorithm() => new ulong[]
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
    }
}
