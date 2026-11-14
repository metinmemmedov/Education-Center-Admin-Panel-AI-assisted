using EducationCenter.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducationCenter.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _db;
    public HomeController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        ViewBag.CourseCount = await _db.Courses.CountAsync();
        ViewBag.StudentCount = await _db.Students.CountAsync();
        ViewBag.EnrollmentCount = await _db.Enrollments.CountAsync();
        ViewBag.InstructorCount = await _db.Courses.Select(c => c.Instructor).Distinct().CountAsync();
        ViewBag.CategoryCounts = await _db.Courses.GroupBy(c => c.Category)
            .Select(g => new { Name = g.Key, Count = g.Count() }).ToDictionaryAsync(x => x.Name, x => x.Count);

        var upcoming = await _db.Courses.Include(c => c.Enrollments)
            .Where(c => c.StartDate >= DateTime.Today)
            .OrderBy(c => c.StartDate).Take(6).ToListAsync();
        return View(upcoming);
    }

    public IActionResult Error() => Content("Something went wrong.");
}
