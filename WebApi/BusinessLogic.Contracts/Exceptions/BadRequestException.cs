using System;

namespace WebApi.BusinessLogic.Contracts.Exceptions
{
    public class BadRequestException : Exception
    {
        public string ErrorCode { get; }

        public BadRequestException(string errorCode)
        {
            ErrorCode = errorCode;
        }
    }
}