using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Amazon.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                var status = 500;
                if (ex is ArgumentException) status = 400;
                else if (ex is InvalidOperationException) status = 409;
                else if (ex is KeyNotFoundException) status = 404;
                else
                {
                    var msg = ex.Message.ToLowerInvariant();
                    if (msg.Contains("not found")) status = 404;
                    else if (msg.Contains("cannot be deleted") || msg.Contains("not enough stock")) status = 409;
                }

                var problem = new ProblemDetails
                {
                    Title = "An error occurred.",
                    Detail = ex.Message,
                    Status = status
                };
                context.Response.StatusCode = status;
                context.Response.ContentType = "application/json";
                var json = JsonSerializer.Serialize(problem);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
