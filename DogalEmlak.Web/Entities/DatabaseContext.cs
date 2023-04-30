using Microsoft.EntityFrameworkCore;
using NETCore.Encrypt.Extensions;

namespace DogalEmlak.Web.Entities
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Property> Properties { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<PropertyImage> PropertyImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>()
                .HasKey(r => new { r.UserId, r.Authority });

			modelBuilder.Entity<PropertyImage>()
	            .HasKey(r => new { r.PropertyId, r.FileName });

            //Başlangıçta boş bir ver itabanı olmasın, Admin kullanıcısı lazım.
			User user = new User("Gökhan", "Güneşoğlu", "gokhan123", "123qweqwe".MD5());
            user.Id = Guid.NewGuid();
            modelBuilder.Entity<User>().HasData(user);
            Role role = new Role(user.Id, "Admin");       
            modelBuilder.Entity<Role>().HasData(role);
			role = new Role(user.Id, "Manager");
			modelBuilder.Entity<Role>().HasData(role);
            role = new Role(user.Id, "Staff");
            modelBuilder.Entity<Role>().HasData(role);
            user = new User("Serkan", "Güneşoğlu", "serkan123", "123qweqwe".MD5());
			user.Id = Guid.NewGuid();
			modelBuilder.Entity<User>().HasData(user);
            role = new Role(user.Id, "Staff");
            modelBuilder.Entity<Role>().HasData(role);

            base.OnModelCreating(modelBuilder);
        }
    }
}
