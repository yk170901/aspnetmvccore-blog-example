using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Project.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed Roles (User, Admin, SuperAdmin)
            string adminRoleId = "7c21344a-c999-4f4e-8735-20b2a722837b";
            string superAdminRoleId = "ea25e605-1400-47fe-9ebd-c81bcffce3e0";
            string userRoleId = "0ee3ea4a-9808-4954-b3d5-48c32a787e04";

            List<IdentityRole> roles = new List<IdentityRole>()
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "Admin",
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId
                },
                new IdentityRole
                {
                    Name = "SuperAdmin",
                    NormalizedName = "SuperAdmin",
                    Id = superAdminRoleId,
                    ConcurrencyStamp = superAdminRoleId
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "User",
                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId
                }
            };

            // insert to db
            builder.Entity<IdentityRole>().HasData(roles);

            // Seed SuperAdminUser
            string superAdminId = "d7fda04d-96c5-40c7-9215-73edec6faee9";
            IdentityUser superAdminUser = new IdentityUser()
            {
                UserName = "superadminname",
                Email = "superadmin@blog.com",
                NormalizedUserName = "superadminname".ToUpper(),
                NormalizedEmail = "superadmin@blog.com".ToUpper(),
                Id = superAdminId
            };

            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>()
                .HashPassword(superAdminUser, "sa1234");

            builder.Entity<IdentityUser>().HasData(superAdminUser);

            // Add all the roles to SuperAdminUser
            List<IdentityUserRole<string>> superAdminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = superAdminId
                },

                new IdentityUserRole<string>
                {
                    RoleId = superAdminRoleId,
                    UserId = superAdminId
                },

                new IdentityUserRole<string>
                {
                    RoleId = userRoleId,
                    UserId = superAdminId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);
        }
    }
}
