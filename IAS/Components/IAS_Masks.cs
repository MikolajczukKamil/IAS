using System;

namespace IAS.Components
{
    public class IAS_Masks
    {
        public static ulong MaskBit40 = (ulong)1 << 39;
        public static byte MaskFirst8Bits = (1 << 8) - 1;
        public static ushort MaskFirst12Bits = (1 << 12) - 1;
        public static ulong MaskFirst39Bits = ((ulong)1 << 39) - 1;
        public static ulong MaskFirst40Bits = ((ulong)1 << 40) - 1;
        public static uint MaskFirst20Bits = (1 << 20) - 1;
    }
}
