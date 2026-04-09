using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RentaCarAPI.Models;

namespace RentaCarAPI.Data
{
    public class RDataContext : IdentityDbContext<Client,IdentityRole<Guid>,Guid>
    {
        public RDataContext(DbContextOptions<RDataContext> options) : base(options)
        {
                
        }

        public DbSet<Client> clients { get; set; }
        public DbSet<Vehicle> vehicles { get; set; }
        public DbSet<Depot> depots { get; set; }
        public DbSet<Location> locations { get; set; }
        public DbSet<rental> rentals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<rental>()
                    .HasKey(Pk => new { Pk.ClientId, Pk.VehId,Pk.DateOfRental });
            modelBuilder.Entity<rental>()
                    .HasOne(p => p.Client)
                    .WithMany(pc => pc.rentals)
                    .HasForeignKey(c => c.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<rental>()
                    .HasOne(p => p.Vehicle)
                    .WithMany(pc => pc.rentals)
                    .HasForeignKey(c => c.VehId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.Depot)
                .WithMany(d => d.vehicles)
                .HasForeignKey(v => v.DepId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<Depot>()
                .HasOne(d => d.Location)
                .WithMany(l => l.Depots)
                .HasForeignKey(d => d.LocId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            modelBuilder.Entity<Client>()
                .HasOne(c => c.Location)
                .WithMany(l => l.Clients)
                .HasForeignKey(c => c.LocId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            base.OnModelCreating(modelBuilder);
            List<IdentityRole<Guid>> roles = new List<IdentityRole<Guid>>
            {
                new IdentityRole<Guid>
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole<Guid>
                {
                    Id=Guid.NewGuid(),
                    Name = "User",
                    NormalizedName = "USER"
                }
            };
            modelBuilder.Entity<IdentityRole<Guid>>().HasData(roles);
        }
    }
}
