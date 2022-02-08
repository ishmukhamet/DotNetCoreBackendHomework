using System;

namespace WebApi.BusinessLogic.Contracts.Exceptions
{
    public class NotFoundException : Exception
    {
        public string ErrorCode { get; }

        public NotFoundException(string errorCode)
        {
            ErrorCode = errorCode;
        }
    }
}
