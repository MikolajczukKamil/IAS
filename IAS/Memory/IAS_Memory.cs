using System;
using System.Text;

using IAS.Bus;
using IAS.Components;

namespace IAS.Memory
{
    using Word = Int64;
    using Address = UInt16;
    using Operation = Byte;

    class IAS_Memory : IAS_Helpers
    {
        public static Address MaxSize = 1000;
        public static Operation MR = 1;
        public static Operation MW = 2;

        Word[] Memory;
        Address Length;
        IAS_Bus Bus;

        public IAS_Memory(Word[] code, IAS_Bus bus, bool copy)
        {
            Bus = bus;
            Length = (Address)code.Length;

            if (Length > MaxSize)
                throw new IASMemoryException($"Instraction limit has been reached, max {MaxSize}, used {code.Length}", -1);

            Memory = copy ? new Word[Length] : code;

            for (int i = 0; i < Length; i++)
                Memory[i] = To40BitsValue(code[i]);
        }

        public void Do()
        {
            if(Bus.Control == MR)
            {
                CheckAddress(Bus.Address);

                Bus.Data = Memory[Bus.Address];
            }

            if (Bus.Control == MW)
            {
                CheckAddress(Bus.Address);

                Memory[Bus.Address] = Bus.Data;
            }
        }

        void CheckAddress(Address address)
        {
            if (address >= Length)
                throw new IASMemoryException($"Program try to access memory[{address}] but memory length is {Memory.Length}", address);
        }

        public Word[] GetInstructions(bool copy)
        {
            if (!copy) return Memory;

            Word[] copyOfMemory = new Word[Length];

            Array.Copy(Memory, copyOfMemory, Length);

            return copyOfMemory;
        }

        public override string ToString() => ToString(Length);

        public string ToString(int manyInstructions)
        {
            StringBuilder description = new StringBuilder();

            for (int i = 0; i < Length && i < manyInstructions; i++)
                description.AppendLine($" {Memory[i]}");

            return description.ToString();
        }
    }
}
