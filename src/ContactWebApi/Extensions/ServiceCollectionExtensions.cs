using ContactWebApi.App.Common.Converters;
using System.ComponentModel;

namespace ContactWebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDateOnlyStringConverter(this IServiceCollection services)
        {
            TypeDescriptor.AddAttributes(typeof(DateOnly), new TypeConverterAttribute(typeof(DateOnlyTypeConverter)));

            services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
            });

            services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
            {
                options.SerializerOptions.Converters.Add(new DateOnlyJsonConverter());
            });

            return services;
        }
    }
}
