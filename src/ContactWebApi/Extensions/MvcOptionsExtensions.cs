using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactWebApi.Extensions
{
    public static class MvcOptionsExtensions
    {
        public static void AddActionFilter<T>(this MvcOptions options, IServiceCollection services, int? order = null)
            where T : class, IFilterMetadata
        {
            if (order.HasValue)
                options.Filters.Add<T>(order.Value);
            else
                options.Filters.Add<T>();

            services.AddScoped<T>();
        }
    }
}
