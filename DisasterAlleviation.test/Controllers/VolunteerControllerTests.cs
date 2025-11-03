using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DisasterAlleviationFoundation.Controllers;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using DisasterAlleviation.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace DisasterAlleviation.Tests
{
    public class VolunteerControllerTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Volunteer_TestDB")
                .Options;

            return new ApplicationDbContext(options);
        }

        private VolunteerController GetController(ApplicationDbContext context)
        {
            var controller = new VolunteerController(context)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };

            controller.HttpContext.Session = new MockSession();
            return controller;
        }

        [Fact]
        public void Register_Post_AddsVolunteer_AndRedirectsToLogin()
        {
            var context = GetDbContext();
            var controller = GetController(context);

            var newVolunteer = new Volunteer { Name = "John", Password = "1234" };

            var result = controller.Register(newVolunteer);

            Assert.Equal(1, context.Volunteers.Count());
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Login", ((RedirectToActionResult)result).ActionName);
        }

        [Fact]
        public void Login_ValidUser_SetsSession_AndRedirectsToProfile()
        {
            var context = GetDbContext();
            context.Volunteers.Add(new Volunteer { Name = "Alice", Password = "pass" });
            context.SaveChanges();

            var controller = GetController(context);

            var result = controller.Login("Alice", "pass");

            Assert.Equal("Alice", controller.HttpContext.Session.GetString("VolunteerName"));
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Profile", ((RedirectToActionResult)result).ActionName);
        }

        [Fact]
        public void Login_InvalidUser_ReturnsView()
        {
            var context = GetDbContext();
            var controller = GetController(context);

            var result = controller.Login("Wrong", "User");

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Delete_RemovesVolunteerAndClearsSession()
        {
            var context = GetDbContext();
            context.Volunteers.Add(new Volunteer { Id = 1, Name = "Sam", Password = "111" });
            context.SaveChanges();

            var controller = GetController(context);

            var result = controller.Delete(1);

            Assert.Equal(0, context.Volunteers.Count());
            Assert.Null(controller.HttpContext.Session.GetString("VolunteerName"));
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Register", ((RedirectToActionResult)result).ActionName);
        }
    }
}
