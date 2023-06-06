using ContactWebApi.App.Common.Extensions;
using ContactWebApi.App.Common.Interfaces;
using ContactWebApi.App.Common.Options;
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
            //services.AddOptions<ContactDbOption>(ContactDbOption.Key).Bind(config);

            var contactDbOption = config.GetSection(ContactDbOption.Key).Get<ContactDbOption>();

            if (contactDbOption == null || StringExtensions.IsNullOrEmptyOrWhiteSpace(contactDbOption.ConnectionString))
            {
                // Use SQLite memory
                var connectionString = "DataSource=ContactDb;mode=memory;cache=shared";
                var keepAliveConnection = new SqliteConnection(connectionString);
                keepAliveConnection.Open();

                services.AddSingleton(keepAliveConnection);

                services.AddDbContext<ContactDbContext>(options =>
                {
                    options.UseSqlite(connectionString);
                });

                services.AddEmployeeImporter<EmployeeImporterDefault>();
            }
            else
            {
                // Use MS-SQL
                services.AddDbContext<ContactDbContext>(options =>
                {
                    options.UseSqlServer(contactDbOption.ConnectionString);
                });

                if (contactDbOption.UseBulkInsert)
                    services.AddEmployeeImporter<EmployeeImporterMssql>();
                else
                    services.AddEmployeeImporter<EmployeeImporterDefault>();
            }

            services.AddTransient<IContactDbContext>(provider =>
                provider.GetRequiredService<ContactDbContext>()
            );

            return services;
        }
    }
}
