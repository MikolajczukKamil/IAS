using System;
using IAS;
using IAS.Components;

namespace Symulator_IAS.Examples
{
    using Word = Int64;
    using Address = UInt16;

    /// <summary>
    /// Example IAS program to run on simple OS
    /// </summary>
    class ProgramOn_IAS : IAS_Helpers
    {
        /// <summary>
        /// Name of program
        /// </summary>
        public string Name;

        /// <summary>
        /// Number of input wariables
        /// </summary>
        public Address Wariables;

        /// <summary>
        /// Amount memory words to show 
        /// </summary>
        public Address MemoryToShow;

        /// <summary>
        /// Start position
        /// </summary>
        Address StartPosiotion;

        /// <summary>
        /// Machine code of program
        /// </summary>
        Word[] Code;

        /// <summary>
        /// Machine to run program
        /// </summary>
        public IAS_Machine Machine;

        /// <summary>
        /// New ProgramOn_IAS
        /// </summary>
        /// <param name="name">Program name</param>
        /// <param name="code">Machine code</param>
        /// <param name="wariables">Number of input wariables</param>
        /// <param name="startPosiotion">Start position</param>
        /// <param name="memoryToShow">Amount memory words to show, default startPosiotion</param>
        public ProgramOn_IAS(string name, Word[] code, Address wariables, Address startPosiotion = 0, Address memoryToShow = 0)
        {
            if (code == null || code.Length == 0) throw new Exception("Code not found");
            if (startPosiotion >= code.Length) throw new Exception("Start Position not found in Code");

            if (memoryToShow == 0) memoryToShow = startPosiotion;

            Name = name;
            Code = code;
            Wariables = wariables;
            MemoryToShow = memoryToShow;
            StartPosiotion = startPosiotion;
        }

        /// <summary>
        /// Reset program
        /// </summary>
        /// <param name="code">Machine code</param>
        public void Reset(Word[] code)
        {
            for (int i = 0; i < code.Length; i++)
                Code[i] = To40BitsValue(code[i]);

            Machine = new IAS_Machine(Code);
            Machine.ManualJumpTo(StartPosiotion);
        }
    }
}
