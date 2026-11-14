using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EducationCenter.Data;
using EducationCenter.Models;

namespace EducationCenter.Controllers
{
    public class CoursesController : Controller
    {
        private readonly AppDbContext _context;

        public CoursesController(AppDbContext context)
        {
            _context = context;
        }

        // List of all courses: search, filter, sort, paging
        public async Task<IActionResult> Index(string? search, string? sort, string? category, string? status, int page = 1)
        {
            const int pageSize = 9;
            var q = _context.Courses.Include(c => c.Enrollments).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                q = q.Where(c => c.Title.Contains(search) || c.Instructor.Contains(search));
            if (!string.IsNullOrWhiteSpace(category))
                q = q.Where(c => c.Category == category);
            if (!string.IsNullOrWhiteSpace(status))
                q = q.Where(c => c.Status == status);

            q = sort switch
            {
                "price" => q.OrderBy(c => c.Price),
                "date" => q.OrderBy(c => c.StartDate),
                "newest" => q.OrderByDescending(c => c.CreatedAt),
                _ => q.OrderBy(c => c.Title)
            };

            var total = await q.CountAsync();
            ViewBag.Search = search;
            ViewBag.Sort = sort;
            ViewBag.Category = category;
            ViewBag.Status = status;
            ViewBag.Total = total;
            ViewBag.Page = page;
            ViewBag.Pages = (int)Math.Ceiling(total / (double)pageSize);

            return View(await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var course = await _context.Courses
                .Include(c => c.Enrollments).ThenInclude(e => e.Student)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (course == null) return NotFound();
            return View(course);
        }

        public IActionResult Create()
        {
            return View(new Course());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course course)
        {
            if (!ModelState.IsValid)
            {
                return View(course);
            }
            course.CreatedAt = DateTime.Now;
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            TempData["Msg"] = "Course created.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Course course)
        {
            if (id != course.Id) return NotFound();
            if (!ModelState.IsValid) return View(course);

            var existing = await _context.Courses.FindAsync(id);
            if (existing == null) return NotFound();

            existing.Title = course.Title;
            existing.Description = course.Description;
            existing.Instructor = course.Instructor;
            existing.Category = course.Category;
            existing.Level = course.Level;
            existing.Price = course.Price;
            existing.Capacity = course.Capacity;
            existing.StartDate = course.StartDate;
            existing.Status = course.Status;

            await _context.SaveChangesAsync();
            TempData["Msg"] = "Course updated.";
            return RedirectToAction(nameof(Index));
        }

        // Quick status change from the Details page
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id, string status)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();
            course.Status = status;
            await _context.SaveChangesAsync();
            TempData["Msg"] = $"Status changed to {status}.";
            return RedirectToAction(nameof(Details), new { id });
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var course = await _context.Courses.Include(c => c.Enrollments)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (course == null) return NotFound();
            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
                TempData["Msg"] = "Course deleted.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
