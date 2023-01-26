using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using officeAPI.Models;

namespace officeAPI.Context
{
    public class MyContexts : DbContext
    {
        public MyContexts(DbContextOptions<MyContexts> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
               .HasOne(a => a.Employee)
               .WithOne(b => b.User)
               .HasForeignKey<Employee>(b => b.NIK);
        }
    }
}
