using ContactWebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactWebApi.App.Common.Interfaces
{
    public interface IContactDbContext
    {
        DbSet<Employee> Employees { get; set; }
        DbSet<EmployeeGroup> EmployeeGroups { get; set; }
    }
}
