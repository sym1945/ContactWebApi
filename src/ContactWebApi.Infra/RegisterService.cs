#define USE_INMEMORY

using ContactWebApi.App.Common.Interfaces;
using ContactWebApi.App.Features.Employee.Commands;
using ContactWebApi.Infra.Datas.Contact;
using ContactWebApi.Infra.Datas.Contact.Employees;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ContactWebApi.Infra
{
    public static class RegisterService
    {
        public static IServiceCollection ConfigureInfra(this IServiceCollection services, IConfiguration config)
        {
#if USE_INMEMORY
            var connectionString = "DataSource=myshareddb;mode=memory;cache=shared";
            var keepAliveConnection = new SqliteConnection(connectionString);
            keepAliveConnection.Open();

            services.AddDbContext<ContactDbContext>(options =>
            {
                options.UseSqlite(connectionString);
            });

            services.AddSingleton(keepAliveConnection);
            services.AddScoped<IEmployeeImporter, EmployeeImporterDefault>();

#else
            services.AddDbContext<ContactDbContext>(options =>
            {
                options.UseSqlServer("Server=.;AttachDbFilename=D:\\Datas\\ContactDb.mdf;Database=ContactDb;Trusted_Connection=Yes;Encrypt=False");
            });

            services.AddScoped<IEmployeeImporter, EmployeeImporterMssql>();
#endif

            services.AddTransient<IContactDbContext>(provider =>
                provider.GetRequiredService<ContactDbContext>()
            );


            return services;
        }
    }
}
