using System;

namespace IAS.Components
{
    using Word = Int64;
    using Instruction = UInt32;
    using Operation = Byte;

    public abstract class IAS_Helpers
    {
        static Word MaxValue = IAS_Masks.First39Bits;
        static Word MinValue = ~MaxValue;

        protected static Instruction GetLeftInstruction(Word word) => (Instruction)word & IAS_Masks.First20Bits;

        protected static Instruction GetRightInstruction(Word word) => (Instruction)(word >> 20);

        protected static Operation GetOpCode(Instruction instruction) => (Operation)(instruction & IAS_Masks.First8Bits);

        protected static Word To40BitsValue(Word a)
        {
            // overflow simulation

            if (a > MaxValue)
            {
                long diff = a - MaxValue;

                return MinValue + (diff - 1);
            }

            if(a < MinValue)
            {
                long diff = MinValue - a;

                return MaxValue - (diff - 1);
            }

            return a;
        }
    }
}
