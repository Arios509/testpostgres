using Domain.Aggregate.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace Api.Infrastructure.Repositories
{
    public class DataContext : DbContext
    {
        public DbSet<User> User { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                modelBuilder.Entity(entity.Name).HasNoKey()
                    .ToTable(entity.ClrType.Name.ToLower());

                foreach (var property in entity.GetProperties())
                {
                    modelBuilder.Entity(entity.Name).Property(property.Name).HasColumnName(property.Name.ToLower());
                }
            }

            //modelBuilder.Entity<User>()
            //.Property(e => e.Detail)
            //.HasConversion(
            //    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
            //    v => JsonSerializer.Deserialize<List<Detail>>(v, (JsonSerializerOptions)null));
            //}
        }
    }
}
