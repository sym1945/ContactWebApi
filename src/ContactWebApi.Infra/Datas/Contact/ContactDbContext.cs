﻿using ContactWebApi.App.Common.Interfaces;
using ContactWebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ContactWebApi.Infra.Datas.Contact
#nullable disable
{

    public class ContactDbContext : DbContext, IContactDbContext
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
