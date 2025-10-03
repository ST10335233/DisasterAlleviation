using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using Microsoft.AspNetCore.Mvc;

namespace DisasterAlleviationFoundation.Controllers
{
    public class DonationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DonationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Show donations
        public IActionResult List()
        {
            return View(_context.Donations.ToList());
        }

        // Add donation
        [HttpGet]
        public IActionResult Add() => View();

        [HttpPost]
        public IActionResult Add(string donorName, string item, int quantity)
        {
            if (!string.IsNullOrEmpty(donorName) && !string.IsNullOrEmpty(item) && quantity > 0)
            {
                var donation = new Donation
                {
                    DonorName = donorName,
                    Item = item,
                    Quantity = quantity,
                    Status = "Pending"
                };
                _context.Donations.Add(donation);
                _context.SaveChanges();
            }
            return RedirectToAction("List");
        }
    }
}
