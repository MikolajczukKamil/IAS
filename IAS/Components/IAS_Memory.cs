using System;
using System.Text;

namespace IAS.Components
{
    using Word = Int64;
    using Address = UInt16;

    class IAS_Memory : IAS_Helpers
    {
        public static Address MaxSize = 1000;

        Word[] Memory;
        Address Length;

        public IAS_Memory(Word[] code, bool copy)
        {
            Length = (Address)code.Length;

            if (Length > MaxSize)
                throw new IASMemoryException($"Instraction limit has been reached, max {MaxSize}, used {code.Length}", -1);

            Memory = copy ? new Word[Length] : code;

            for (int i = 0; i < Length; i++)
                Memory[i] = To40BitsValue(code[i]);
        }

        void CheckAddress(Address address)
        {
            if (address >= Length)
                throw new IASMemoryException($"Program try to access memory[{address}] but memory length is {Memory.Length}", address);
        }

        public Word GetWord(Address address)
        {
            CheckAddress(address);

            return Memory[address];
        }

        public void SetWord(Address address, Word word)
        {
            CheckAddress(address);

            Memory[address] = To40BitsValue(word);
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
