using ContactWebApi.App.Common.Interfaces;
using ContactWebApi.App.Features.Employee.Commands.Import;
using ContactWebApi.Infra.Datas.Contact;
using ContactWebApi.Infra.Datas.Contact.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ContactWebApi.Infra
{
    public static class RegisterService
    {
        public static IServiceCollection ConfigureInfra(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<ContactDbContext>(options => {
                options.UseSqlServer("Server=.;AttachDbFilename=D:\\Datas\\ContactDb.mdf;Database=ContactDb;Trusted_Connection=Yes;Encrypt=False");
            });

            services.AddTransient<IContactDbContext>(provider =>
                provider.GetRequiredService<ContactDbContext>()
            );
            
            services.AddScoped<IEmployeeImporter, EmployeeImporterDefault>();

            return services;
        }
    }
}
