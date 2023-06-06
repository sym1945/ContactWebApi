using ContactWebApi.Constants;
using ContactWebApi.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;
using System.Text.Json;

namespace ContactWebApi.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _Next;
        private readonly ProblemDetailsFactory _ProblemDetailsFactory;
        private readonly IHostEnvironment _Environment;
        private readonly ILogger<ExceptionMiddleware> _Logger;


        public ExceptionMiddleware(RequestDelegate next, ProblemDetailsFactory problemDetailsFactory, IHostEnvironment environment, ILogger<ExceptionMiddleware> logger)
        {
            _Next = next;
            _ProblemDetailsFactory = problemDetailsFactory;
            _Environment = environment;
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
                _Logger.LogError(ex, ex.Message);

                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            ProblemDetails? problemDetails = null;

            switch (exception)
            {
                case UnsupportedImportContentTypeException e:
                    {
                        problemDetails = _ProblemDetailsFactory.CreateProblemDetails(
                            httpContext: context,
                            statusCode: (int)HttpStatusCode.UnsupportedMediaType,
                            title: nameof(UnsupportedImportContentTypeException),
                            detail: e.Message
                        );
                        break;
                    }
                case InvalidModelException e:
                    {
                        var modelStateMap = new ModelStateDictionary();

                        foreach (var modelError in e.ModelErrors)
                            modelStateMap.AddModelError(modelError.Name, modelError.Description);

                        problemDetails = _ProblemDetailsFactory.CreateValidationProblemDetails(
                            httpContext: context,
                            modelStateDictionary: modelStateMap,
                            statusCode: (int)HttpStatusCode.BadRequest,
                            title: nameof(InvalidModelException),
                            detail: e.Message
                        );
                        break;
                    }
                case DuplicatedRecordException e:
                    {
                        problemDetails = _ProblemDetailsFactory.CreateProblemDetails(
                            httpContext: context,
                            statusCode: (int)HttpStatusCode.Conflict,
                            title: nameof(DuplicatedRecordException),
                            detail: e.Message
                        );
                        break;
                    }
                default:
                    {
                        problemDetails = _ProblemDetailsFactory.CreateProblemDetails(
                            httpContext: context,
                            statusCode: (int)HttpStatusCode.InternalServerError,
                            title: "Internal Server Error",
                            detail: _Environment.IsDevelopment() ? exception.Message : null
                        );
                        break;
                    }
            }

            context.Response.ContentType = ContentTypes.ApplicationJson;
            context.Response.StatusCode = problemDetails.Status.GetValueOrDefault();

            await context.Response.WriteAsync(problemDetails switch
            {
                ValidationProblemDetails vpd => JsonSerializer.Serialize(vpd),
                _ => JsonSerializer.Serialize(problemDetails)
            });
        }

    }
}
