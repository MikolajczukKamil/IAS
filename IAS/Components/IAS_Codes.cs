using IAS.Components;

namespace IAS
{
    /**
     * 
     * It is recommended to inherit from OptCodes
     * 
     * @example
     * 
     * [
     *  Word(1),                   // 0
     *  Word(
     *    Instruction(LOAD_M, 0),  // 1L
     *    Instruction(ADD_M, 0)    // 1R
     *  )
     * ]
     * 
     **/
    public class IAS_Codes
    {
        public const byte LOAD_MQ = 0b0001010;
        public const byte LOAD_MQ_M = 0b0001001;
        public const byte STOR_M = 0b0100001;
        public const byte LOAD_M = 0b0000001;
        public const byte LOAD_DM = 0b0000010;
        public const byte LOAD_M_M = 0b0000011;
        public const byte LOAD_D_M_M = 0b0000100;

        public const byte STOR_M_L = 0b0010010;
        public const byte STOR_M_R = 0b0010011;
        public const byte JUMP_M_L = 0b0001101;
        public const byte JUMP_M_R = 0b0001110;
        public const byte JUMP_L = 0b1001101; // Addded Jump to address
        public const byte JUMP_R = 0b1001110; // Addded Jump to address

        public const byte JUMP_P_M_L = 0b0001111;
        public const byte JUMP_P_M_R = 0b0010000;
        public const byte JUMP_P_L = 0b1001111; // Addded Jump to address
        public const byte JUMP_P_R = 0b1010000; // Addded Jump to address

        public const byte ADD_M = 0b0000101;
        public const byte ADD_M_M = 0b0000111;
        public const byte SUB_M = 0b0000110;
        public const byte SUB_M_M = 0b0001000;
        public const byte MUL_M = 0b0001011;
        public const byte DIV_M = 0b0001100;
        public const byte LSH = 0b0010101;
        public const byte RSH = 0b0100010;

        public static uint Instruction(byte optCode, ushort address = 0)
        {
            uint instrution = ((uint)address) << 8;

            instrution |= optCode;

            return instrution & IAS_Masks.MaskFirst20Bits;
        }

        public static ulong Word(uint leftInstruction, uint rightInstruction)
        {
            leftInstruction &= IAS_Masks.MaskFirst20Bits;
            rightInstruction &= IAS_Masks.MaskFirst20Bits;

            ulong instrution = leftInstruction;

            instrution |= ((ulong)rightInstruction) << 20;

            return instrution;
        }

        public static ulong Word(ulong data) => data & IAS_Masks.MaskFirst40Bits;

        public static ulong Word(long data)
        {
            if (data >= 0) return Word((ulong)data);

            data *= -1;

            return ((ulong)data) & IAS_Masks.MaskFirst40Bits | IAS_Masks.MaskBit40;
        }

        public static ulong Word() => 0;
    };
}
