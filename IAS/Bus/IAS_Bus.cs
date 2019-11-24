using System;

namespace IAS.Bus
{
    using Word = Int64;
    using AddressT = UInt16;
    using Operation = Byte;

    class IAS_Bus
    {
        public AddressT Address;

        public Word Data;

        public Operation Control;
    }
}
