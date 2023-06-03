using ContactWebApi.App.Features.Employee.Mappers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ContactWebApi.App
{
    public static class RegisterService
    {
        public static IServiceCollection ConfigureApp(this IServiceCollection services, IConfiguration config)
        {
            //services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(a => a.AddProfile(new EmployeeDtoMapper()));

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            return services;
        }
    }
}
