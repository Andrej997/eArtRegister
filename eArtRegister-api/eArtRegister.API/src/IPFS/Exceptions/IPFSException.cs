using System;
using System.Net;

namespace IPFS.Exceptions
{
    public class IPFSException : Exception
    {
        public HttpStatusCode _statusCode { get; set; }

        public IPFSException() { }

        public IPFSException(string message) : base(message) { }

        public IPFSException(string message, HttpStatusCode statusCode) : base(message) 
        {
            _statusCode = statusCode;
        }

        public IPFSException(string message, Exception exception) : base(message, exception) { }
    }
}
