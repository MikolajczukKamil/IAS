using System;
using IAS.Components;

namespace IAS
{
    using Word = Int64;
    using Instruction = UInt32;
    using Address = UInt16;
    using Operation = Byte;
    
    /// <summary>
    /// IAS helper, operations codes, generating machine code
    /// </summary>
    public class IAS_Codes : IAS_Helpers
    {
        #region Codes of instructions

        #region Data transfer

        /// <summary>
        /// LOAD M(X) -> AC = M(X)
        /// </summary>
        public const Operation LOAD_M = 1;

        /// <summary>
        /// LOAD -M(X) -> AC = -M(X)
        /// </summary>
        public const Operation LOAD_D_M = 2;

        /// <summary>
        /// LOAD |M(X)| -> AC = |M(X)|
        /// </summary>
        public const Operation LOAD_M_M = 3;

        /// <summary>
        /// LOAD -|M(X)| -> AC = -|M(X)|
        /// </summary>
        public const Operation LOAD_D_M_M = 4;

        /// <summary>
        /// LOAD MQ -> AC = MQ
        /// </summary>
        public const Operation LOAD_MQ = 10;

        /// <summary>
        /// LOAD MQM(X) -> MQ = M(X)
        /// </summary>
        public const Operation LOAD_MQ_M = 9;

        /// <summary>
        /// STOR M(X) -> M(X) = AC
        /// </summary>
        public const Operation STOR_M = 33;

        #endregion

        #region Address modification

        /// <summary>
        /// STOR M(X, 8:19) -> zamień adres lewego rozkazu M(X) na 12 prawych bitów AC
        /// </summary>
        public const Operation STOR_M_L = 18;

        /// <summary>
        /// STOR M(X, 28:39) -> zamień adres prawego rozkazu M(X) na 12 prawych bitów AC
        /// </summary>
        public const Operation STOR_M_R = 19;

        #endregion

        #region Unconditional jumps

        /// <summary>
        /// JUMP M(X, 0:19) -> skocz do lewego rozkazu M(X)
        /// </summary>
        public const Operation JUMP_M_L = 13;

        /// <summary>
        /// JUMP (X, 0:19) -> skocz do lewego rozkazu X
        /// * Addded, not in the original!
        /// </summary>
        public const Operation JUMP_L = 13 + 128;

        /// <summary>
        /// JUMP M(X, 20:39) -> skocz do prawego rozkazu M(X)
        /// </summary>
        public const Operation JUMP_M_R = 14;

        /// <summary>
        /// JUMP (X, 0:19) -> skocz do prawego rozkazu X
        /// * Addded, not in the original!
        /// </summary>
        public const Operation JUMP_R = 14 + 128;

        #endregion

        #region Conditional jumps

        /// <summary>
        /// JUMP + M(X, 0:19) -> jeżeli AC >= 0 skocz do lewego rozkazu M(X)
        /// </summary>
        public const Operation JUMP_P_M_L = 15;

        /// <summary>
        /// JUMP + (X, 0:19) -> jeżeli AC >= 0 skocz do lewego rozkazu X
        /// * Addded, not in the original!
        /// </summary>
        public const Operation JUMP_P_L = 15 + 128; // Addded Jump to address

        /// <summary>
        /// JUMP + M(X, 20:39) -> jeżeli AC >= 0 skocz do prawego rozkazu M(X)
        /// </summary>
        public const Operation JUMP_P_M_R = 16;

        /// <summary>
        /// JUMP + (X, 0:19) -> jeżeli AC >= 0 skocz do prawego rozkazu X
        /// * Addded, not in the original!
        /// </summary>
        public const Operation JUMP_P_R = 16 + 128; // Addded Jump to address

        #endregion

        #region Arithmetic

        /// <summary>
        /// ADD M(X) -> AC = AC + M(X)
        /// </summary>
        public const Operation ADD_M = 5;

        /// <summary>
        /// ADD |M(X)| -> AC = AC + |M(X)|
        /// </summary>
        public const Operation ADD_M_M = 7;

        /// <summary>
        /// SUB M(X) -> AC = AC - M(X)
        /// </summary>
        public const Operation SUB_M = 6;

        /// <summary>
        /// SUB |M(X)| -> AC = AC - |M(X)|
        /// </summary>
        public const Operation SUB_M_M = 8;

        /// <summary>
        /// MUL M(X) -> AC:MQ = MQ * M(X)
        /// </summary>
        public const Operation MUL_M = 11;

        /// <summary>
        /// DIV M(X) -> MQ = AC / M(X); AC = AC % M(X)
        /// </summary>
        public const Operation DIV_M = 12;

        /// <summary>
        /// LSH -> AC = AC << 1
        /// </summary>
        public const Operation LSH = 20;

        /// <summary>
        /// RSH -> AC = AC >> 1
        /// </summary>
        public const Operation RSH = 21;

        #endregion

        #endregion

        #region Generating machine code

        /// <summary>
        /// Geterate part of machine code part - instruction - half of word
        /// </summary>
        /// <param name="operationCode">Operation code</param>
        /// <param name="address">Address</param>
        /// <returns>Instruction of machine code</returns>
        public static Instruction Instruction(Operation operationCode, Address address = 0)
        {
            // Instruction = [operationCode, address]

            address &= IAS_Masks.First12Bits;

            Instruction instrution = ((Instruction)operationCode) << 12;

            instrution |= address;

            return instrution;
        }

        /// <summary>
        /// Geterate part of machine code part - full word
        /// </summary>
        /// <param name="leftInstruction">First instruction</param>
        /// <param name="rightInstruction">Second instruction</param>
        /// <returns>Word of machine code</returns>
        public static Word Word(Instruction leftInstruction, Instruction rightInstruction)
        {
            // Word = [leftInstruction, rightInstruction]

            leftInstruction &= IAS_Masks.First20Bits;
            rightInstruction &= IAS_Masks.First20Bits;

            Word instrutionWord = ((Word)leftInstruction) << 20;

            instrutionWord |= rightInstruction;

            return instrutionWord;
        }

        /// <summary>
        /// Translate constant into word of machine code
        /// </summary>
        /// <param name="data">Constant</param>
        /// <returns>Word of machine code</returns>
        public static Word Word(long data) => To40BitsValue(data);

        /// <summary>
        /// Empty Word - 0
        /// </summary>
        /// <returns>Word of machine code</returns>
        public static Word Word()
        {
            return 0;
        } 

        #endregion
    };
}
