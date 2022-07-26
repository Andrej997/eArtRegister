using System;
using System.Net;

namespace KeyCloak.Exceptions
{
    public class KeyCloakUserException : Exception
    {
        public HttpStatusCode _statusCode { get; set; }

        public KeyCloakUserException() { }

        public KeyCloakUserException(string message) : base(message) { }

        public KeyCloakUserException(string message, HttpStatusCode statusCode) : base(message) 
        {
            _statusCode = statusCode;
        }

        public KeyCloakUserException(string message, Exception exception) : base(message, exception) { }
    }
}
