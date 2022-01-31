using System;
using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApi.BusinessLogic.Contracts.Exceptions;

namespace WebApi.Engine
{
    public class ErrorFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;

            if (context.Exception is AggregateException aggregateException)
            {
                aggregateException.Flatten();
                exception = aggregateException;
            }

            var statusCode = StatusCodes.Status500InternalServerError;
            var userMessage = "Внутренняя ошибка сервиса";
            var errorCode = "InternalError";

            switch (exception)
            {
                case BadRequestException badRequestException:
                    statusCode = StatusCodes.Status400BadRequest;
                    userMessage = $"Некорректный запрос, код ошибки {badRequestException.ErrorCode}";
                    errorCode = badRequestException.ErrorCode;

                    break;
                case NotFoundException notFoundException:
                    statusCode = StatusCodes.Status404NotFound;
                    userMessage = "Cущность не найдена";
                    errorCode = notFoundException.ErrorCode;

                    break;
            }

            var errorData = new ErrorData(userMessage, errorCode);

            context.Result = new ContentResult
            {
                Content = JsonSerializer.Serialize(errorData),
                ContentType = MediaTypeNames.Application.Json,
                StatusCode = statusCode
            };
        }

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
}