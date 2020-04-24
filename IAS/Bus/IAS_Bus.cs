using System;

namespace IAS.Bus
{
    using Word = Int64;
    using AddressT = UInt16;
    using Operation = Byte;

    /// <summary>
    /// IAS component - Bus
    /// </summary>
    class IAS_Bus
    {
        /// <summary>
        /// Address of value - Address (12 bit)
        /// </summary>
        public AddressT Address;

        /// <summary>
        /// Word of data - Word (40 bit)
        /// </summary>
        public Word Data;

        /// <summary>
        /// Operation code - Operation (8 bit)
        /// </summary>
        public Operation Control;
    }
}
