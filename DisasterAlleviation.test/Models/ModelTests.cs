using System;
using Xunit;
using DisasterAlleviationFoundation.Models;

namespace DisasterAlleviationFoundation.Tests.Models
{
    public class ModelTests
    {
        // ====== Volunteer Tests ======
        [Fact]
        public void Volunteer_DefaultValues_ShouldBeSetCorrectly()
        {
            var volunteer = new Volunteer();

            Assert.Equal("", volunteer.Name);
            Assert.Equal("", volunteer.Password);
            Assert.True(volunteer.IsRegistered);
        }

        [Fact]
        public void Volunteer_CanSetPropertiesCorrectly()
        {
            var volunteer = new Volunteer
            {
                Name = "John Doe",
                Password = "password123",
                IsRegistered = false
            };

            Assert.Equal("John Doe", volunteer.Name);
            Assert.Equal("password123", volunteer.Password);
            Assert.False(volunteer.IsRegistered);
        }

        // ====== IncidentReport Tests ======
        [Fact]
        public void IncidentReport_DefaultValues_ShouldBeSetCorrectly()
        {
            var report = new IncidentReport();

            Assert.Equal("", report.Title);
            Assert.Equal("", report.Description);
            Assert.Equal("", report.Location);
            Assert.True((DateTime.Now - report.DateReported).TotalSeconds < 5); // DateReported auto-filled
        }

        [Fact]
        public void IncidentReport_CanSetPropertiesCorrectly()
        {
            var now = DateTime.Now;
            var report = new IncidentReport
            {
                Title = "Test Incident",
                Description = "Details here",
                Location = "City A",
                DateReported = now
            };

            Assert.Equal("Test Incident", report.Title);
            Assert.Equal("Details here", report.Description);
            Assert.Equal("City A", report.Location);
            Assert.Equal(now, report.DateReported);
        }

        // ====== TaskRegistration Tests ======
        [Fact]
        public void TaskRegistration_DefaultValues_ShouldBeEmpty()
        {
            var task = new TaskRegistration();

            Assert.Null(task.VolunteerName);
            Assert.Null(task.IncidentTitle);
        }

        [Fact]
        public void TaskRegistration_CanSetPropertiesCorrectly()
        {
            var task = new TaskRegistration
            {
                VolunteerName = "John Doe",
                IncidentTitle = "Fire Incident"
            };

            Assert.Equal("John Doe", task.VolunteerName);
            Assert.Equal("Fire Incident", task.IncidentTitle);
        }

        // ====== Incident Tests ======
        [Fact]
        public void Incident_DefaultValues_ShouldBeSetCorrectly()
        {
            var incident = new Incident();

            Assert.Equal("", incident.Title);
            Assert.Equal("", incident.Description);
            Assert.Equal("", incident.Location);
            Assert.True((DateTime.Now - incident.DateReported).TotalSeconds < 5);
            Assert.Null(incident.Severity); // Severity is a field, default null
        }

        [Fact]
        public void Incident_CanSetPropertiesCorrectly()
        {
            var now = DateTime.Now;
            var incident = new Incident
            {
                Title = "Flood",
                Description = "Severe flood in area",
                Location = "City B",
                DateReported = now,
                Severity = "High"
            };

            Assert.Equal("Flood", incident.Title);
            Assert.Equal("Severe flood in area", incident.Description);
            Assert.Equal("City B", incident.Location);
            Assert.Equal(now, incident.DateReported);
            Assert.Equal("High", incident.Severity);
        }

        // ====== Donation Tests ======
        [Fact]
        public void Donation_DefaultValues_ShouldBeSetCorrectly()
        {
            var donation = new Donation();

            Assert.Equal("", donation.DonorName);
            Assert.Equal("", donation.Item);
            Assert.Equal(0, donation.Quantity);
            Assert.Equal("Pending", donation.Status);
        }

        [Fact]
        public void Donation_CanSetPropertiesCorrectly()
        {
            var donation = new Donation
            {
                DonorName = "Jane Smith",
                Item = "Blankets",
                Quantity = 50,
                Status = "Delivered"
            };

            Assert.Equal("Jane Smith", donation.DonorName);
            Assert.Equal("Blankets", donation.Item);
            Assert.Equal(50, donation.Quantity);
            Assert.Equal("Delivered", donation.Status);
        }

        // ====== Admin Tests ======
        [Fact]
        public void Admin_DefaultValues_ShouldBeEmpty()
        {
            var admin = new Admin();

            Assert.Equal("", admin.Username);
            Assert.Equal("", admin.Password);
        }

        [Fact]
        public void Admin_CanSetPropertiesCorrectly()
        {
            var admin = new Admin
            {
                Username = "admin1",
                Password = "secret"
            };

            Assert.Equal("admin1", admin.Username);
            Assert.Equal("secret", admin.Password);
        }
    }
}
