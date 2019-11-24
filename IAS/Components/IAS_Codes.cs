using System;
using IAS.Components;

namespace IAS
{
    using Word = Int64;
    using Instruction = UInt32;
    using Address = UInt16;
    using Operation = Byte;
    
    public class IAS_Codes : IAS_Helpers
    {
        // Data transfer
        public const Operation LOAD_M =     1;
        public const Operation LOAD_D_M =   2;
        public const Operation LOAD_M_M =   3;
        public const Operation LOAD_D_M_M = 4;
        public const Operation LOAD_MQ =    10;
        public const Operation LOAD_MQ_M =  9;
        public const Operation STOR_M =     33;

        // Address modification
        public const Operation STOR_M_L =   18;
        public const Operation STOR_M_R =   19;

        // Unconditional jumps
        public const Operation JUMP_M_L =   13;
        public const Operation JUMP_L =     13 + 128; // Addded Jump to address
        public const Operation JUMP_M_R =   14;
        public const Operation JUMP_R =     14 + 128; // Addded Jump to address

        // Conditional jumps
        public const Operation JUMP_P_M_L = 15;
        public const Operation JUMP_P_L =   15 + 128; // Addded Jump to address
        public const Operation JUMP_P_M_R = 16;
        public const Operation JUMP_P_R =   16 + 128; // Addded Jump to address

        // Arithmetic
        public const Operation ADD_M =      5;
        public const Operation ADD_M_M =    7;
        public const Operation SUB_M =      6;
        public const Operation SUB_M_M =    8;
        public const Operation MUL_M =      11;
        public const Operation DIV_M =      12;
        public const Operation LSH =        20;
        public const Operation RSH =        21;

        public static Instruction Instruction(Operation opCode, Address address = 0)
        {
            Instruction instrution = ((Instruction)address) << 8;

            instrution |= opCode;

            return instrution & IAS_Masks.First20Bits;
        }

        public static Word Word(Instruction leftInstruction, Instruction rightInstruction)
        {
            leftInstruction &= IAS_Masks.First20Bits;
            rightInstruction &= IAS_Masks.First20Bits;

            Word instrution = leftInstruction;

            instrution |= ((Word)rightInstruction) << 20;

            return instrution;
        }

        public static Word Word(long data) => To40BitsValue(data);

        public static Word Word() => 0;
    };
}
