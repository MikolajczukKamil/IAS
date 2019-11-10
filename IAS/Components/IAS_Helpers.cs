using System;

namespace IAS.Components
{
    public abstract class IAS_Helpers : IAS_Masks
    {
        public uint GetLeftInstruction(ulong word) => (uint)word & MaskFirst20Bits;

        public uint GetRightInstruction(ulong word) => (uint)(word >> 20);

        public byte GetOptCode(uint instruction) => (byte)(instruction & MaskFirst8Bits);

        public static ulong IntTo40ZM(long a) => ((ulong)Math.Abs(a) & MaskFirst39Bits) | (a >= 0 ? 0 : MaskBit40);

        public static long ZM40ToInt(ulong a) => (long)Module(a) * (Sign(a) == 0 ? 1 : -1);

        public static byte Sign(ulong data) => (byte)((data >> 39) & 1);

        public static ulong Module(ulong a) => a & (~MaskBit40);

        public static ulong Add(ulong a, ulong b) => IntTo40ZM(ZM40ToInt(a) + ZM40ToInt(b));

        public static ulong Sub(ulong a, ulong b) => IntTo40ZM(ZM40ToInt(a) - ZM40ToInt(b));
    }
}
