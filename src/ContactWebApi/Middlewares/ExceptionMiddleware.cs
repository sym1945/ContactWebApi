using ContactWebApi.Constants;
using ContactWebApi.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net;
using System.Text.Json;


namespace ContactWebApi.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _Next;
        private readonly ProblemDetailsFactory _ProblemDetailsFactory;
        private readonly ILoggerFactory _LoggerFactory;

        public ExceptionMiddleware(RequestDelegate next, ProblemDetailsFactory problemDetailsFactory, ILoggerFactory loggerFactory)
        {
            _Next = next;
            _ProblemDetailsFactory = problemDetailsFactory;
            _LoggerFactory = loggerFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _Next(context);
            }
            catch (Exception ex)
            {
                var logger = _LoggerFactory.CreateLogger(nameof(ExceptionMiddleware));

                logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            ProblemDetails? problemDetails = null;

            if (exception is NotSupportedImportDataType)
            {
                problemDetails = _ProblemDetailsFactory.CreateProblemDetails(
                    httpContext: context,
                    statusCode: (int)HttpStatusCode.UnsupportedMediaType,
                    title: "Media type is not supported"
                );
            }
            else
            {
                problemDetails = _ProblemDetailsFactory.CreateProblemDetails(
                    httpContext: context,
                    statusCode: (int)HttpStatusCode.InternalServerError,
                    title: "Internal Server Error from the custom middleware."
                );
            }

            context.Response.ContentType = ContentTypes.ApplicationJson;
            context.Response.StatusCode = problemDetails.Status.GetValueOrDefault();

            await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
        }


    }
}
