using Xunit;
using Microsoft.EntityFrameworkCore;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using System.Linq;

namespace DisasterAlleviationFoundation.Tests.Integration
{
    public class DatabaseIntegrationTests
    {
        private const string TestConnectionString = "Server=tcp:thando.database.windows.net,1433;Initial Catalog=Sihle;Persist Security Info=False;User ID=weadmin;Password=Mvus3n1@40;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(TestConnectionString)
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public void AddIncident_Should_SaveToDatabase()
        {
            using var context = GetDbContext();
            using var transaction = context.Database.BeginTransaction();

            var incident = new Incident
            {
                Title = "Test Fire Incident",
                Location = "Test City",
                Description = "This is a test incident.",
                DateReported = System.DateTime.Now,
                Severity = "High"
            };

            // Act
            context.Incidents.Add(incident);
            context.SaveChanges();

            // Assert
            var saved = context.Incidents.FirstOrDefault(i => i.Title == "Test Fire Incident");
            Assert.NotNull(saved);
            Assert.Equal("Test City", saved.Location);

            // Rollback so test database remains clean
            transaction.Rollback();
        }

        [Fact]
        public void UpdateIncident_Should_ModifyRecordInDatabase()
        {
            using var context = GetDbContext();
            using var transaction = context.Database.BeginTransaction();

            var incident = new Incident
            {
                Title = "Test Flood Incident",
                Location = "City A",
                Description = "Initial description",
                DateReported = System.DateTime.Now,
                Severity = "Medium"
            };

            context.Incidents.Add(incident);
            context.SaveChanges();

            // Act
            incident.Description = "Updated description";
            context.Incidents.Update(incident);
            context.SaveChanges();

            // Assert
            var updated = context.Incidents.FirstOrDefault(i => i.Title == "Test Flood Incident");
            Assert.Equal("Updated description", updated.Description);

            transaction.Rollback();
        }

        [Fact]
        public void DeleteIncident_Should_RemoveRecordFromDatabase()
        {
            using var context = GetDbContext();
            using var transaction = context.Database.BeginTransaction();

            var incident = new Incident
            {
                Title = "Test Storm Incident",
                Location = "City B",
                Description = "This incident will be deleted.",
                DateReported = System.DateTime.Now,
                Severity = "Low"
            };

            context.Incidents.Add(incident);
            context.SaveChanges();

            // Act
            context.Incidents.Remove(incident);
            context.SaveChanges();

            // Assert
            var deleted = context.Incidents.FirstOrDefault(i => i.Title == "Test Storm Incident");
            Assert.Null(deleted);

            transaction.Rollback();
        }
    }
}
