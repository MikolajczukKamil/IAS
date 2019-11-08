using System;
using System.Collections.Generic;
using System.Text;

namespace Symulator
{
    public abstract class IAS_Helpers
    {
        public static ulong BitsMaskBit40 =         (ulong) 1 << 39;
        public static byte BitsMaskFirst8Bits =     (1 << 8) - 1;
        public static ushort BitsMaskFirst12Bits =  (1 << 12) - 1;
        public static ulong BitsMaskFirst39Bits =   ((ulong) 1 << 39) - 1;
        public static ulong BitsMaskFirst40Bits =   ((ulong) 1 << 40) - 1;
        public static uint BitsMaskFirst20Bits =    (1 << 20) - 1;

        public static byte Module(ulong data) => (byte)((data >> 39) & 1);

        public static ulong Value(ulong data) => data & BitsMaskFirst39Bits;

        public static ulong IntTo40ZM(long a) => ((ulong) Math.Abs(a) & BitsMaskFirst39Bits) | (a >= 0 ? 0 : BitsMaskBit40);

        public static long ZM40ToInt(ulong a) => (long) Value(a) * (Module(a) == 0 ? 1 : -1);

        public static ulong ToModuleValue(ulong a) => a & (~BitsMaskBit40);

        protected static ulong Add(ulong a, ulong b) => IntTo40ZM(ZM40ToInt(a) + ZM40ToInt(b));

        protected static ulong Sub(ulong a, ulong b) => IntTo40ZM(ZM40ToInt(a) - ZM40ToInt(b));
    }
}
