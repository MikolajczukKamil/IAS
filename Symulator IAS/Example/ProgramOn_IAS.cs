using System;
using IAS;
using IAS.Components;

namespace Symulator_IAS.Example
{
    class ProgramOn_IAS
    {
        public string Name;
        public ushort Wariables;
        public short MemoryToShow;

        ushort StartPosiotion;
        ulong[] Code;

        public IAS_Machine Machine;

        public ProgramOn_IAS(string name, ulong[] code, ushort wariables, ushort startPosiotion = 0, short memoryToShow = -1)
        {
            if (code == null || code.Length == 0) throw new Exception("Code not found");
            if (startPosiotion >= code.Length) throw new Exception("Start Position not found in Code");

            if (memoryToShow < 0) memoryToShow = (short)startPosiotion;

            Name = name;
            Code = code;
            Wariables = wariables;
            MemoryToShow = memoryToShow;
            StartPosiotion = startPosiotion;
        }

        public void Reset(int[] n)
        {
            for (int i = 0; i < n.Length; i++)
                Code[i] = IAS_Helpers.IntTo40ZM(n[i]);

            Machine = new IAS_Machine(Code);
            Machine.ManualJumpTo(StartPosiotion);
        }
    }
}
