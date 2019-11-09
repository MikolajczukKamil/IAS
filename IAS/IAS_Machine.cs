using System;
using System.Text;
using System.Numerics;

namespace IAS
{
    public abstract class IASMachineException : Exception
    {
        public IASMachineException(string message) : base(message) { }
    }

    public class IASExeciuteException : IASMachineException
    {
        public byte OptCode;

        public IASExeciuteException(string message, byte optCode) : base(message)
        {
            OptCode = optCode;
        }
    }

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
                case IAS_OptCodes.LOAD_MQ:
                    AC = MQ;
                    return;

                case IAS_OptCodes.LOAD_MQ_M:
                    MQ = Memory.GetWord(MAR);
                    return;

                case IAS_OptCodes.STOR_M:
                    SetData(MAR, AC);
                    return;

                case IAS_OptCodes.LOAD_M:
                    AC = Memory.GetWord(MAR);
                    return;

                case IAS_OptCodes.LOAD_DM:
                    AC = IntTo40ZM(-ZM40ToInt(Memory.GetWord(MAR)));
                    return;

                case IAS_OptCodes.LOAD_M_M:
                    AC = Memory.GetWord(MAR) & MaskFirst39Bits;
                    return;

                case IAS_OptCodes.LOAD_D_M_M:
                    AC = (Memory.GetWord(MAR) & MaskFirst39Bits) | MaskBit40;
                    return;

                #endregion transfer

                #region modyfikacja-adresu

                case IAS_OptCodes.STOR_M_L:
                    {
                        ulong oldInstruction = Memory.GetWord(MAR);

                        uint left = GetLeftInstruction(oldInstruction);
                        uint right = GetRightInstruction(oldInstruction);
                        byte opt = GetOptCode(left);
                        ushort newAddress = (ushort)(AC & MaskFirst12Bits);

                        ulong newInstruction = IAS_OptCodes.Word(IAS_OptCodes.Instruction(opt, newAddress), right);

                        SetData(MAR, newInstruction);
                        return;
                    }

                case IAS_OptCodes.STOR_M_R:
                    {
                        ulong oldInstruction = Memory.GetWord(MAR);

                        uint left = GetLeftInstruction(oldInstruction);
                        uint right = GetRightInstruction(oldInstruction);
                        byte opt = GetOptCode(right);
                        ushort newAddress = (ushort)(AC & MaskFirst12Bits);

                        ulong newInstruction = IAS_OptCodes.Word(left, IAS_OptCodes.Instruction(opt, newAddress));

                        SetData(MAR, newInstruction);
                        return;
                    }

                #endregion modyfikacja-adresu

                #region skoki-bezwarunkowe

                case IAS_OptCodes.JUMP_M_L:
                    RightInstruction = false;

                    PC = (ushort)Memory.GetWord(MAR);

                    return;

                case IAS_OptCodes.JUMP_M_R:
                    RightInstruction = true;

                    PC = (ushort)Memory.GetWord(MAR);

                    return;

                // ADDED !

                case IAS_OptCodes.JUMP_L:
                    RightInstruction = false;

                    PC = MAR;

                    return;

                case IAS_OptCodes.JUMP_R:
                    RightInstruction = true;

                    PC = MAR;

                    return;

                #endregion skoki-bezwarunkowe

                #region skoki-warunkowe

                case IAS_OptCodes.JUMP_P_M_L:

                    if (Sign(AC) == 0)
                    {
                        RightInstruction = false;
                        PC = (ushort)Memory.GetWord(MAR);
                    }

                    return;

                case IAS_OptCodes.JUMP_P_M_R:

                    if (Sign(AC) == 0)
                    {
                        RightInstruction = true;
                        PC = (ushort)Memory.GetWord(MAR);
                    }

                    return;

                // ADDED !

                case IAS_OptCodes.JUMP_P_L:

                    if (Sign(AC) == 0)
                    {
                        RightInstruction = false;
                        PC = MAR;
                    }

                    return;

                case IAS_OptCodes.JUMP_P_R:

                    if (Sign(AC) == 0)
                    {
                        RightInstruction = true;
                        PC = MAR;
                    }

                    return;

                #endregion skoki-warunkowe

                #region arytmetyczne

                case IAS_OptCodes.ADD_M:
                    AC = Add(AC, Memory.GetWord(MAR));

                    return;

                case IAS_OptCodes.ADD_M_M:
                    AC = Add(AC, Module(Memory.GetWord(MAR)));

                    return;

                case IAS_OptCodes.SUB_M:
                    AC = Sub(AC, Memory.GetWord(MAR));

                    return;

                case IAS_OptCodes.SUB_M_M:
                    AC = Sub(AC, Module(Memory.GetWord(MAR)));

                    return;

                case IAS_OptCodes.MUL_M:
                    {
                        BigInteger mul = new BigInteger(Memory.GetWord(MAR)) * MQ;

                        if (mul <= ulong.MaxValue)
                        {
                            ulong res = (ulong)mul;
                            MQ = res & MaskFirst40Bits;
                            AC = res >> 40;
                        }
                        else
                        {
                            MQ = (ulong)(mul & MaskFirst40Bits);
                            AC = (ulong)(mul >> 40);
                        }

                        return;
                    }

                case IAS_OptCodes.DIV_M:
                    {
                        ulong word = Memory.GetWord(MAR);

                        MQ = AC / word;
                        AC = AC % word;

                        return;
                    }

                case IAS_OptCodes.LSH:

                    AC = AC << 1;

                    return;

                case IAS_OptCodes.RSH:

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
