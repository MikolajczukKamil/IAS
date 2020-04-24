using System;

namespace IAS.Components
{
    using Word = Int64;
    using Instruction = UInt32;
    using Address = UInt16;
    using Operation = Byte;

    /// <summary>
    /// IAS machine helpers
    /// </summary>
    public abstract class IAS_Helpers
    {
        /// <summary>
        /// Max value possible to store in word of data
        /// </summary>
        static Word MaxValue = IAS_Masks.First39Bits;

        /// <summary>
        /// Min value possible to store in word of data
        /// </summary>
        static Word MinValue = ~MaxValue;

        /// <summary>
        /// Get left instruction from word of data
        /// </summary>
        /// <param name="word">Word</param>
        /// <returns>Instruction</returns>
        protected static Instruction GetLeftInstruction(Word word) => (Instruction)(word >> 20);

        /// <summary>
        /// Get right instruction from word of data
        /// </summary>
        /// <param name="word">Word</param>
        /// <returns>Instruction</returns>
        protected static Instruction GetRightInstruction(Word word) => (Instruction)word & IAS_Masks.First20Bits;

        /// <summary>
        /// Get operation code from instruction
        /// </summary>
        /// <param name="instruction">Instruction</param>
        /// <returns>Operation code</returns>
        protected static Operation GetOperationCode(Instruction instruction) => (Operation)(instruction >> 12);

        /// <summary>
        /// Get address code from instruction
        /// </summary>
        /// <param name="instruction"></param>
        /// <returns>Address</returns>
        protected static Address GetAddress(Instruction instruction) => (Address)(instruction & IAS_Masks.First12Bits);

        /// <summary>
        /// Translate value to Word length value (40 bits) with overflow effect
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Word length value</returns>
        protected static Word To40BitsValue(Word value)
        {
            if (value > MaxValue)
            {
                long diff = value - MaxValue;

                return MinValue + (diff - 1);
            }

            if(value < MinValue)
            {
                long diff = MinValue - value;

                return MaxValue - (diff - 1);
            }

            return value;
        }
    }
}
