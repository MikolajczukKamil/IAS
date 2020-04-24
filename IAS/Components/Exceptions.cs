using System;

namespace IAS
{
    using AddressT = UInt16;
    using Operation = Byte;

    /// <summary>
    /// Exception in IAS machine
    /// </summary>
    public abstract class IASException : Exception {
        /// <summary>
        /// New IASException
        /// </summary>
        /// <param name="message">Error message</param>
        public IASException(string message) : base(message) { }
    }

    /// <summary>
    /// Exception in IAS memory
    /// </summary>
    public class IASMemoryException : IASException
    {
        /// <summary>
        /// Address with error
        /// </summary>
        public AddressT Address;

        /// <summary>
        /// New IASMemoryException
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="address">Address with error</param>
        public IASMemoryException(string message, AddressT address) : base(message)
        {
            Address = address;
        }
    }

    /// <summary>
    /// Exception in IAS main machine
    /// </summary>
    public abstract class IASMachineException : IASException
    {
        /// <summary>
        /// new IASMachineException
        /// </summary>
        /// <param name="message">Error message</param>
        public IASMachineException(string message) : base(message) { }
    }

    /// <summary>
    /// Exception in execiution time
    /// </summary>
    public class IASExeciuteException : IASMachineException
    {
        /// <summary>
        /// Operation code with exception
        /// </summary>
        public Operation OperationCode;

        /// <summary>
        /// New IASExeciuteException
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="operationCode">Operation code</param>
        public IASExeciuteException(string message, Operation operationCode) : base(message)
        {
            OperationCode = operationCode;
        }
    }
}
