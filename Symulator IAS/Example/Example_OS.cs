using System;
using IAS;

namespace Symulator_IAS
{
    class Example_OS : IAS_OptCodes
    {
        static ProgramOn_IAS[] programs = {
            new ProgramOn_IAS("Zadanie 1, suma liczb od 1 do n, działająca wersja z wykładu", Zad1PoprawioneZNowymiSkokami(), 4),
            new ProgramOn_IAS("Zadanie 1, suma liczb od 1 do n, poprawna wersja", Zad1Poprawne(), 4),
            new ProgramOn_IAS("Zadanie 2, wyrażenie n(n+1)/2", Zad2(), 2),
            new ProgramOn_IAS("Zadanie 3, n!, z wykładu", Zad3(), 4)
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
                        Console.Write("n = ");

                        int n = Convert.ToInt32(Console.ReadLine());

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
            catch (Exception e)
            {
                error = true;

                //Console.Clear();

                Console.WriteLine("Maszyna IAS się zmęczyła wróć później, najprawdopodobniej zawiódł operator maszyny :(" + " " + e.Message);
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
    }
}
