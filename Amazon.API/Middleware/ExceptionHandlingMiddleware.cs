using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Amazon.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            this.next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred during the request.");

                var status = 500;
                var title = "An error occurred.";

                if (ex is ArgumentException) status = 400;
                else if (ex is InvalidOperationException) status = 409;
                else if (ex is KeyNotFoundException) status = 404;
                else if (ex is DbUpdateConcurrencyException)
                {
                    status = 409;
                    title = "Concurrency error.";
                }
                else
                {
                    var msg = ex.Message.ToLowerInvariant();
                    if (msg.Contains("not found")) status = 404;
                    else if (msg.Contains("cannot be deleted") || msg.Contains("not enough stock")) status = 409;
                }

                var problem = new ProblemDetails
                {
                    Title = title,
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
