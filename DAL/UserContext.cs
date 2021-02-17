using API_App.Models;
using Microsoft.EntityFrameworkCore;

namespace API_App.DAL
{
    public class UserContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>()
                .Property(e => e.Username)
                .IsRequired();

            modelBuilder.Entity<UserModel>()
                .Property(e => e.Password)
                .IsRequired();
            
            modelBuilder.Entity<UserModel>()
                .Property(e => e.Mailaddress);

            modelBuilder.Entity<UserModel>()
                .Property(e => e.CreationDate);
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = Users.db");
        }
    }
}