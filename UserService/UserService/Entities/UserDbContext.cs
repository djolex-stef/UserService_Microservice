using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace UserService.Entities
{
    public class UserDbContext : DbContext
    {
        private readonly IConfiguration configuration;

        public UserDbContext(DbContextOptions<UserDbContext> options, IConfiguration configuration)
            : base(options)
        {
            this.configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("UserDBConnection"));

        }

        public DbSet<PersonalUser> PersonalUser { get; set; }
        public DbSet<CorporationUser> CorporationUser { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Role> Role { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<PersonalUser>()
            .HasIndex(b => b.Username)
            .IsUnique();
            modelBuilder.Entity<CorporationUser>()
            .HasIndex(b => b.Username)
            .IsUnique();
            modelBuilder.Seed();

        }
    }
}
