using System;
using Microsoft.EntityFrameworkCore;
using ContactsManager.Domain.Models;

namespace ContactsManager.Persistence.ContextDb
{
    public class ContactsDbContext : DbContext
    {
        public DbSet<Contact> Contacts { get; set; }

        public ContactsDbContext(DbContextOptions<ContactsDbContext> options)
        : base(options)
        {}

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ContacstsDb;Trusted_Connection=true;MultipleActiveResultSets=true;")
            .EnableSensitiveDataLogging(true);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
