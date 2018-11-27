using CleanVidly.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanVidly.Persistance.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Email).HasMaxLength(128).IsRequired();
            builder.Property(u => u.Name).HasMaxLength(32).IsRequired();
            builder.Property(u => u.Lastname).HasMaxLength(32).IsRequired();
            builder.Property(u => u.Salt).HasMaxLength(128).IsRequired();
            builder.Property(u => u.Password).HasMaxLength(64).IsRequired();
            builder.Property(u => u.JoinDate).HasDefaultValueSql("GETDATE()").IsRequired();
        }
    }
}