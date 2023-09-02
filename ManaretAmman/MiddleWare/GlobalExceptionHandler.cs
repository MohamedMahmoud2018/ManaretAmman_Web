using ManaretAmman.ExceptionTypes;
using ManaretAmman.Models;
using System.Net;
using System.Text.Json;

namespace ManaretAmman.MiddleWare
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {

            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
                Console.WriteLine($"Error Message::::{ex.Message}:::::{ex.InnerException}");
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            var message = "An unexpected error occurred.";
            //var stackTrace = string.Empty;

            if (exception is ApiException apiException)
            {
                statusCode = apiException.StatusCode;
                message = $"{apiException.Message}::::::{apiException.InnerException}";
                //stackTrace = apiException.StackTrace;
            }

            IApiResponse response = ApiResponse.Failure(message, new[] { exception.Message });

            string responseAsString = JsonSerializer.Serialize(response);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(responseAsString);
        }

    }


}
