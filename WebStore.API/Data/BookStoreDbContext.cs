using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebStore.API.Data
{
    public partial class BookStoreDbContext : IdentityDbContext<ApiUser>
    {
        public BookStoreDbContext()
        {
        }

        public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Author> Authors { get; set; } = null!;
        public virtual DbSet<Book> Books { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-DTBPRRG;Database=BookStoreDb;Trusted_Connection=true;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           
            
            modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(e => e.Bio).HasMaxLength(250);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.Property(e => e.Image).HasMaxLength(50);

                entity.Property(e => e.Isbn)
                    .HasMaxLength(50)
                    .HasColumnName("ISBN");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Summary).HasMaxLength(350);

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.AuthorId)
                    .HasConstraintName("FK_Books_ToTable");
            });

            modelBuilder.Entity<IdentityRole>().HasData(

               new IdentityRole
               {
                   Name = "User",
                   NormalizedName = "USER",
                   Id = "0f987b51-4404-4f91-824a-1cb53921832a"
               },
               new IdentityRole
               {
                   Name = "Admin",
                   NormalizedName = "ADMIN",
                   Id = "956fe1e9-131f-414f-9267-2743f23bca6e"
               });
            var hash = new PasswordHasher<ApiUser>();
            modelBuilder.Entity<ApiUser>().HasData(
                new ApiUser
                {
                    Id = "4601852b-7be2-49b4-a983-ec1ee209d80d",
                    Email = "AdminBook@mail.com",
                    NormalizedEmail = "ADMINBOOK@MAIL.COM",
                    UserName = "AdminBook@mail.com",
                    NormalizedUserName = "ADMINMOOK@MAIL.COM",
                    FirstName = "System",
                    LastName = "Admin",
                    PasswordHash = hash.HashPassword(null, "password@1")

                },
                new ApiUser
                {
                    Id = "fdb1806c-75dd-4be1-ad12-eb3a10b34cb0",
                    Email = "UserBook@mail.com",
                    NormalizedEmail = "USERBOOK@MAIL.COM",
                    UserName = "UserBook@mail.com",
                    NormalizedUserName = "USERMOOK@MAIL.COM",
                    FirstName = "System",
                    LastName = "User",
                    PasswordHash = hash.HashPassword(null, "password@1")

                }
                );
             modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                    new IdentityUserRole<string>
                    {
                        UserId = "fdb1806c-75dd-4be1-ad12-eb3a10b34cb0",
                        RoleId = "0f987b51-4404-4f91-824a-1cb53921832a"
                    }
                );

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                    new IdentityUserRole<string>
                    {
                        UserId = "4601852b-7be2-49b4-a983-ec1ee209d80d",
                        RoleId = "956fe1e9-131f-414f-9267-2743f23bca6e"
                    }
                );

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
