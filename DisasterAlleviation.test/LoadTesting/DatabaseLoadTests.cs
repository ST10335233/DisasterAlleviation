using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;

namespace DisasterAlleviationFoundation.Tests.Load
{
    public class DatabaseLoadTests
    {
        // Use your Azure SQL test database connection string
        private const string TestConnectionString = "Server=tcp:thando.database.windows.net,1433;Initial Catalog=SihleTest;Persist Security Info=False;User ID=weadmin;Password=Mvus3n1@40;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        private ApplicationDbContext GetTestDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(TestConnectionString)
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task SimulateConcurrentIncidentInserts()
        {
            int numberOfUsers = 50; // simulate 50 concurrent users

            var tasks = Enumerable.Range(1, numberOfUsers).Select(async i =>
            {
                using var context = GetTestDbContext();
                using var transaction = await context.Database.BeginTransactionAsync();

                var incident = new Incident
                {
                    Title = $"Load Test Incident {i}",
                    Location = "Test City",
                    Description = "Testing load",
                    DateReported = DateTime.Now,
                    Severity = "Medium"
                };

                context.Incidents.Add(incident);
                await context.SaveChangesAsync();

                // Rollback to keep test database clean
                await transaction.RollbackAsync();
            });

            await Task.WhenAll(tasks);
        }
    }
}
