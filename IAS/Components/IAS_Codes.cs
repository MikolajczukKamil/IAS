using System;
using IAS.Components;

namespace IAS
{
    using Word = UInt64;
    using Instruction = UInt32;
    using Address = UInt16;
    using Operation = Byte;
    
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
        public const Operation LOAD_MQ = 0b0001010;
        public const Operation LOAD_MQ_M = 0b0001001;
        public const Operation STOR_M = 0b0100001;
        public const Operation LOAD_M = 0b0000001;
        public const Operation LOAD_DM = 0b0000010;
        public const Operation LOAD_M_M = 0b0000011;
        public const Operation LOAD_D_M_M = 0b0000100;

        public const Operation STOR_M_L = 0b0010010;
        public const Operation STOR_M_R = 0b0010011;
        public const Operation JUMP_M_L = 0b0001101;
        public const Operation JUMP_M_R = 0b0001110;
        public const Operation JUMP_L = 0b1001101; // Addded Jump to address
        public const Operation JUMP_R = 0b1001110; // Addded Jump to address

        public const Operation JUMP_P_M_L = 0b0001111;
        public const Operation JUMP_P_M_R = 0b0010000;
        public const Operation JUMP_P_L = 0b1001111; // Addded Jump to address
        public const Operation JUMP_P_R = 0b1010000; // Addded Jump to address

        public const Operation ADD_M = 0b0000101;
        public const Operation ADD_M_M = 0b0000111;
        public const Operation SUB_M = 0b0000110;
        public const Operation SUB_M_M = 0b0001000;
        public const Operation MUL_M = 0b0001011;
        public const Operation DIV_M = 0b0001100;
        public const Operation LSH = 0b0010101;
        public const Operation RSH = 0b0100010;

        public static Instruction Instruction(Operation optCode, Address address = 0)
        {
            Instruction instrution = ((Instruction)address) << 8;

            instrution |= optCode;

            return instrution & IAS_Masks.MaskFirst20Bits;
        }

        public static Word Word(Instruction leftInstruction, Instruction rightInstruction)
        {
            leftInstruction &= IAS_Masks.MaskFirst20Bits;
            rightInstruction &= IAS_Masks.MaskFirst20Bits;

            Word instrution = leftInstruction;

            instrution |= ((Word)rightInstruction) << 20;

            return instrution;
        }

        public static Word Word(Word data) => data & IAS_Masks.MaskFirst40Bits;

        public static Word Word(long data)
        {
            if (data >= 0) return Word((Word)data);

            data *= -1;

            return ((Word)data) & IAS_Masks.MaskFirst40Bits | IAS_Masks.MaskBit40;
        }

        public static Word Word() => 0;
    };
}
