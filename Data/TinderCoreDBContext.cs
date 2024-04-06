using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tinder_Admin.Entities;
using Tinder_Admin.Helpers.Constants;

namespace Tinder_Admin.Data
{
    public class TinderCoreDBContext : IdentityDbContext<AppUser>
    {
        public TinderCoreDBContext(DbContextOptions<TinderCoreDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = RoleConstants.ADMIN, NormalizedName = RoleConstants.ADMIN },
                new IdentityRole { Id = "2", Name = RoleConstants.ENDUSER, NormalizedName = RoleConstants.ENDUSER }
            );
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<IdentityRole> Roles { get; set; }

    }
}
