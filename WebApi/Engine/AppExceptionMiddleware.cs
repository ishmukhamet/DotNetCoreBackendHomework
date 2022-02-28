using Microsoft.AspNetCore.Http;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using WebApi.BusinessLogic.Contracts.Exceptions;

namespace WebApi.Engine
{
    public class AppExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public AppExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AppException e)
            {
                context.Response.StatusCode = (int)e.HttpStatusCode;
                context.Response.ContentType = MediaTypeNames.Application.Json;

                var data = new ErrorData(e.Message, e.ErrorCode);

                await context.Response.WriteAsync(JsonSerializer.Serialize(data));
            }
        }
    }
}
