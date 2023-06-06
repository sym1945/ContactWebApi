using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace ContactWebApi.Extensions
{
    public static class SwaggerGenOptionsExtensions
    {
        public static void UseDateOnlyStringConverter(this SwaggerGenOptions options)
        {
            options.MapType<DateOnly>(() => new OpenApiSchema
            {
                Type = "string",
                Format = "date"
            });
        }

        public static void IncludeXmlComments(this SwaggerGenOptions options, bool includeControllerXmlComments = false)
        {
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

            options.IncludeXmlComments(
                filePath: Path.Combine(AppContext.BaseDirectory, xmlFilename)
                , includeControllerXmlComments
            );
        }
    }
}
