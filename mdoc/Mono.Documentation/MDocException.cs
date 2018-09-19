using System;
using System.Runtime.Serialization;

namespace Mono.Documentation
{
    [Serializable]
    internal class MDocException : Exception
    {
        public MDocException ()
        {
        }

        public MDocException (string message) : base (message)
        {
        }

        public MDocException (string message, Exception innerException) : base (message, innerException)
        {
        }

        protected MDocException (SerializationInfo info, StreamingContext context) : base (info, context)
        {
        }
    }
}