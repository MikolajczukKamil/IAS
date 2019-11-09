using System;
using IAS;

namespace Symulator_IAS
{
    class Test_Machine : OptCodes
    {
        public static void Run()
        {
            IAS_Machine machine = new IAS_Machine(Zad1());

            machine.ManualJumpTo(4); // zaczynamy od 4

            Console.WriteLine(machine.ToString(4));

            while (Console.ReadKey().KeyChar != 'a')
            {
                machine.Step();

                Console.WriteLine(machine.ToString(4));
            }
        }

        static ulong[] Zad1() => new ulong[]
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
    }
}
