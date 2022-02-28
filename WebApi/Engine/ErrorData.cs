using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Engine
{
    public class ErrorData
    {
        public string UserMessage { get; }

        public string ErrorCode { get; }

        public ErrorData(string userMessage, string errorCode)
        {
            UserMessage = userMessage;
            ErrorCode = errorCode;
        }
    }
}
