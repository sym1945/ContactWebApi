using ContactWebApi.Infra.Datas.Contact;
using Microsoft.Extensions.DependencyInjection;

namespace ContactWebApi.Infra
{
    public static class Initializer
    {
        public static void InitInfra(this IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ContactDbContext>();
            context.Database.EnsureCreated();
        }
    }
}
