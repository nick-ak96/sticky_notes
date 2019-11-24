using System;
using api.Models.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace api.Filters
{
    public class GlobalExceptionHandler : IExceptionFilter
    {
        private readonly ILogger _logger;

        public GlobalExceptionHandler(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GlobalExceptionHandler>();
        }

        public void OnException(ExceptionContext context)
        {
            var ex = context.Exception;
            context.Result = HandleException(ex);
        }

        private IActionResult HandleException(Exception ex)
        {
            if (ex is NotFoundException)
            {
                return new NotFoundObjectResult(
                    new ProblemDetails()
                    {
                        Title = "Not found",
                        Status = 404,
                        Detail = ex.Message
                    });
            }
            else if (ex is ConflictException)
            {
                return new ConflictObjectResult(
                    new ProblemDetails()
                    {
                        Title = "Conflict",
                        Status = 409,
                        Detail = ex.Message
                    });
            }
            else
            {
                return new ContentResult()
                {
                    StatusCode = 500,
                    ContentType = "application/json",
                    Content = JsonConvert.SerializeObject(new ProblemDetails()
                    {
                        Title = "Unknown error",
                        Status = 500,
                        Detail = ex.Message + "\n" + ex.StackTrace //"Unknown internal server error"
                    })
                };
            }
        }
    }

}
