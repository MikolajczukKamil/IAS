using System;

namespace IAS.Components
{
    using Word = Int64;
    using Instruction = UInt32;
    using Operation = Byte;

    public abstract class IAS_Helpers : IAS_Masks
    {
        public static Word MaxValue = MaskFirst39Bits;
        public static Word MinValue = ~MaxValue;

        public static Instruction GetLeftInstruction(Word word) => (Instruction)word & MaskFirst20Bits;

        public static Instruction GetRightInstruction(Word word) => (Instruction)(word >> 20);

        public static Operation GetOpCode(Instruction instruction) => (Operation)(instruction & MaskFirst8Bits);

        public static Word To40BitsValue(Word a)
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
