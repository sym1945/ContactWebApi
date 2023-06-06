using ContactWebApi.Middlewares;

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
