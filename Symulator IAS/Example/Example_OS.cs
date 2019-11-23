using System;
using IAS;

namespace Symulator_IAS.Example
{
    class Example_OS : ExampleCodes
    {
        static ProgramOn_IAS[] programs = {
            new ProgramOn_IAS(
                "Suma liczb od 1 do n + 1                max 1'048'574        wynik w m[3]", 
                Zad1SumaOd1DoN(), 1, 4
            ),

            new ProgramOn_IAS(
                "Suma liczb od 1 do n                    max 1'048'575        wynik w m[2]", 
                Zad1SumaOd1DoNPoprawne(), 1, 4
            ),

            new ProgramOn_IAS(
                "Wyrażenie n(n+1)/2                      max 1'048'575        wynik w m[1]", 
                Zad2SumaLiczbOd1DoNzUzyciemWzoru(), 1, 2
            ),

            new ProgramOn_IAS(
                "n!                                      max 14               wynik w m[3]",
                Zad3Silnia(), 1, 4
            ),

            new ProgramOn_IAS(
                "Kwadrat liczby                          max 741'455          wynik w m[0]", 
                Squere(), 1, 1
            ),

            new ProgramOn_IAS(
                "N wyraz ciągu Fibonacciego              max 57               wynik w m[2]",
                Fibonacci(), 1, 7
            ),

            new ProgramOn_IAS(
                "NWD, algorytm Euklidesa                 max 549'755'813'887  wynik w m[0]",
                EuclideanAlgorithm(), 2, 4
            ),

            new ProgramOn_IAS(
                "Suma kwadratów do n z użyciem funkcji   max 11'814           wynik w m[1]",
                SumSquereTo(), 1, 7
            ),

            new ProgramOn_IAS(
                "Dzielenie przez zero :)",
                DivideByZero(), 0, 1
            )
        };

        public static void Run()
        {
            bool error = false;

            try
            {
                bool use;

                do
                {
                    Console.Clear();
                    Console.WriteLine("SYMULATOR IAS");
                    Console.WriteLine();
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

                    Console.Clear();

                    use = option > 0 && option <= programs.Length;

                    if (use)
                    {
                        ProgramOn_IAS program = programs[option - 1];

                        Console.WriteLine($"{option}) {program.Name}");

                        long[] n = new long[program.Wariables];

                        for (int i = 0; i < program.Wariables; i++)
                        {
                            Console.Write($"m[{i}] = ");
                            n[i] = Convert.ToInt64(Console.ReadLine());
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

                } while (use);
            }
            catch (Exception e)
            {
                error = true;

                Console.Clear();

                Console.WriteLine("Maszyna IAS się zmęczyła wróć później, najprawdopodobniej zawiódł operator maszyny :(");
                Console.WriteLine(e.Message);
            }

            if (!error)
            {
                Console.Clear();

                Console.WriteLine("Maszyna IAS dziękuje za pamięć :)");
            }
        }
    }
}
