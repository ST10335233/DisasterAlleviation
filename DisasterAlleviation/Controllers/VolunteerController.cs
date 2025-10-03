using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using Microsoft.AspNetCore.Mvc;

namespace DisasterAlleviationFoundation.Controllers
{
    public class VolunteerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VolunteerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Show all volunteers
        public IActionResult List()
        {
            return View(_context.Volunteers.ToList());
        }

        // ✅ Register page (GET)
        [HttpGet]
        public IActionResult Register() => View();

        // ✅ Register (POST)
        [HttpPost]
        public IActionResult Register(Volunteer volunteer)
        {
            if (ModelState.IsValid)
            {
                _context.Volunteers.Add(volunteer);
                _context.SaveChanges();
                return RedirectToAction("Login"); // go to login instead of list
            }
            return View(volunteer);
        }



        // ✅ Login page (GET)
        [HttpGet]
        public IActionResult Login() => View();

        // ✅ Login (POST)
        [HttpPost]
        public IActionResult Login(string name, string password)
        {
            // Look up volunteer in the database
            var volunteer = _context.Volunteers
                .FirstOrDefault(v => v.Name == name && v.Password == password);

            if (volunteer != null)
            {
                // Save volunteer session
                HttpContext.Session.SetString("VolunteerName", volunteer.Name);
                return RedirectToAction("Profile");
            }

            ViewBag.Error = "Invalid login credentials. Please try again.";
            return View();
        }


        // ✅ Volunteer Profile
        public IActionResult Profile()
        {
            var name = HttpContext.Session.GetString("VolunteerName");
            if (string.IsNullOrEmpty(name))
                return RedirectToAction("Login");

            var volunteer = _context.Volunteers.FirstOrDefault(v => v.Name == name);
            return View(volunteer);
        }

        // ✅ Edit Profile (GET)
        [HttpGet]
        public IActionResult Edit()
        {
            var name = HttpContext.Session.GetString("VolunteerName");
            if (string.IsNullOrEmpty(name))
                return RedirectToAction("Login");

            var volunteer = _context.Volunteers.FirstOrDefault(v => v.Name == name);
            return View(volunteer);
        }

        // ✅ Edit Profile (POST)
        [HttpPost]
        public IActionResult Edit(Volunteer updatedVolunteer)
        {
            var volunteer = _context.Volunteers.FirstOrDefault(v => v.Id == updatedVolunteer.Id);
            if (volunteer != null)
            {
                volunteer.Name = updatedVolunteer.Name;
                volunteer.Password = updatedVolunteer.Password; // ⚠️ should hash!
                volunteer.IsRegistered = updatedVolunteer.IsRegistered;

                _context.Update(volunteer);
                _context.SaveChanges();

                // Refresh session with updated name
                HttpContext.Session.SetString("VolunteerName", volunteer.Name);

                return RedirectToAction("Profile");
            }
            return View(updatedVolunteer);
        }

        // ✅ Delete Profile
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var volunteer = _context.Volunteers.FirstOrDefault(v => v.Id == id);
            if (volunteer != null)
            {
                _context.Volunteers.Remove(volunteer);
                _context.SaveChanges();
                HttpContext.Session.Clear();
                return RedirectToAction("Register");
            }
            return RedirectToAction("Profile");
        }

        // ✅ Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
