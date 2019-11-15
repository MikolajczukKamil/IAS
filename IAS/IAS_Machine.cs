using System;
using System.Text;
using System.Numerics;
using IAS.Components;

namespace IAS
{
    using Word = UInt64;
    using Instruction = UInt32;
    using Address = UInt16;
    using Operation = Byte;

    public class IAS_Machine : IAS_Helpers
    {
        // AC
        Word AC = 0;   // 40 bit
        Word MQ = 0;   // 40 bit
        //Word MBR = 0;  // 40 bit

        // CC
        Instruction IBR = 0;  // 20 bit
        Address PC = 0;        // 12 bit
        Address MAR = 0;       // 12 bit
        Operation IR = 0;     // 8  bit

        IAS_Memory Memory;

        bool RightInstruction = false;

        public IAS_Machine(Word[] code, bool copy = true)
        {
            Memory = new IAS_Memory(code, copy);
        }

        public void ManualJumpTo(Address address)
        {
            PC = address;
        }

        void SetData(Address address, Word data) => Memory.SetWord(address, data);

        public void Step()
        {
            Fetch();
            Decode();
            Execiute();
        }

        void Fetch()
        {
            Word MBR = Memory.GetWord(PC);

            if (RightInstruction)
                PC++;

            IBR = RightInstruction ? GetRightInstruction(MBR) : GetLeftInstruction(MBR);

            RightInstruction = !RightInstruction;
        }

        void Decode()
        {
            IR = (Operation)(IBR & MaskFirst8Bits);
            MAR = (Address)(IBR >> 8);
        }

        void Execiute()
        {
            switch (IR)
            {
                #region transfer
                case IAS_Codes.LOAD_MQ:
                    AC = MQ;
                    return;

                case IAS_Codes.LOAD_MQ_M:
                    MQ = Memory.GetWord(MAR);
                    return;

                case IAS_Codes.STOR_M:
                    SetData(MAR, AC);
                    return;

                case IAS_Codes.LOAD_M:
                    AC = Memory.GetWord(MAR);
                    return;

                case IAS_Codes.LOAD_DM:
                    AC = IntTo40ZM(-ZM40ToInt(Memory.GetWord(MAR)));
                    return;

                case IAS_Codes.LOAD_M_M:
                    AC = Memory.GetWord(MAR) & MaskFirst39Bits;
                    return;

                case IAS_Codes.LOAD_D_M_M:
                    AC = (Memory.GetWord(MAR) & MaskFirst39Bits) | MaskBit40;
                    return;

                #endregion transfer

                #region modyfikacja-adresu

                case IAS_Codes.STOR_M_L:
                    {
                        Word oldInstruction = Memory.GetWord(MAR);

                        Instruction left = GetLeftInstruction(oldInstruction);
                        Instruction right = GetRightInstruction(oldInstruction);
                        Operation opt = GetOptCode(left);
                        Address newAddress = (Address)(AC & MaskFirst12Bits);

                        Word newInstruction = IAS_Codes.Word(IAS_Codes.Instruction(opt, newAddress), right);

                        SetData(MAR, newInstruction);
                        return;
                    }

                case IAS_Codes.STOR_M_R:
                    {
                        Word oldInstruction = Memory.GetWord(MAR);

                        Instruction left = GetLeftInstruction(oldInstruction);
                        Instruction right = GetRightInstruction(oldInstruction);
                        Operation opt = GetOptCode(right);
                        Address newAddress = (Address)(AC & MaskFirst12Bits);

                        Word newInstruction = IAS_Codes.Word(left, IAS_Codes.Instruction(opt, newAddress));

                        SetData(MAR, newInstruction);
                        return;
                    }

                #endregion modyfikacja-adresu

                #region skoki-bezwarunkowe

                case IAS_Codes.JUMP_M_L:
                    RightInstruction = false;

                    PC = (Address)Memory.GetWord(MAR);

                    return;

                case IAS_Codes.JUMP_M_R:
                    RightInstruction = true;

                    PC = (Address)Memory.GetWord(MAR);

                    return;

                // ADDED !

                case IAS_Codes.JUMP_L:
                    RightInstruction = false;

                    PC = MAR;

                    return;

                case IAS_Codes.JUMP_R:
                    RightInstruction = true;

                    PC = MAR;

                    return;

                #endregion skoki-bezwarunkowe

                #region skoki-warunkowe

                case IAS_Codes.JUMP_P_M_L:

                    if (Sign(AC) == 0)
                    {
                        RightInstruction = false;
                        PC = (Address)Memory.GetWord(MAR);
                    }

                    return;

                case IAS_Codes.JUMP_P_M_R:

                    if (Sign(AC) == 0)
                    {
                        RightInstruction = true;
                        PC = (Address)Memory.GetWord(MAR);
                    }

                    return;

                // ADDED !

                case IAS_Codes.JUMP_P_L:

                    if (Sign(AC) == 0)
                    {
                        RightInstruction = false;
                        PC = MAR;
                    }

                    return;

                case IAS_Codes.JUMP_P_R:

                    if (Sign(AC) == 0)
                    {
                        RightInstruction = true;
                        PC = MAR;
                    }

                    return;

                #endregion skoki-warunkowe

                #region arytmetyczne

                case IAS_Codes.ADD_M:
                    AC = Add(AC, Memory.GetWord(MAR));

                    return;

                case IAS_Codes.ADD_M_M:
                    AC = Add(AC, Module(Memory.GetWord(MAR)));

                    return;

                case IAS_Codes.SUB_M:
                    AC = Sub(AC, Memory.GetWord(MAR));

                    return;

                case IAS_Codes.SUB_M_M:
                    AC = Sub(AC, Module(Memory.GetWord(MAR)));

                    return;

                case IAS_Codes.MUL_M:
                    {
                        BigInteger mul = new BigInteger(ZM40ToInt(Memory.GetWord(MAR))) * ZM40ToInt(MQ);

                        AC = mul < 0 ? MaskBit40 : 0;

                        mul *= mul.Sign;

                        MQ = (Word)(mul & MaskFirst40Bits);
                        AC = AC | (Word)(mul >> 40);

                        return;
                    }

                case IAS_Codes.DIV_M:
                    {
                        long divisor = ZM40ToInt(Memory.GetWord(MAR));

                        if(divisor == 0)
                        {
                            MQ = IntTo40ZM(ZM40ToInt(MaskFirst39Bits) * Math.Sign(ZM40ToInt(AC)));
                            AC = MaskFirst40Bits;
                            return;
                        }

                        MQ = IntTo40ZM(ZM40ToInt(AC) / divisor);
                        AC = IntTo40ZM(ZM40ToInt(AC) % divisor);

                        return;
                    }

                case IAS_Codes.LSH:

                    AC = AC << 1;

                    return;

                case IAS_Codes.RSH:

                    AC = AC >> 1;

                    return;

                    #endregion arytmetyczne
            }

            throw new IASExeciuteException($"Operation not found 0b{Convert.ToString(IR, 2).PadLeft(8, '0')}", IR);
        }

        public Word[] GetMemory(bool copy = true) => Memory.GetInstructions(copy);

        public override string ToString() => ToString(-1);

        public string ToString(int manyInstructions)
        {
            StringBuilder description = new StringBuilder();

            description.AppendLine($" IC: {PC}{(RightInstruction ? 'R' : 'L')}");
            description.AppendLine($" AC: {ZM40ToInt(AC)}");
            description.AppendLine($" MQ: {ZM40ToInt(MQ)}");
            description.AppendLine($" Memory:");

            description.AppendLine(Memory.ToString(manyInstructions));

            return description.ToString();
        }
    }
}
