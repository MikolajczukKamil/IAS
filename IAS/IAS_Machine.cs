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

    /// <summary>
    /// IAS machine model (von Neumann machine)
    /// Simulates the work of the IAS machine
    /// more: 
    /// https://en.wikipedia.org/wiki/IAS_machine,
    /// https://en.wikipedia.org/wiki/Von_Neumann_architecture
    /// </summary>
    public class IAS_Machine : IAS_Helpers
    {
        #region CA

        /// <summary>
        /// Accumulator register, Word (40 bit)
        /// </summary>
        Word AC = 0;

        /// <summary>
        /// multiply-quotient register, Word (40 bit)
        /// </summary>
        Word MQ = 0;

        /// <summary>
        /// memory buffer register, Word (40 bit)
        /// </summary>
        Word MBR = 0;

        #endregion

        #region CC

        /// <summary>
        /// instruction buffer register, Instruction (20 bit)
        /// </summary>
        Instruction IBR = 0;

        /// <summary>
        /// program counter, Address (12 bit)
        /// </summary>
        Address PC = 0;

        /// <summary>
        /// memory address register, Address (12 bit)
        /// </summary>
        Address MAR = 0;

        /// <summary>
        /// insruction register, Operation (8 bit)
        /// </summary>
        Operation IR = 0;

        #endregion

        #region Components

        /// <summary>
        /// Memory of machine
        /// </summary>
        IAS_Memory Memory;

        /// <summary>
        /// Inside Bus
        /// </summary>
        IAS_Bus Bus = new IAS_Bus();

        #endregion

        /// <summary>
        /// Require Left Instruction - eg. not right jump
        /// </summary>
        bool RequireLeftInstruction = true;

        /// <summary>
        /// Is machine in halt state <=> inf loop, jump to this place
        /// </summary>
        public bool HaltState { get; private set; } = false;

        /// <summary>
        /// New IAS machine
        /// </summary>
        /// <param name="code">Array od machine code to run</param>
        /// <param name="copy">Copy memory or use orginal</param>
        public IAS_Machine(Word[] code, bool copy = true)
        {
            Memory = new IAS_Memory(code, Bus, copy);
        }

        /// <summary>
        /// Manuall jump to n line of machine code
        /// </summary>
        /// <param name="address">Address in memmory to jump to</param>
        /// <param name="ToRightInstruction">To right instruction?</param>
        public void ManualJumpTo(Address address, bool ToRightInstruction = false)
        {
            IBR = 0;
            PC = address;
            RequireLeftInstruction = !ToRightInstruction;

            HaltState = false;
        }

        /// <summary>
        /// Read memory, move M[MAR] to MBR
        /// </summary>
        void MemoryRead()
        {
            Bus.Address = MAR;
            Bus.Control = IAS_Memory.MR;

            Memory.Cycle();

            MBR = Bus.Data;
        }

        /// <summary>
        /// Write memory, move MBR to M[MAR]
        /// </summary>
        void WriteMemory()
        {
            Bus.Data = MBR;
            Bus.Address = MAR;
            Bus.Control = IAS_Memory.MW;

            Memory.Cycle();
        }

        /// <summary>
        /// Fetch sub cycle of instruction cycle
        /// </summary>
        void FetchSubCycle()
        {
            if (IBR != 0)
            {
                IR = GetOperationCode(IBR);
                MAR = GetAddress(IBR);

                IBR = 0;
                PC++;
            }
            else
            {
                MAR = PC;

                MemoryRead();

                if (RequireLeftInstruction)
                {
                    IBR = GetRightInstruction(MBR);

                    Instruction leftInstruction = GetLeftInstruction(MBR);

                    IR = GetOperationCode(leftInstruction);
                    MAR = GetAddress(leftInstruction);
                }
                else
                {
                    Instruction rightInstruction = GetRightInstruction(MBR);

                    IR = GetOperationCode(rightInstruction);
                    MAR = GetAddress(rightInstruction);

                    PC++;
                }
            }
        }

        /// <summary>
        /// Execute sub cycle of instruction cycle
        /// </summary>
        void ExecuteSubCycle()
        {
            RequireLeftInstruction = true;

            switch (IR)
            {
                #region transfer
                case IAS_Codes.LOAD_MQ:
                    AC = MQ;
                    return;

                case IAS_Codes.LOAD_MQ_M:
                    MemoryRead();
                    MQ = MBR;
                    return;

                case IAS_Codes.STOR_M:
                    MBR = AC;
                    WriteMemory();
                    return;

                case IAS_Codes.LOAD_M:
                    MemoryRead();
                    AC = MBR;
                    return;

                case IAS_Codes.LOAD_D_M:
                    MemoryRead();
                    AC = -MBR;
                    return;

                case IAS_Codes.LOAD_M_M:
                    MemoryRead();
                    AC = Math.Abs(MBR);
                    return;

                case IAS_Codes.LOAD_D_M_M:
                    MemoryRead();
                    AC = -Math.Abs(MBR);
                    return;

                #endregion transfer

                #region modyfikacja-adresu

                case IAS_Codes.STOR_M_L:
                    {
                        MemoryRead();

                        Instruction left = GetLeftInstruction(MBR);
                        Instruction right = GetRightInstruction(MBR);

                        Address newAddress = (Address)(AC & IAS_Masks.First12Bits);
                        MBR = IAS_Codes.Word(IAS_Codes.Instruction(GetOperationCode(left), newAddress), right);

                        WriteMemory();

                        return;
                    }

                case IAS_Codes.STOR_M_R:
                    {
                        MemoryRead();

                        Instruction left = GetLeftInstruction(MBR);
                        Instruction right = GetRightInstruction(MBR);

                        Address newAddress = (Address)(AC & IAS_Masks.First12Bits);
                        MBR = IAS_Codes.Word(left, IAS_Codes.Instruction(GetOperationCode(right), newAddress));

                        WriteMemory();

                        return;
                    }

                #endregion modyfikacja-adresu

                #region skoki-bezwarunkowe

                case IAS_Codes.JUMP_M_L:
                    MemoryRead();

                    IBR = 0;

                    PC = (Address)MBR;

                    return;

                case IAS_Codes.JUMP_M_R:
                    MemoryRead();

                    IBR = 0;
                    RequireLeftInstruction = false;

                    PC = (Address)MBR;

                    return;

                // ADDED !

                case IAS_Codes.JUMP_L:
                    IBR = 0;

                    PC = MAR;

                    return;

                case IAS_Codes.JUMP_R:
                    IBR = 0;
                    RequireLeftInstruction = false;

                    PC = MAR;

                    return;

                #endregion skoki-bezwarunkowe

                #region skoki-warunkowe

                case IAS_Codes.JUMP_P_M_L:

                    if (AC >= 0)
                    {
                        MemoryRead();

                        IBR = 0;

                        PC = (Address)MBR;
                    }

                    return;

                case IAS_Codes.JUMP_P_M_R:

                    if (AC >= 0)
                    {
                        MemoryRead();

                        IBR = 0;
                        RequireLeftInstruction = false;

                        PC = (Address)MBR;
                    }

                    return;

                // ADDED !

                case IAS_Codes.JUMP_P_L:

                    if (AC >= 0)
                    {
                        IBR = 0;

                        PC = MAR;
                    }

                    return;

                case IAS_Codes.JUMP_P_R:

                    if (AC >= 0)
                    {
                        IBR = 0;
                        RequireLeftInstruction = false;

                        PC = MAR;
                    }

                    return;

                #endregion skoki-warunkowe

                #region arytmetyczne

                case IAS_Codes.ADD_M:
                    MemoryRead();

                    AC = To40BitsValue(AC + MBR);

                    return;

                case IAS_Codes.ADD_M_M:
                    MemoryRead();

                    AC = To40BitsValue(AC + Math.Abs(MBR));

                    return;

                case IAS_Codes.SUB_M:
                    MemoryRead();

                    AC = To40BitsValue(AC - MBR);

                    return;

                case IAS_Codes.SUB_M_M:
                    MemoryRead();

                    AC = To40BitsValue(AC - Math.Abs(MBR));

                    return;

                case IAS_Codes.MUL_M:
                    {
                        MemoryRead();

                        BigInteger mul = new BigInteger(MBR) * MQ;

                        MQ = To40BitsValue((Word)(mul & IAS_Masks.First40Bits));

                        AC = To40BitsValue((Word)(mul >> 40) & IAS_Masks.First40Bits);

                        return;
                    }

                case IAS_Codes.DIV_M:
                    {
                        MemoryRead();

                        if (MBR == 0)
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

        /// <summary>
        /// Instruction cycle of IAS, clock
        /// </summary>
        public void Cycle()
        {
            Instruction _PC = PC;
            Instruction _IBR = IBR;

            FetchSubCycle();
            ExecuteSubCycle();

            if (_PC == PC && _IBR == IBR)
            {
                HaltState = true;
            }
        }

        /// <summary>
        /// Default to string, show all memory
        /// </summary>
        /// <returns>Machine state description</returns>
        public override string ToString()
        {
            return ToString(-1);
        }

        /// <summary>
        /// To string
        /// </summary>
        /// <param name="manyInstructions">Many instructions to show, negative value means all</param>
        /// <returns>Machine state description</returns>
        public string ToString(int manyInstructions)
        {
            StringBuilder description = new StringBuilder();

            description.AppendLine($" IC: {PC}{(IBR != 0 || !RequireLeftInstruction ? 'R' : 'L')}");
            description.AppendLine($" AC: {AC}");
            description.AppendLine($" MQ: {MQ}");
            description.AppendLine($" Memory:");

            description.AppendLine(Memory.ToString((Address)manyInstructions));

            return description.ToString();
        }

    }
}
