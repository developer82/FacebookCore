using Newtonsoft.Json.Linq;
using System;
using System.Runtime.Serialization;

namespace FacebookCore
{
    [Serializable]
    internal class FacebookApiException : Exception
    {
        public FacebookApiException()
        {
        }

        public FacebookApiException(string message) : base(message)
        {
        }

        public FacebookApiException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FacebookApiException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}