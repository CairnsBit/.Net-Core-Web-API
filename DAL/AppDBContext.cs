using API_App.Models;
using Microsoft.EntityFrameworkCore;

namespace API_App.DAL
{
    public class AppDbContext : DbContext
    {
        public DbSet<TestModel> Tables { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestModel>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<TestModel>()
                .Property(e => e.Temperature)
                .IsRequired();

            modelBuilder.Entity<TestModel>()
                .Property(e => e.Summary);

            modelBuilder.Entity<TestModel>()
                .Property(e => e.Date);
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = Info.db");
        }
    }
}