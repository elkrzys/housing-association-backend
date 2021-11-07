using System;
using HousingAssociation.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace HousingAssociation.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserCredentials> Credentials { get; set; }
        public virtual DbSet<Building> Buildings { get; set; }
        public virtual DbSet<Local> Locals { get; set; }
        public virtual DbSet<Announcement> Announcements { get; set; }
        public virtual DbSet<Issue> Issues { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSnakeCaseNamingConvention().UseEnumCheckConstraints();
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<UserCredentials>()
            //     .HasKey(c => c.UserId);
            
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserCredentials)
                .WithOne(c => c.User)
                .HasForeignKey<UserCredentials>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Local>()
                .HasMany(l => l.Residents)
                .WithMany(u => u.ResidedLocals)
                .UsingEntity(join => join.ToTable("locals_residents"));

            modelBuilder.Entity<Local>()
                .HasMany(l => l.Owners)
                .WithMany(u => u.OwnedLocals)
                .UsingEntity(join => join.ToTable("locals_owners"));

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion(
                    v => v.ToString(),
                    v => (Role) Enum.Parse(typeof(Role), v));

            modelBuilder.Entity<Building>()
                .Property(b => b.Type)
                .HasConversion(
                    v => v.ToString(),
                    v => (BuildingType)Enum.Parse(typeof(BuildingType), v));
            
            modelBuilder.Entity<Announcement>()
                .Property(a => a.Type)
                .HasConversion(v => v.ToString(),
                    v => (EventType) Enum.Parse(typeof(EventType), v));
            
            modelBuilder.Entity<Issue>()
                .Property(i => i.Type)
                .HasConversion(v => v.ToString(),
                    v => (EventType) Enum.Parse(typeof(EventType), v));

            base.OnModelCreating(modelBuilder);

        }
        
    }
}