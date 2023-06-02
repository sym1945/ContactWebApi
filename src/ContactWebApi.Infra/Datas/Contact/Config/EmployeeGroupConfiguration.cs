using ContactWebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContactWebApi.Infra.Datas.Contact.Config
{
    internal class EmployeeGroupConfiguration : IEntityTypeConfiguration<EmployeeGroup>
    {
        public void Configure(EntityTypeBuilder<EmployeeGroup> builder)
        {
            builder.HasKey(entity => entity.Id);
        }
    }

   
}
