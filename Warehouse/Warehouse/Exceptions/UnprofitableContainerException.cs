using System;

namespace Warehouse.Exceptions
{


    /// <summary>
    /// Exception that is used by Warehouse.
    /// </summary>
    [Serializable]
    class UnprofitableContainerException : Exception
    {

        /// <summary>
        /// Constructor to show message on cathching the exception, based on the Exception's one.
        /// </summary>
        /// <param name="message"></param>
        public UnprofitableContainerException(string message) : base(message)
        { }
    }
}
