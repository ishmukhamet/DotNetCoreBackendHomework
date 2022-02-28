using System;
using System.Net;

namespace WebApi.BusinessLogic.Contracts.Exceptions
{
    public abstract class AppException : Exception
    {
        public abstract string ErrorCode { get; }

        public abstract HttpStatusCode HttpStatusCode { get; }

        public AppException(string message) : base(message) { }
    }
}
