using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DisasterAlleviationFoundation.Controllers;
using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using System.Linq;

namespace DisasterAlleviation.Tests
{
    public class DonationControllerTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("DonationDB_Test")
                .Options;

            return new ApplicationDbContext(options);
        }

        private DonationController GetController(ApplicationDbContext context)
        {
            return new DonationController(context);
        }

        [Fact]
        public void List_ShouldReturnAllDonations()
        {
            var context = GetDbContext();
            context.Donations.Add(new Donation { DonorName = "TestDonor", Item = "Food", Quantity = 5 });
            context.SaveChanges();

            var controller = GetController(context);
            var result = controller.List() as ViewResult;

            Assert.NotNull(result);
            var model = Assert.IsAssignableFrom<System.Collections.Generic.List<Donation>>(result.Model);
            Assert.Single(model);
        }

        [Fact]
        public void Add_Get_ReturnsView()
        {
            var context = GetDbContext();
            var controller = GetController(context);

            var result = controller.Add();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Add_Post_ValidDonation_SavesAndRedirects()
        {
            var context = GetDbContext();
            var controller = GetController(context);

            var result = controller.Add("John", "Water", 10);

            Assert.Equal(1, context.Donations.Count());
            var donation = context.Donations.First();
            Assert.Equal("John", donation.DonorName);
            Assert.Equal("Water", donation.Item);
            Assert.Equal(10, donation.Quantity);
            Assert.Equal("Pending", donation.Status);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("List", redirect.ActionName);
        }

        [Fact]
        public void Add_Post_InvalidDonation_DoesNotSave_RedirectsToList()
        {
            var context = GetDbContext();
            var controller = GetController(context);

            var result = controller.Add("", "", 0);

            Assert.Equal(0, context.Donations.Count());
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("List", redirect.ActionName);
        }
    }
}
