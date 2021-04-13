using System;

namespace Warehouse.Exceptions
{


    /// <summary>
    /// Exception that is used by Container.
    /// </summary>
    [Serializable]
    class ContainerOverweightException : Exception
    {
        /// <summary>
        /// Constructor to show message on cathching the exception, based on the Exception's one.
        /// </summary>
        /// <param name="message"></param>
        public ContainerOverweightException(string message) : base(message)
        { }
    }
}
