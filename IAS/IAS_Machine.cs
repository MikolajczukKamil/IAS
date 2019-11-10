﻿using System;
using System.Text;
using System.Numerics;
using IAS.Components;

namespace IAS
{
    public class IAS_Machine : IAS_Helpers
    {
        // AC
        ulong AC = 0;  // 40 bit
        ulong MQ = 0;  // 40 bit
        // ulong   MBR = 0;  // 40 bit

        // CC
        uint IBR = 0;  // 20 bit
        ushort PC = 0;  // 12 bit
        ushort MAR = 0;  // 12 bit
        byte IR = 0;  // 8  bit

        IAS_Memory Memory;

        bool RightInstruction = false;

        public IAS_Machine(ulong[] instructions, bool copyInstructions = true)
        {
            Memory = new IAS_Memory(instructions, copyInstructions);
        }

        public void ManualJumpTo(ushort pos)
        {
            PC = pos;
        }

        void SetData(ushort address, ulong data) => Memory.SetWord(address, data);

        public void Step()
        {
            Fetch();
            Decode();
            Execiute();
        }

        void Fetch()
        {
            ulong MBR = Memory.GetWord(PC);

            if (RightInstruction)
                PC++;

            IBR = RightInstruction ? GetRightInstruction(MBR) : GetLeftInstruction(MBR);

            RightInstruction = !RightInstruction;
        }

        void Decode()
        {
            IR = (byte)(IBR & MaskFirst8Bits);
            MAR = (ushort)(IBR >> 8);
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
                        ulong oldInstruction = Memory.GetWord(MAR);

                        uint left = GetLeftInstruction(oldInstruction);
                        uint right = GetRightInstruction(oldInstruction);
                        byte opt = GetOptCode(left);
                        ushort newAddress = (ushort)(AC & MaskFirst12Bits);

                        ulong newInstruction = IAS_Codes.Word(IAS_Codes.Instruction(opt, newAddress), right);

                        SetData(MAR, newInstruction);
                        return;
                    }

                case IAS_Codes.STOR_M_R:
                    {
                        ulong oldInstruction = Memory.GetWord(MAR);

                        uint left = GetLeftInstruction(oldInstruction);
                        uint right = GetRightInstruction(oldInstruction);
                        byte opt = GetOptCode(right);
                        ushort newAddress = (ushort)(AC & MaskFirst12Bits);

                        ulong newInstruction = IAS_Codes.Word(left, IAS_Codes.Instruction(opt, newAddress));

                        SetData(MAR, newInstruction);
                        return;
                    }

                #endregion modyfikacja-adresu

                #region skoki-bezwarunkowe

                case IAS_Codes.JUMP_M_L:
                    RightInstruction = false;

                    PC = (ushort)Memory.GetWord(MAR);

                    return;

                case IAS_Codes.JUMP_M_R:
                    RightInstruction = true;

                    PC = (ushort)Memory.GetWord(MAR);

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
                        PC = (ushort)Memory.GetWord(MAR);
                    }

                    return;

                case IAS_Codes.JUMP_P_M_R:

                    if (Sign(AC) == 0)
                    {
                        RightInstruction = true;
                        PC = (ushort)Memory.GetWord(MAR);
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

                        MQ = (ulong)(mul & MaskFirst40Bits);
                        AC = AC | (ulong)(mul >> 40);

                        return;
                    }

                case IAS_Codes.DIV_M:
                    {
                        long word = ZM40ToInt(Memory.GetWord(MAR));

                        if(word == 0)
                        {
                            MQ = IntTo40ZM(ZM40ToInt(MaskFirst39Bits) * Math.Sign(ZM40ToInt(AC)));
                            AC = 0;
                            return;
                        }

                        MQ = IntTo40ZM(ZM40ToInt(AC) / word);
                        AC = IntTo40ZM(ZM40ToInt(AC) % word);

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

        public override string ToString() => ToString(-1);

        public string ToString(short manyInstructions)
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