using LoginService.App.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LoginService.App.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Usuarios { get; set; }
        public DbSet<Schedule> Agendas { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasIndex(i => i.Email)
                .IsUnique();

            builder.Entity<Schedule>()
                .HasOne(i => i.Usuario)
                .WithMany(a => a.Agendas)
                .HasForeignKey(a => a.Email);
        }
    }
}
