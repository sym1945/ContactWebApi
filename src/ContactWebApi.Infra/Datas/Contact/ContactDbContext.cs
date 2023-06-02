using ContactWebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ContactWebApi.Infra.Datas.Contact
{
    public class ContactDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        public DbSet<EmployeeGroup> EmployeeGroups { get; set; }

        public ContactDbContext(DbContextOptions options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }

}
