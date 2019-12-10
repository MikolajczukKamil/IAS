using System;
using System.Text;
using System.Numerics;
using System.Linq;

using IAS.Bus;
using IAS.Memory;
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
        IAS_Bus Bus = new IAS_Bus();

        bool RightInstruction = false;

        public IAS_Machine(Word[] code, bool copy = true)
        {
            Memory = new IAS_Memory(code, Bus, copy);
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
            IR = GetOpCode(IBR);
            MAR = GetAddress(IBR);
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

                        Address newAddress = (Address)(AC & IAS_Masks.First12Bits);
                        MBR = IAS_Codes.Word(IAS_Codes.Instruction(GetOpCode(left), newAddress), right);

                        WriteMemory();

                        return;
                    }

                case IAS_Codes.STOR_M_R:
                    {
                        ReadMemory();

                        Instruction left = GetLeftInstruction(MBR);
                        Instruction right = GetRightInstruction(MBR);

                        Address newAddress = (Address)(AC & IAS_Masks.First12Bits);
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

                        MQ = To40BitsValue((Word)(mul & IAS_Masks.First40Bits));

                        AC = To40BitsValue((Word)(mul >> 40) & IAS_Masks.First40Bits);

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
            Bus.Address = MAR;
            Bus.Control = IAS_Memory.MR;

            Memory.Do();

            MBR = Bus.Data;
        }

        void WriteMemory()
        {
            Bus.Data = MBR;
            Bus.Address = MAR;
            Bus.Control = IAS_Memory.MW;

            Memory.Do();
        }

        public bool IsDone()
        {
            Operation[] Jumps = {
                IAS_Codes.JUMP_L,
                IAS_Codes.JUMP_M_L,
                IAS_Codes.JUMP_P_L,
                IAS_Codes.JUMP_P_M_L,
                IAS_Codes.JUMP_R,
                IAS_Codes.JUMP_M_R,
                IAS_Codes.JUMP_P_R,
                IAS_Codes.JUMP_P_M_R   
            };

            Word _MBR = MBR;
            Instruction _IBR = IBR;
            Address _PC = PC;
            Address _MAR = MAR;
            Operation _IR = IR;
            bool _RightInstruction = RightInstruction;

            Fetch();
            Decode();

            bool done = false;

            if (Jumps.Contains(IR))
            {
                Execiute();

                if (PC == _PC && RightInstruction == _RightInstruction) done = true;
            }

            MBR = _MBR;
            IBR = _IBR;
            PC = _PC;
            MAR = _MAR;
            IR = _IR;
            RightInstruction = _RightInstruction;

            return done;
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
