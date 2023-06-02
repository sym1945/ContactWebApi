using ContactWebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContactWebApi.Infra.Datas.Contact.Config
{
    internal class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.Name)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(entity => entity.Email)
                .HasColumnType("varchar")
                .HasMaxLength(320)
                .IsRequired();

            builder.HasIndex(entity => entity.Email)
                .IsUnique();

            builder.Property(entity => entity.Tel)
                .HasColumnType("varchar")
                .HasMaxLength(15)
                .IsRequired();

            builder.HasIndex(entity => entity.Tel)
                .IsUnique();

            builder.Property(entity => entity.Joined)
                .IsRequired();

            builder.HasOne(entity => entity.Group)
                .WithMany(group => group.Employees)
                .HasForeignKey(entity => entity.GroupId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
