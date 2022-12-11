using Microsoft.EntityFrameworkCore;
using Stemauto.Entities;

namespace Stemauto.Repository
{
    public class MyDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Order> Orders { get; set; }


        public object HttpContext { get; internal set; }

        public MyDbContext()
        {
            this.Users = this.Set<User>();
            this.Cars = this.Set<Car>();
            this.Services = this.Set<Service>();
            this.Orders = this.Set<Order>();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server = localhost; Database = StemautoDB; Trusted_Connection = True;")
                          .UseLazyLoadingProxies();

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    Id = 1,
                    Username = "stefibrat",
                    Password = "stefipass",
                    FirstName = "Stefani",
                    LastName = "Uzunova",
                    Email = "chiche021223@gmail.com",
                    Role = "admin"
                });
            ;
        }
    }
}
