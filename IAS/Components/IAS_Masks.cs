using System;

namespace IAS.Components
{
    public class IAS_Masks
    {
        public static long MaskBit40 = (long)1 << 39;
        public static byte MaskFirst8Bits = (1 << 8) - 1;
        public static ushort MaskFirst12Bits = (1 << 12) - 1;
        public static long MaskFirst39Bits = ((long)1 << 39) - 1;
        public static long MaskFirst40Bits = ((long)1 << 40) - 1;
        public static uint MaskFirst20Bits = (1 << 20) - 1;
    }
}
