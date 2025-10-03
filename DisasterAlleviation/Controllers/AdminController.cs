using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace DisasterAlleviationFoundation.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Hard-coded admin credentials
        private const string AdminUsername = "disasteradmin";
        private const string AdminPassword = "Alleviation";

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin login page
        [HttpGet]
        public IActionResult Login() => View();

        // POST: Login
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (username == AdminUsername && password == AdminPassword)
            {
                HttpContext.Session.SetString("IsAdmin", "true");
                return RedirectToAction("Dashboard");
            }

            ViewBag.Error = "Invalid credentials";
            return View();
        }

        // GET: Admin dashboard
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Login");

            ViewBag.Volunteers = _context.Volunteers.ToList();
            ViewBag.Donations = _context.Donations.ToList();
            ViewBag.TaskRegistrations = _context.TaskRegistrations.ToList();

            return View();
        }

        // Remove a volunteer
        public IActionResult RemoveVolunteer(int id)
        {
            var volunteer = _context.Volunteers.Find(id);
            if (volunteer != null)
            {
                _context.Volunteers.Remove(volunteer);
                _context.SaveChanges();
            }
            return RedirectToAction("Dashboard");
        }

        // Remove a task
        public IActionResult RemoveTask(int id)
        {
            var task = _context.TaskRegistrations.Find(id);
            if (task != null)
            {
                _context.TaskRegistrations.Remove(task);
                _context.SaveChanges();
            }
            return RedirectToAction("Dashboard");
        }

        // Accept donation
        public IActionResult AcceptDonation(int id)
        {
            var donation = _context.Donations.Find(id);
            if (donation != null)
            {
                donation.Status = "Accepted";
                _context.SaveChanges();
            }
            return RedirectToAction("Dashboard");
        }

        // Deny donation
        public IActionResult DenyDonation(int id)
        {
            var donation = _context.Donations.Find(id);
            if (donation != null)
            {
                donation.Status = "Denied";
                _context.SaveChanges();
            }
            return RedirectToAction("Dashboard");
        }

        // Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("IsAdmin");
            return RedirectToAction("Login");
        }
    }
}
