using Microsoft.EntityFrameworkCore;
using SBM_System_Backend.Entity;

namespace SBM_System_Backend.AppDbContext
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Booking> Bookings { get; set; }
    }
}
