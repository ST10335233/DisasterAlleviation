using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;

namespace DisasterAlleviationFoundation.Tests.Load
{
    public class StressTest
    {
        private const string TestConnectionString = "Server=tcp:thando.database.windows.net,1433;Initial Catalog=SihleTest;Persist Security Info=False;User ID=weadmin;Password=Mvus3n1@40;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        private ApplicationDbContext GetTestDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(TestConnectionString)
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task HeavyConcurrentReadAndWrite()
        {
            int users = 100; // simulate 100 concurrent users

            var tasks = Enumerable.Range(1, users).Select(async i =>
            {
                using var context = GetTestDbContext();
                using var transaction = await context.Database.BeginTransactionAsync();

                // 50% chance to insert, 50% to read
                if (i % 2 == 0)
                {
                    var incident = new Incident
                    {
                        Title = $"Stress Incident {i}",
                        Location = "Test City",
                        Description = "Stress testing",
                        DateReported = DateTime.Now,
                        Severity = "High"
                    };
                    context.Incidents.Add(incident);
                    await context.SaveChangesAsync();
                }
                else
                {
                    var count = await context.Incidents.CountAsync();
                }

                await transaction.RollbackAsync(); // keep DB clean
            });

            await Task.WhenAll(tasks);
        }
    }
}
