using System;
using System.Text;

using IAS.Bus;
using IAS.Components;

namespace IAS.Memory
{
    using Word = Int64;
    using Address = UInt16;
    using Operation = Byte;

    /// <summary>
    /// IAS component - Memory
    /// </summary>
    class IAS_Memory : IAS_Helpers
    {
        /// <summary>
        /// Max memory capacity in Words, orginal machine has 2^10 = 1024 Words length
        /// </summary>
        public const Address MaxSize = 1024;

        /// <summary>
        /// Memory MR sygnal
        /// </summary>
        public const Operation MR = 0;

        /// <summary>
        /// Memory MW sygnal
        /// </summary>
        public const Operation MW = 1;

        /// <summary>
        /// Machine code
        /// </summary>
        Word[] Memory;

        /// <summary>
        /// Length od Memory
        /// </summary>
        Address Length;

        /// <summary>
        /// Inside Bus, comunication with main machine
        /// </summary>
        IAS_Bus Bus;

        /// <summary>
        /// New IAS memory
        /// </summary>
        /// <param name="code">Machine code to store</param>
        /// <param name="bus">Bus to comunicate with main machine</param>
        /// <param name="copy">Copy machine code or use orginal</param>
        public IAS_Memory(Word[] code, IAS_Bus bus, bool copy)
        {
            Bus = bus;
            Length = (Address)code.Length;

            if (Length > MaxSize)
                throw new IASMemoryException($"Instraction limit has been reached, max {MaxSize}, used {code.Length}", 0);

            Memory = copy ? new Word[Length] : code;

            for (int i = 0; i < Length; i++)
                Memory[i] = To40BitsValue(code[i]);
        }

        /// <summary>
        /// Machine step, like clock circle
        /// </summary>
        public void Step()
        {
            switch(Bus.Control)
            {
                case MR:
                    CheckAddress(Bus.Address);

                    Bus.Data = Memory[Bus.Address];

                    break;

                case MW:
                    CheckAddress(Bus.Address);

                    Memory[Bus.Address] = Bus.Data;

                    break;
            }
        }

        /// <summary>
        /// Check if address is in memory range
        /// </summary>
        /// <param name="address">Address to check</param>
        void CheckAddress(Address address)
        {
            if (address >= Length)
                throw new IASMemoryException($"Program try to access memory[{address}] but memory length is {Memory.Length}", address);
        }

        /// <summary>
        /// Get machine code in memory
        /// </summary>
        /// <param name="copy">Copy instructions or return orginal</param>
        /// <returns>Current machine code</returns>
        public Word[] GetInstructions(bool copy)
        {
            if (!copy) return Memory;

            Word[] copyOfMemory = new Word[Length];

            Array.Copy(Memory, copyOfMemory, Length);

            return copyOfMemory;
        }

        /// <summary>
        /// Default to string, show all memory
        /// </summary>
        /// <returns>Machine code</returns>
        public override string ToString() {
            return ToString(Length);
        }

        /// <summary>
        /// To string
        /// </summary>
        /// <param name="manyInstructions">Many instructions to show</param>
        /// <returns>Machine code</returns>
        public string ToString(Address manyInstructions)
        {
            StringBuilder description = new StringBuilder();

            manyInstructions = Math.Min(manyInstructions, Length);

            for (int i = 0; i < manyInstructions; i++)
                description.AppendLine($" {Memory[i]}");

            return description.ToString();
        }
    }
}
