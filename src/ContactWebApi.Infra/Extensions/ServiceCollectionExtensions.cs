using ContactWebApi.App.Features.Employee.Commands;
using ContactWebApi.Infra.Datas.Contact.Employees;
using Microsoft.Extensions.DependencyInjection;

namespace ContactWebApi.Infra.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEmployeeImporter<T>(this IServiceCollection services)
            where T : IEmployeeImporter
        {
            services.AddScoped<IEmployeeImporter>(provider =>
            {
                T importer = ActivatorUtilities.CreateInstance<T>(provider);

                return new EmployeeImporterWrapper(importer);
            });

            return services;
        }

    }
}
