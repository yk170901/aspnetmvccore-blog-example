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

            // cannot use c# interactive for getting guid there
            string adminRoleId = Guid.NewGuid().ToString();
            string superAdminRoleId = Guid.NewGuid().ToString();
            string userRoldId = Guid.NewGuid().ToString();

            // Seed Roles (User, Admin, SuperAdmin)
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
                    Id = userRoldId,
                    ConcurrencyStamp = userRoldId
                }
            };

            // insert to db
            builder.Entity<IdentityRole>().HasData(roles);

            // Seed SuperAdminUser
            string superAdminId = Guid.NewGuid().ToString();
            IdentityUser superAdminUser = new IdentityUser()
            {
                UserName = "superadminname",
                Email = "superadmin@blog.com",
                NormalizedUserName = "superadminname".ToUpper(),
                NormalizedEmail = "superadmin@blog.com".ToUpper(),
                Id = adminRoleId
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
                    RoleId = userRoldId,
                    UserId = superAdminId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);
        }
    }
}
