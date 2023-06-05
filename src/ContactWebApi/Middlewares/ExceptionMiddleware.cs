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
        private readonly ILogger<ExceptionMiddleware> _Logger;

        public ExceptionMiddleware(RequestDelegate next, ProblemDetailsFactory problemDetailsFactory, ILogger<ExceptionMiddleware> logger)
        {
            _Next = next;
            _ProblemDetailsFactory = problemDetailsFactory;
            _Logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _Next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            ProblemDetails? problemDetails = null;

            switch (exception)
            {
                case NotSupportedImportDataType e:
                    {
                        problemDetails = _ProblemDetailsFactory.CreateProblemDetails(
                            httpContext: context,
                            statusCode: (int)HttpStatusCode.UnsupportedMediaType,
                            title: nameof(NotSupportedImportDataType)
                        );
                        break;
                    }
                case RequestModelInvalidException e:
                    {
                        problemDetails = _ProblemDetailsFactory.CreateProblemDetails(
                            httpContext: context,
                            statusCode: (int)HttpStatusCode.BadRequest,
                            title: nameof(RequestModelInvalidException)
                        );
                        break;
                    }
                case DuplicatedRecordException e:
                    {
                        problemDetails = _ProblemDetailsFactory.CreateProblemDetails(
                            httpContext: context,
                            statusCode: (int)HttpStatusCode.Conflict,
                            title: nameof(DuplicatedRecordException)
                        );
                        break;
                    }
                default:
                    {
                        problemDetails = _ProblemDetailsFactory.CreateProblemDetails(
                            httpContext: context,
                            statusCode: (int)HttpStatusCode.InternalServerError,
                            title: "Internal Server Error"
                        );
                        break;
                    }
            }

            context.Response.ContentType = ContentTypes.ApplicationJson;
            context.Response.StatusCode = problemDetails.Status.GetValueOrDefault();

            await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
        }


    }
}
