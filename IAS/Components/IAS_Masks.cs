using System;

namespace IAS.Components
{
    public class IAS_Masks
    {
        public static long Bit40 = (long)1 << 39;
        public static byte First8Bits = (1 << 8) - 1;
        public static ushort First12Bits = (1 << 12) - 1;
        public static long First39Bits = ((long)1 << 39) - 1;
        public static long First40Bits = ((long)1 << 40) - 1;
        public static uint First20Bits = (1 << 20) - 1;
    }
}
