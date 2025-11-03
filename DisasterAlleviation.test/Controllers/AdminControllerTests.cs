using DisasterAlleviationFoundation.Controllers;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Xunit;

namespace DisasterAlleviationFoundation.Tests
{
    public class AdminControllerTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "AdminTestDB")
                .Options;

            return new ApplicationDbContext(options);
        }

        private DefaultHttpContext GetHttpContextWithSession(bool isAdmin = false)
        {
            var context = new DefaultHttpContext();
            context.Features.Set<ISessionFeature>(new SessionFeature());
            context.Session = new MockSession();

            if (isAdmin)
                context.Session.SetString("IsAdmin", "true");

            return context;
        }

        [Fact]
        public void Login_ValidCredentials_RedirectsToDashboard()
        {
            var db = GetDbContext();
            var controller = new AdminController(db);
            controller.ControllerContext.HttpContext = GetHttpContextWithSession();

            var result = controller.Login("disasteradmin", "Alleviation") as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Dashboard", result.ActionName);
            Assert.Equal("true", controller.HttpContext.Session.GetString("IsAdmin"));
        }

        [Fact]
        public void Login_InvalidCredentials_ReturnsViewWithError()
        {
            var db = GetDbContext();
            var controller = new AdminController(db);
            controller.ControllerContext.HttpContext = GetHttpContextWithSession();

            var result = controller.Login("wrong", "wrong") as ViewResult;

            Assert.NotNull(result);
            Assert.Equal("Invalid credentials", controller.ViewBag.Error);
        }

        [Fact]
        public void Dashboard_NoAdminSession_RedirectsToLogin()
        {
            var db = GetDbContext();
            var controller = new AdminController(db);
            controller.ControllerContext.HttpContext = GetHttpContextWithSession(false);

            var result = controller.Dashboard() as RedirectToActionResult;

            Assert.Equal("Login", result.ActionName);
        }

        [Fact]
        public void Dashboard_AdminSession_ReturnsView()
        {
            var db = GetDbContext();
            var controller = new AdminController(db);
            controller.ControllerContext.HttpContext = GetHttpContextWithSession(true);

            var result = controller.Dashboard() as ViewResult;

            Assert.NotNull(result);
        }

        [Fact]
        public void AcceptDonation_UpdatesDonationStatus()
        {
            var db = GetDbContext();
            db.Donations.Add(new Donation { Id = 1, DonorName = "A", Item = "Food", Quantity = 5, Status = "Pending" });
            db.SaveChanges();

            var controller = new AdminController(db);
            controller.ControllerContext.HttpContext = GetHttpContextWithSession(true);

            controller.AcceptDonation(1);
            var donation = db.Donations.Find(1);

            Assert.Equal("Accepted", donation.Status);
        }

        [Fact]
        public void DenyDonation_UpdatesDonationStatus()
        {
            var db = GetDbContext();
            db.Donations.Add(new Donation { Id = 2, DonorName = "A", Item = "Water", Quantity = 3, Status = "Pending" });
            db.SaveChanges();

            var controller = new AdminController(db);
            controller.ControllerContext.HttpContext = GetHttpContextWithSession(true);

            controller.DenyDonation(2);
            var donation = db.Donations.Find(2);

            Assert.Equal("Denied", donation.Status);
        }

        [Fact]
        public void RemoveVolunteer_DeletesVolunteer()
        {
            var db = GetDbContext();
            db.Volunteers.Add(new Volunteer { Id = 1, Name = "John", Password = "123" });
            db.SaveChanges();

            var controller = new AdminController(db);
            controller.ControllerContext.HttpContext = GetHttpContextWithSession(true);

            controller.RemoveVolunteer(1);
            var volunteer = db.Volunteers.Find(1);

            Assert.Null(volunteer);
        }

        [Fact]
        public void RemoveTask_DeletesTask()
        {
            var db = GetDbContext();
            db.TaskRegistrations.Add(new TaskRegistration { Id = 1, VolunteerName = "Sam", IncidentTitle = "Fire" });
            db.SaveChanges();

            var controller = new AdminController(db);
            controller.ControllerContext.HttpContext = GetHttpContextWithSession(true);

            controller.RemoveTask(1);
            var task = db.TaskRegistrations.Find(1);

            Assert.Null(task);
        }

        [Fact]
        public void Logout_ClearsSession()
        {
            var db = GetDbContext();
            var controller = new AdminController(db);
            controller.ControllerContext.HttpContext = GetHttpContextWithSession(true);

            controller.Logout();

            Assert.Null(controller.HttpContext.Session.GetString("IsAdmin"));
        }
    }

    // ✅ Mock Session (same working version used earlier)
    public class MockSession : ISession
    {
        Dictionary<string, byte[]> _sessionStorage = new Dictionary<string, byte[]>();
        public string Id => "mock";
        public bool IsAvailable => true;
        public IEnumerable<string> Keys => _sessionStorage.Keys;
        public void Clear() => _sessionStorage.Clear();
        public void Remove(string key) => _sessionStorage.Remove(key);
        public void Set(string key, byte[] value) => _sessionStorage[key] = value;
        public bool TryGetValue(string key, out byte[] value) => _sessionStorage.TryGetValue(key, out value);
        public void CommitAsync() { }
        public void LoadAsync() { }
        public Task CommitAsync(CancellationToken token = default) => Task.CompletedTask;
        public Task LoadAsync(CancellationToken token = default) => Task.CompletedTask;
    }

    public class SessionFeature : ISessionFeature
    {
        public ISession Session { get; set; }
    }
}
