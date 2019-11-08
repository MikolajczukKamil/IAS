using System;
using System.Numerics;

namespace Symulator
{
    public class IAS : IAS_Helpers
    {
        // use only 40 bits
        ulong AC = 0, MQ = 0;
        ushort IC = 0;
        ulong[] memory;

        int licznik = 0;

        public IAS(ulong[] instructions, bool copyInstructions = true)
        {
            memory = copyInstructions ? new ulong[instructions.Length] : memory;

            for (int i = 0; i < instructions.Length; i++)
                memory[i] = instructions[i] & BitsMaskFirst40Bits;
        }

        uint FetchInstructionWord()
        {
            ulong word = memory[IC >> 1];
            bool isLeftInstruction = IC % 2 == 0;
            IC++;

            return (uint) 
                (isLeftInstruction ? word & BitsMaskFirst20Bits : word >> 20);
        }

        ulong FetchData(ushort address) => memory[address];

        void SetDataInMemory(ushort address, ulong data) => memory[address] = data;

        byte DecodeOptCode(uint instruction) => (byte)(instruction & BitsMaskFirst8Bits);

        ushort DecodeAdress(uint instruction) => (ushort)(instruction >> 8);

        void Execiute(uint instruction)
        {
            byte optCode = DecodeOptCode(instruction);
            ushort address = DecodeAdress(instruction);

            ulong oldInstruction;

            switch (optCode)
            {
                #region transfer
                case OptCodes.LOAD_MQ:
                    AC = MQ;
                    return;

                case OptCodes.LOAD_MQ_M:
                    MQ = FetchData(address);
                    return;

                case OptCodes.STOR_M:
                    SetDataInMemory(address, AC);
                    return;

                case OptCodes.LOAD_M:
                    AC = FetchData(address);
                    return;

                case OptCodes.LOAD_DM:
                    AC = IntTo40ZM(-From40ZMToInt(FetchData(address)));
                    return;

                case OptCodes.LOAD_M_M:
                    AC = FetchData(address) & BitsMaskFirst39Bits;
                    return;

                case OptCodes.LOAD_D_M_M:
                    AC = (FetchData(address) & BitsMaskFirst39Bits) | BitsMaskBit40;
                    return;

                #endregion transfer

                #region modyfikacja-adresu

                case OptCodes.STOR_M_L:
                    oldInstruction = memory[address >> 1] & BitsMaskFirst20Bits;

                    OptCodes.Word();//todo

                    //SetDataInMemory(address, );
                    return;

                case OptCodes.STOR_M_R:
                    oldInstruction = memory[address >> 1];


                    //SetDataInMemory(address, );
                    return;

                #endregion modyfikacja-adresu

                #region skoki-bezwarunkowe

                case OptCodes.JUMP_M_L:
                    IC = (ushort) (FetchData(address) << 1);

                    return;

                case OptCodes.JUMP_M_R:
                    IC = (ushort)((FetchData(address) << 1) + 1);

                    return;

                #endregion skoki-bezwarunkowe

                #region skoki-warunkowe

                case OptCodes.JUMP_P_M_L:

                    if (Module(AC) == 0)
                        IC = (ushort)(FetchData(address) << 1);

                    return;

                case OptCodes.JUMP_P_M_R:

                    if (Module(AC) == 0)
                        IC = (ushort)((FetchData(address) << 1) + 1);

                    return;

                #endregion skoki-warunkowe

                #region arytmetyczne

                case OptCodes.ADD_M:
                    AC = Add(AC, FetchData(address));

                    return;

                case OptCodes.SUB_M:
                    AC = Sub(AC, FetchData(address));

                    return;

                case OptCodes.MUL_M:
                    BigInteger mul = new BigInteger(FetchData(address)) * MQ;

                    if(mul <= ulong.MaxValue)
                    {
                        ulong res = (ulong) mul;
                        MQ = res & BitsMaskFirst40Bits;
                        AC = res >> 40;
                    } else {
                        MQ = (ulong) (mul & BitsMaskFirst40Bits);
                        AC = (ulong) (mul >> 40);
                    }

                    return;

                case OptCodes.DIV_M:
                    ulong dataWord = FetchData(address);

                    MQ = AC / dataWord;
                    AC = AC % dataWord;

                    return;

                case OptCodes.LSH:
                    
                    AC = AC << 1;

                    return;

                case OptCodes.RSH:

                    AC = AC >> 1;

                    return;

                #endregion arytmetyczne
            }

            throw new Exception($"Operation not found {Convert.ToString(optCode, 2).PadLeft(8, '0')}");
        }

        public void ManualJumpTo(ushort pos) => IC = (ushort)(pos << 1);

        public void Step()
        {
            licznik++;
            Execiute(FetchInstructionWord());
        }

        public override string ToString() => ToString(memory.Length);

        public string ToString(int manyInstructions)
        {
            string desc = $"\n Krok: {licznik}\n IC: {IC / 2f}\n AC: {From40ZMToInt(AC)} \n MQ: {From40ZMToInt(MQ)}\n Memory:\n";

            for (int i = 0; i < memory.Length && i < manyInstructions; i++)
                desc += memory[i].ToString() + '\n';
            
            return desc;
        }
    }
}
