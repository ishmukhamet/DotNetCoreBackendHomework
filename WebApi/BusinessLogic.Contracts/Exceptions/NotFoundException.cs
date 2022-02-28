using System;
using System.Net;

namespace WebApi.BusinessLogic.Contracts.Exceptions
{
    public class NotFoundException : AppException
    {
        public override string ErrorCode => "NotFound";

        public override HttpStatusCode HttpStatusCode => HttpStatusCode.NotFound;

        public NotFoundException(string message) : base(message) { }
    }
}