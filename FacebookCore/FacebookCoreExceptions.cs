using System;

namespace FacebookCore
{
    [Serializable]
    public class FacebookCollectionException : Exception
    {
        public FacebookCollectionException()
        {

        }

        public FacebookCollectionException(string message) : base(message)
        {

        }

        public FacebookCollectionException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
