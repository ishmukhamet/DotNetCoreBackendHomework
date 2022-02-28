using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebApi.Engine
{
    /// <summary>
    /// Часть конвейера, обрабатывающая все эксепшны, включается только на продакшне
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var resp = new ErrorData("Внутренняя ошибка", "InternalError");

                await context.Response.WriteAsJsonAsync(resp);
            }
        }
    }
}
