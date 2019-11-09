using System;
using IAS;

namespace Symulator_IAS
{
    class IAS_Program
    {
        public string Name;
        public short MemoryToShow;
        ushort StartPosiotion;
        ulong[] Code;

        public IAS_Machine Machine;

        public IAS_Program(string name, ulong[] code, ushort startPosiotion = 0, short memoryToShow = -1)
        {
            if (code == null || code.Length == 0) throw new Exception("Code not found");
            if (startPosiotion >= code.Length) throw new Exception("Start Position not found in Code");

            if (memoryToShow < 0) memoryToShow = (short)startPosiotion;

            Name = name;
            Code = code;
            MemoryToShow = memoryToShow;
            StartPosiotion = startPosiotion;
        }

        public void Reset(int n)
        {
            Code[0] = IAS_Helpers.IntTo40ZM(n);
            Machine = new IAS_Machine(Code);
            Machine.ManualJumpTo(StartPosiotion);
        }
    }
}
