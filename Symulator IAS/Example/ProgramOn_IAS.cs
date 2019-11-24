using System;
using IAS;
using IAS.Components;

namespace Symulator_IAS.Example
{
    using Word = Int64;

    class ProgramOn_IAS : IAS_Helpers
    {
        public string Name;
        public ushort Wariables;
        public short MemoryToShow;

        ushort StartPosiotion;
        Word[] Code;

        public IAS_Machine Machine;

        public ProgramOn_IAS(string name, Word[] code, ushort wariables, ushort startPosiotion = 0, short memoryToShow = -1)
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

        public void Reset(long[] code)
        {
            for (int i = 0; i < code.Length; i++)
                Code[i] = To40BitsValue(code[i]);

            Machine = new IAS_Machine(Code);
            Machine.ManualJumpTo(StartPosiotion);
        }
    }
}
