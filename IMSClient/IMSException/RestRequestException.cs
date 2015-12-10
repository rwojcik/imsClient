using System;

namespace IMSClient.IMSException
{
    public class RestRequestException : Exception
    {
        public RestRequestException() { }

        public RestRequestException(string msg) : base(msg) { }

        public RestRequestException(string msg, Exception inner) : base(msg, inner) { }
    }
}
