using ContactWebApi.Domain.Entities;

namespace ContactWebApi.Infra.Datas.Contact
{
    internal static class ContactDbContextExtensions
    {
        public static async Task<EmployeeGroup> CreateEmployeeGroupAsync(this ContactDbContext context, CancellationToken cancelToken = default)
        {
            var entity = new EmployeeGroup { CreateTime = DateTime.UtcNow };

            await context.EmployeeGroups.AddAsync(entity, cancelToken);
            await context.SaveChangesAsync(cancelToken);

            return entity;
        }

        public static async Task<int> RemoveEmployeeGroupAsync(this ContactDbContext context, EmployeeGroup entity, CancellationToken cancelToken = default)
        {
            context.EmployeeGroups.Remove(entity);
            return await context.SaveChangesAsync(cancelToken);
        }
    }
}
