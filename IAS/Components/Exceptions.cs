using System;

namespace IAS
{
    public class IASMemoryException : Exception
    {
        public int LastAddress;

        public IASMemoryException(string message, int lastAddress) : base(message)
        {
            LastAddress = lastAddress;
        }
    }

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
}
