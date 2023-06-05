#define USE_INMEMORY

using ContactWebApi.App.Common.Interfaces;
using ContactWebApi.Infra.Datas.Contact;
using ContactWebApi.Infra.Datas.Contact.Employees;
using ContactWebApi.Infra.Extensions;
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
            var connectionString = "DataSource=ContactDb;mode=memory;cache=shared";
            var keepAliveConnection = new SqliteConnection(connectionString);
            keepAliveConnection.Open();

            services.AddSingleton(keepAliveConnection);

            services.AddDbContext<ContactDbContext>(options =>
            {
                options.UseSqlite(connectionString);
            });

            services.AddEmployeeImporter<EmployeeImporterDefault>();

#else
            services.AddDbContext<ContactDbContext>(options =>
            {
                options.UseSqlServer("Server=.;AttachDbFilename=D:\\Datas\\ContactDb.mdf;Database=ContactDb;Trusted_Connection=Yes;Encrypt=False");
            });

            services.AddEmployeeImporter<EmployeeImporterMssql>();
#endif

            services.AddTransient<IContactDbContext>(provider =>
                provider.GetRequiredService<ContactDbContext>()
            );


            return services;
        }
    }
}
