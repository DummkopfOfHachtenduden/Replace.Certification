using System;
using System.Runtime.Serialization;

namespace Replace.Common.Security
{
    public class SecurityException : Exception, ISerializable
    {
        public SecurityException()
        {
            // Add implementation.
        }

        public SecurityException(string message)
            : base(message)
        {
            // Add implementation.
        }

        public SecurityException(string message, Exception inner)
            : base(message, inner)
        {
            // Add implementation.
        }

        // This constructor is needed for serialization.
        protected SecurityException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // Add implementation.
        }
    }
}