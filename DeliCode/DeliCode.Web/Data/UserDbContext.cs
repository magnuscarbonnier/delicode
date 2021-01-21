using DeliCode.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeliCode.Web.Data
{
    public class UserDbContext : IdentityDbContext<ApplicationUser>
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
            base.Database.Migrate();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            const string ROLE_ID = "1";
            string ADMIN_ID = Guid.NewGuid().ToString();


            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = ROLE_ID,
                Name = "admin",
                NormalizedName = "admin"
            });

            var hasher = new PasswordHasher<ApplicationUser>();
            modelBuilder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                Id = ADMIN_ID,
                UserName = "admin@delicode.se",
                NormalizedUserName = "admin@delicode.se",
                Email = "admin@delicode.se",
                NormalizedEmail = "admin@delicode.se",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Admin123#"),
                SecurityStamp = string.Empty
            });

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = ROLE_ID,
                UserId = ADMIN_ID
            });

            base.OnModelCreating(modelBuilder);
            modelBuilder.Ignore<IdentityUserLogin<string>>();
        }
    }
}