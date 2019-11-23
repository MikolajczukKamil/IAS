using System;
using System.Text;
using System.Numerics;
using IAS.Components;

namespace IAS
{
    using Word = Int64;
    using Instruction = UInt32;
    using Address = UInt16;
    using Operation = Byte;

    public class IAS_Machine : IAS_Helpers
    {
        // AC
        Word AC = 0;            // 40 bit
        Word MQ = 0;            // 40 bit
        Word MBR = 0;           // 40 bit

        // CC
        Instruction IBR = 0;    // 20 bit
        Address PC = 0;         // 12 bit
        Address MAR = 0;        // 12 bit
        Operation IR = 0;       //  8 bit

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

        public void Step()
        {
            Fetch();
            Decode();
            Execiute();
        }

        void Fetch()
        {
            MAR = PC;

            ReadMemory();

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
                    ReadMemory();
                    MQ = MBR;
                    return;

                case IAS_Codes.STOR_M:
                    MBR = AC;
                    WriteMemory();
                    return;

                case IAS_Codes.LOAD_M:
                    ReadMemory();
                    AC = MBR;
                    return;

                case IAS_Codes.LOAD_D_M:
                    ReadMemory();
                    AC = -MBR;
                    return;

                case IAS_Codes.LOAD_M_M:
                    ReadMemory();
                    AC = Math.Abs(MBR);
                    return;

                case IAS_Codes.LOAD_D_M_M:
                    ReadMemory();
                    AC = -Math.Abs(MBR);
                    return;

                #endregion transfer

                #region modyfikacja-adresu

                case IAS_Codes.STOR_M_L:
                    {
                        ReadMemory();

                        Instruction left = GetLeftInstruction(MBR);
                        Instruction right = GetRightInstruction(MBR);

                        Address newAddress = (Address)(AC & MaskFirst12Bits);
                        MBR = IAS_Codes.Word(IAS_Codes.Instruction(GetOpCode(left), newAddress), right);

                        WriteMemory();

                        return;
                    }

                case IAS_Codes.STOR_M_R:
                    {
                        ReadMemory();

                        Instruction left = GetLeftInstruction(MBR);
                        Instruction right = GetRightInstruction(MBR);

                        Address newAddress = (Address)(AC & MaskFirst12Bits);
                        MBR = IAS_Codes.Word(left, IAS_Codes.Instruction(GetOpCode(right), newAddress));

                        WriteMemory();

                        return;
                    }

                #endregion modyfikacja-adresu

                #region skoki-bezwarunkowe

                case IAS_Codes.JUMP_M_L:
                    RightInstruction = false;

                    ReadMemory();

                    PC = (Address)MBR;

                    return;

                case IAS_Codes.JUMP_M_R:
                    RightInstruction = true;

                    ReadMemory();

                    PC = (Address)MBR;

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

                    if (AC >= 0)
                    {
                        RightInstruction = false;

                        ReadMemory();

                        PC = (Address)MBR;
                    }

                    return;

                case IAS_Codes.JUMP_P_M_R:

                    if (AC >= 0)
                    {
                        RightInstruction = true;

                        ReadMemory();

                        PC = (Address)MBR;
                    }

                    return;

                // ADDED !

                case IAS_Codes.JUMP_P_L:

                    if (AC >= 0)
                    {
                        RightInstruction = false;
                        PC = MAR;
                    }

                    return;

                case IAS_Codes.JUMP_P_R:

                    if (AC >= 0)
                    {
                        RightInstruction = true;
                        PC = MAR;
                    }

                    return;

                #endregion skoki-warunkowe

                #region arytmetyczne

                case IAS_Codes.ADD_M:
                    ReadMemory();

                    AC = To40BitsValue(AC + MBR);

                    return;

                case IAS_Codes.ADD_M_M:
                    ReadMemory();

                    AC = To40BitsValue(AC + Math.Abs(MBR));

                    return;

                case IAS_Codes.SUB_M:
                    ReadMemory();

                    AC = To40BitsValue(AC - MBR);

                    return;

                case IAS_Codes.SUB_M_M:
                    ReadMemory();

                    AC = To40BitsValue(AC - Math.Abs(MBR));

                    return;

                case IAS_Codes.MUL_M:
                    {
                        ReadMemory();

                        BigInteger mul = new BigInteger(MBR) * MQ;

                        MQ = To40BitsValue((Word)(mul & MaskFirst40Bits));

                        AC = To40BitsValue((Word)(mul >> 40));

                        return;
                    }

                case IAS_Codes.DIV_M:
                    {
                        ReadMemory();

                        if(MBR == 0)
                            throw new IASExeciuteException("Divide by zero", IR);

                        MQ = AC / MBR;
                        AC = AC % MBR;

                        return;
                    }

                case IAS_Codes.LSH:

                    AC <<= 1;

                    return;

                case IAS_Codes.RSH:

                    AC >>= 1;

                    return;

                    #endregion arytmetyczne
            }

            throw new IASExeciuteException($"Operation not found 0b{Convert.ToString(IR, 2).PadLeft(8, '0')}", IR);
        }

        void ReadMemory()
        {
            MBR = Memory.GetWord(MAR);
        }

        void WriteMemory()
        {
            Memory.SetWord(MAR, MBR);
        }

        public Word[] GetMemory(bool copy = true) => Memory.GetInstructions(copy);

        public override string ToString() => ToString(-1);

        public string ToString(int manyInstructions)
        {
            StringBuilder description = new StringBuilder();

            description.AppendLine($" IC: {PC}{(RightInstruction ? 'R' : 'L')}");
            description.AppendLine($" AC: {AC}");
            description.AppendLine($" MQ: {MQ}");
            description.AppendLine($" Memory:");

            description.AppendLine(Memory.ToString(manyInstructions));

            return description.ToString();
        }
    }
}
