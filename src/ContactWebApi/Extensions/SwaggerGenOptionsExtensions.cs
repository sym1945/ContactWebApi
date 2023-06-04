using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

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
    }
}
