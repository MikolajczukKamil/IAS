using System;
using System.Text;

namespace IAS
{
    public class IASMemoryException : Exception
    {
        public int LastAddress;

        public IASMemoryException(string message, int lastAddress) : base(message) {
            LastAddress = lastAddress;
        }
    }

    class IAS_Memory
    {
        public static ushort MaxSize = 1000;

        ulong[] Instructions;
        ushort Length;

        public IAS_Memory(ulong[] instructions, bool copyInstructions)
        {
            if (instructions.Length > MaxSize)
                throw new IASMemoryException($"Instraction limit has been reached, max {MaxSize}, used {instructions.Length}", -1);

            Length = (ushort)instructions.Length;

            Instructions = copyInstructions ? new ulong[Length] : instructions;

            for (int i = 0; i < Length; i++)
                Instructions[i] = instructions[i] & IAS_Helpers.MaskFirst40Bits;
        }

        void CheckAddress(ushort address)
        {
            if (address >= Length)
                throw new IASMemoryException($"Program try to access memory[{address}] but memory length is {Instructions.Length}", address);
        }

        public ulong GetWord(ushort address)
        {
            CheckAddress(address);

            return Instructions[address];
        }

        public void SetWord(ushort address, ulong word)
        {
            CheckAddress(address);

            Instructions[address] = word & IAS_Helpers.MaskFirst40Bits;
        }

        public override string ToString() => ToString((short)Length);

        public string ToString(short manyInstructions)
        {
            if (manyInstructions < 0) manyInstructions = short.MaxValue;

            StringBuilder description = new StringBuilder();

            for (int i = 0; i < Length && i < manyInstructions; i++)
                description.AppendLine($" {IAS_Helpers.ZM40ToInt(Instructions[i])}");

            return description.ToString();
        }
    }
}
