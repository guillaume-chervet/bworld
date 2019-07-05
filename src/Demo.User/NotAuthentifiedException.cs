using System;
using System.Runtime.Serialization;

namespace Demo.Common
{
    [Serializable]
    public class NotAuthentifiedException : Exception
    {
        public NotAuthentifiedException()
        {
        }

        public NotAuthentifiedException(string message) : base(message)
        {
        }

        public NotAuthentifiedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotAuthentifiedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}