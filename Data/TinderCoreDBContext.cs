using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tinder_Admin.Entities;

namespace Tinder_Admin.Data
{
    public class TinderCoreDBContext : IdentityDbContext<AppUser>
    {
        public TinderCoreDBContext(DbContextOptions<TinderCoreDBContext> options) : base(options) { }

        public DbSet<AppUser> Users { get; set; }
    }
}
