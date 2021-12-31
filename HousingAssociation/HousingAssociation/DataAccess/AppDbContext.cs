using System;
using HousingAssociation.DataAccess.Entities;
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
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<Building> Buildings { get; set; }
        public virtual DbSet<Local> Locals { get; set; }
        public virtual DbSet<Announcement> Announcements { get; set; }
        public virtual DbSet<Issue> Issues { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        //public virtual DbSet<AnnouncementBuilding> AnnouncementBuildings { get; set; }

        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //     => optionsBuilder.UseSnakeCaseNamingConvention().UseEnumCheckConstraints();
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.Role)
                    .HasConversion(
                        v => v.ToString(),
                        v => (Role) Enum.Parse(typeof(Role), v));

                entity.HasOne(u => u.UserCredentials)
                    .WithOne(c => c.User)
                    .HasForeignKey<UserCredentials>(c => c.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(u => u.ReceivedDocuments)
                    .WithMany(d => d.Receivers)
                    .UsingEntity(join => join.ToTable("users_documents"));

                // entity.HasOne(u => u.Document)
                //     .WithOne(d => d.Author)
                //     .HasForeignKey<Document>(d => d.AuthorId)
                //     .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Document>(entity =>
            {
                entity.HasOne(d => d.Author)
                    .WithMany(u => u.CreatedDocuments)
                    .HasForeignKey(d => d.AuthorId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Issue>(entity =>
            {
                entity.HasOne(i => i.Local)
                    .WithMany(l => l.Issues)
                    .HasForeignKey(i => i.SourceLocalId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Local>(entity =>
            {
                entity.HasMany(l => l.Residents)
                    .WithMany(u => u.ResidedLocals)
                    .UsingEntity(join => join.ToTable("locals_residents"));

                entity.HasMany(l => l.Owners)
                    .WithMany(u => u.OwnedLocals)
                    .UsingEntity(join => join.ToTable("locals_owners"));
            });
            
            modelBuilder.Entity<Building>()
                .Property(b => b.Type)
                .HasConversion(
                    v => v.ToString(),
                    v => (BuildingType)Enum.Parse(typeof(BuildingType), v));

            modelBuilder.Entity<Announcement>(entity =>
            {
                entity.Property(a => a.Type)
                    .HasConversion(v => v.ToString(),
                        v => (AnnouncementType) Enum.Parse(typeof(AnnouncementType), v));

                entity.HasMany(a => a.TargetBuildings)
                    .WithMany(b => b.Announcements)
                    .UsingEntity(join => join.ToTable("announcements_buildings"));

            });

            base.OnModelCreating(modelBuilder);
        }
        
    }
}