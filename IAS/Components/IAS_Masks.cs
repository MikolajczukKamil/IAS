namespace IAS.Components
{
    /// <summary>
    /// IAS helper, binary masks
    /// </summary>
    public class IAS_Masks
    {
        /// <summary>
        /// Mask - 1 on 40 position - 1000000000 0000000000 0000000000 0000000000
        /// </summary>
        public const long Bit40 = (long)1 << 39;

        /// <summary>
        /// Mask - 1's on first 8 bits - 0000000000 0000000000 0000000000 0011111111
        /// </summary>
        public const byte First8Bits = (1 << 8) - 1;

        /// <summary>
        /// Mask - 1's on first 12 bits - 0000000000 0000000000 0000000011 1111111111
        /// </summary>
        public const ushort First12Bits = (1 << 12) - 1;

        /// <summary>
        /// Mask - 1's on first 39 bits - 0111111111 1111111111 1111111111 1111111111
        /// </summary>
        public const long First39Bits = ((long)1 << 39) - 1;

        /// <summary>
        /// Mask - 1's on first 40 bits - 1111111111 1111111111 1111111111 1111111111
        /// </summary>
        public const long First40Bits = ((long)1 << 40) - 1;

        /// <summary>
        /// Mask - 1's on first 20 bits - 0000000000 0000000000 1111111111 1111111111
        /// </summary>
        public const uint First20Bits = (1 << 20) - 1;
    }
}
