using ContactWebApi.Middlewares;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;

namespace ContactWebApi.Extensions
{
    public static class ApplicationBuilderExteions
    {
        public static void UseExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
