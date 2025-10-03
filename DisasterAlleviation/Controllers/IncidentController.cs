using DisasterAlleviationFoundation.Data;
using DisasterAlleviationFoundation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

public class IncidentController : Controller
{
    private readonly ApplicationDbContext _context;

    public IncidentController(ApplicationDbContext context) { _context = context; }

    // GET: Report Incident
    [HttpGet]
    public IActionResult Report()
    {
        var name = HttpContext.Session.GetString("VolunteerName");
        if (string.IsNullOrEmpty(name)) return RedirectToAction("Login", "Volunteer");

        ViewBag.VolunteerName = name;
        return View();
    }

    // POST: Report Incident
    [HttpPost]
    public IActionResult Report(string title, string description, string location)
    {
        if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description))
        {
            ViewBag.Error = "Title and Description are required!";
            return View();
        }

        _context.Incidents.Add(new Incident { Title = title, Description = description, Location = location });
        _context.SaveChanges();
        ViewBag.Message = "Incident reported successfully!";
        return View();
    }

    // List incidents
    public IActionResult List()
    {
        var incidents = _context.Incidents.ToList();
        return View(incidents);
    }

    // GET: Register for a Task
    [HttpGet]
    public IActionResult RegisterTask()
    {
        var volunteerName = HttpContext.Session.GetString("VolunteerName");
        if (string.IsNullOrEmpty(volunteerName)) return RedirectToAction("Login", "Volunteer");

        ViewBag.VolunteerName = volunteerName;
        ViewBag.IncidentOptions = new[] { "Earthquake Rescue", "Flood Rescue", "Fire Rescue", "Storm Rescue", "Landslide Rescue", "Tsunami Rescue", "Other" };
        return View();
    }

    [HttpPost]
    public IActionResult RegisterTask(string volunteerName, string incidentTitle)
    {
        if (string.IsNullOrEmpty(volunteerName) || string.IsNullOrEmpty(incidentTitle))
        {
            ViewBag.Error = "Both volunteer name and incident title are required!";
            ViewBag.IncidentOptions = new[] { "Earthquake Rescue", "Flood Rescue", "Fire Rescue", "Storm Rescue", "Landslide Rescue", "Tsunami Rescue", "Other" };
            return View();
        }

        _context.TaskRegistrations.Add(new TaskRegistration { VolunteerName = volunteerName, IncidentTitle = incidentTitle});
        _context.SaveChanges();

        return RedirectToAction("TaskList");
    }

    public IActionResult TaskList() => View(_context.TaskRegistrations.ToList());
}
