using System.Reflection;
using LogBook.Database.Model;
using Microsoft.EntityFrameworkCore;

namespace LogBook.Database
{
    public class LogBookDbContext : DbContext
    {
        public LogBookDbContext()
        {   
        }
        
        public LogBookDbContext(DbContextOptions<LogBookDbContext> options) : base(options)
        {
        }
        
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Route> Routes { get; set; }
        public virtual DbSet<WayPoint> WayPoints { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.HasPostgresExtension("postgis");
            
            modelBuilder.Entity<User>(
                e =>
                {
                    e.ToTable("users");
                    e.HasKey(x => x.Id);
                    e.Property(x => x.Id)
                        .ValueGeneratedOnAdd();
                   
                    e.HasMany<Route>(x => x.Routes)
                        .WithOne(x => x.User)
                        .HasForeignKey(x => x.UserId);
                }
            );
            
            modelBuilder.Entity<Route>(
                e =>
                {
                    e.ToTable("routes");
                    e.HasKey(x => x.Id);
                    e.Property(x => x.Id)
                        .ValueGeneratedOnAdd();
                    e.Property(x => x.Date)
                        .HasUTCConversion();
                    e.HasMany<WayPoint>(x => x.WayPoints)
                        .WithOne(x => x.Route)
                        .HasForeignKey(x => x.RouteId);
                }
            );


            modelBuilder.Entity<WayPoint>(
                e =>
                {
                    e.ToTable("waypoints");
                    e.HasKey(x => x.Id);
                    e.Property(x => x.Id)
                        .ValueGeneratedOnAdd();
                    e.Property(x => x.Date)
                        .HasUTCConversion();
                }
            );
        }
    }
}
