using DisasterAlleviationFoundation.Controllers;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DisasterAlleviation.Tests
{
    // ✅ Mock Session class so controller can use HttpContext.Session
    public class MockSession : ISession
    {
        Dictionary<string, byte[]> _store = new Dictionary<string, byte[]>();

        public IEnumerable<string> Keys => _store.Keys;

        public string Id => "MockSessionId";

        public bool IsAvailable => true;

        public void Clear() => _store.Clear();

        public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

        public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

        public void Remove(string key) => _store.Remove(key);

        public void Set(string key, byte[] value) => _store[key] = value;

        public bool TryGetValue(string key, out byte[] value) => _store.TryGetValue(key, out value);
    }

    public class IncidentControllerTests
    {
        private ApplicationDbContext GetDb()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "IncidentTestDB")
                .Options;
            return new ApplicationDbContext(options);
        }

        private IncidentController GetController(ApplicationDbContext db, bool loggedIn = true)
        {
            var controller = new IncidentController(db);

            var context = new DefaultHttpContext();
            context.Session = new MockSession();
            if (loggedIn)
                context.Session.Set("VolunteerName", System.Text.Encoding.UTF8.GetBytes("Tester"));

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = context
            };

            return controller;
        }

        // ✅ GET Report redirect if not logged in
        [Fact]
        public void Report_Get_Redirects_When_Not_Logged()
        {
            var db = GetDb();
            var controller = GetController(db, loggedIn: false);

            var result = controller.Report() as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Login", result.ActionName);
            Assert.Equal("Volunteer", result.ControllerName);
        }

        // ✅ GET Report returns View when logged in
        [Fact]
        public void Report_Get_Returns_View_When_Logged_In()
        {
            var db = GetDb();
            var controller = GetController(db);

            var result = controller.Report();

            Assert.IsType<ViewResult>(result);
        }

        // ✅ POST Report fails when missing fields
        [Fact]
        public void Report_Post_Returns_View_On_Missing_Fields()
        {
            var db = GetDb();
            var controller = GetController(db);

            var result = controller.Report("", "", "Cape Town");

            Assert.IsType<ViewResult>(result);
            Assert.True(controller.ViewBag.Error != null);
        }

        // ✅ POST Report saves data successfully
        [Fact]
        public void Report_Post_Saves_Incident()
        {
            var db = GetDb();
            var controller = GetController(db);

            controller.Report("Fire", "Wildfire", "Cape Town");

            Assert.Equal(1, db.Incidents.CountAsync().Result);
        }

        // ✅ List returns view with incidents
        [Fact]
        public void List_Returns_Incident_List()
        {
            var db = GetDb();
            db.Incidents.Add(new Incident { Title = "Flood" });
            db.SaveChanges();

            var controller = GetController(db);
            var result = controller.List() as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<List<Incident>>(result.Model);
        }

        // ✅ RegisterTask GET redirects if not logged
        [Fact]
        public void RegisterTask_Get_Redirects_Not_Logged()
        {
            var db = GetDb();
            var controller = GetController(db, loggedIn: false);

            var result = controller.RegisterTask() as RedirectToActionResult;

            Assert.Equal("Login", result.ActionName);
        }

        // ✅ RegisterTask POST saves task
        [Fact]
        public void RegisterTask_Post_Saves_Task()
        {
            var db = GetDb();
            var controller = GetController(db);

            var result = controller.RegisterTask("John", "Flood Rescue") as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal(1, db.TaskRegistrations.CountAsync().Result);
        }
    }
}
