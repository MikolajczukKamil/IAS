using System;
using System.Collections.Generic;
using System.Text;

namespace IAS
{
    public abstract class IAS_Helpers
    {
        public static ulong MaskBit40 =         (ulong) 1 << 39;
        public static byte MaskFirst8Bits =     (1 << 8) - 1;
        public static ushort MaskFirst12Bits =  (1 << 12) - 1;
        public static ulong MaskFirst39Bits =   ((ulong) 1 << 39) - 1;
        public static ulong MaskFirst40Bits =   ((ulong) 1 << 40) - 1;
        public static uint MaskFirst20Bits =    (1 << 20) - 1;

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
