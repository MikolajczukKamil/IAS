using System;
using IAS;

namespace Symulator_IAS.Example
{
    class Example_OS : ExampleCodes
    {
        static ProgramOn_IAS[] programs = {
            new ProgramOn_IAS(
                "Zadanie 1, suma liczb od 1 do n, działająca wersja z wykładu, wynik w m[3]", 
                Zad1Poprawione(), 1, 4
                ),

            new ProgramOn_IAS(
                "Zadanie 1, suma liczb od 1 do n, poprawna wersja, wynik w m[3]", 
                Zad1Poprawne(), 1, 4
                ),

            new ProgramOn_IAS(
                "Zadanie 2, wyrażenie n(n+1)/2, wynik w m[1]", 
                Zad2(), 1, 2
                ),

            new ProgramOn_IAS(
                "Zadanie 3, n!, z wykładu, wynik w m[3]", 
                Zad3(), 1, 4
                ),

            new ProgramOn_IAS(
                "Kwadrat liczby, wynik w m[0]", 
                Squere(), 1, 1
                ),

            new ProgramOn_IAS(
                "N wyraz ciągu Fibonacciego, wynik w m[2]",
                Fibonacci(), 1, 5
                ),

            new ProgramOn_IAS(
                "NWD, algorytm Euklidesa, wynik w m[0]", 
                EuclideanAlgorithm(), 2, 3
                ),

            new ProgramOn_IAS(
                "Suma kwadratów do n z użyciem funkcji, wynik w m[1]",
                SumSquereTo(), 1, 2
                )
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
    }
}
