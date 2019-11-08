using System;
using System.Collections.Generic;
using System.Text;

namespace Symulator
{
    public abstract class IAS_Helpers
    {
        public static ulong BitsMaskBit40 =         1 << 39;
        public static byte BitsMaskFirst8Bits =     (1 << 8) - 1;
        public static ushort BitsMaskFirst12Bits =  (1 << 12) - 1;
        public static ulong BitsMaskFirst39Bits =   (1 << 39) - 1;
        public static ulong BitsMaskFirst40Bits =   (1 << 40) - 1;
        public static uint BitsMaskFirst20Bits =    (1 << 20) - 1;

        public static byte Module(ulong data) => (byte)((data >> 39) & 1);

        public static ulong Value(ulong data) => data & BitsMaskFirst39Bits;

        public static ulong IntTo40ZM(long a) => ((ulong)a & BitsMaskFirst39Bits) | (a >= 0 ? 0 : BitsMaskBit40);

        public static long From40ZMToInt(ulong a) => (long)Value(a) * (Module(a) == 0 ? 1 : -1);

        protected static ulong Add(ulong a, ulong b) => IntTo40ZM(From40ZMToInt(a) + From40ZMToInt(b));

        protected static ulong Sub(ulong a, ulong b) => IntTo40ZM(From40ZMToInt(a) - From40ZMToInt(b));
    }
}
