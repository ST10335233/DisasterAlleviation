using Microsoft.EntityFrameworkCore;
using DisasterAlleviationFoundation.Models;

namespace DisasterAlleviationFoundation.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<TaskRegistration> TaskRegistrations { get; set; }
        public DbSet<Incident> Incidents { get; set; }

    }
}
