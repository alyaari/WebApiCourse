using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebApplication4.Models;

namespace WebApplication4.Data
{

    
    public class ApplicationContext : IdentityDbContext<User, Role, int>
    {
        public ApplicationContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Employee> Employee { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Role>().HasData(
               new Role(1,"Visitor"),
               new Role(2,"Admin")
               );
        }
    }
}
