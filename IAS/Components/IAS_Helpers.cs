using System;

namespace IAS.Components
{
    using Word = UInt64;
    using Instruction = UInt32;
    using Operation = Byte;

    public abstract class IAS_Helpers : IAS_Masks
    {
        public Instruction GetLeftInstruction(Word word) => (Instruction)word & MaskFirst20Bits;

        public Instruction GetRightInstruction(Word word) => (Instruction)(word >> 20);

        public Operation GetOptCode(Instruction instruction) => (Operation)(instruction & MaskFirst8Bits);

        public static Word IntTo40ZM(long a) => ((Word)Math.Abs(a) & MaskFirst39Bits) | (a >= 0 ? 0 : MaskBit40);

        public static long ZM40ToInt(Word a) => (long)Module(a) * (Sign(a) == 0 ? 1 : -1);

        public static byte Sign(Word data) => (byte)((data >> 39) & 1);

        public static Word Module(Word a) => a & (~MaskBit40);

        public static Word Add(Word a, Word b) => IntTo40ZM(ZM40ToInt(a) + ZM40ToInt(b));

        public static Word Sub(Word a, Word b) => IntTo40ZM(ZM40ToInt(a) - ZM40ToInt(b));
    }
}
