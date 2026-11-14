using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EducationCenter.Data;
using EducationCenter.Models;

namespace EducationCenter.Controllers
{
    public class StudentsController : Controller
    {
        private readonly AppDbContext _context;

        public StudentsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? search)
        {
            var q = _context.Students.Include(s => s.Enrollments).AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
                q = q.Where(s => s.FullName.Contains(search) || s.Email.Contains(search));
            ViewBag.Search = search;
            return View(await q.OrderBy(s => s.FullName).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var s = await _context.Students
                .Include(x => x.Enrollments).ThenInclude(e => e.Course)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (s == null) return NotFound();
            return View(s);
        }

        public IActionResult Create()
        {
            return View(new Student());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            if (!ModelState.IsValid) return View(student);
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            TempData["Msg"] = "Student added.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var s = await _context.Students.FindAsync(id);
            if (s == null) return NotFound();
            return View(s);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student student)
        {
            if (id != student.Id) return NotFound();
            if (!ModelState.IsValid) return View(student);

            var existing = await _context.Students.FindAsync(id);
            if (existing == null) return NotFound();

            existing.FullName = student.FullName;
            existing.Email = student.Email;
            existing.Phone = student.Phone;
            existing.DateOfBirth = student.DateOfBirth;

            await _context.SaveChangesAsync();
            TempData["Msg"] = "Student updated.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var s = await _context.Students.Include(x => x.Enrollments)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (s == null) return NotFound();
            return View(s);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var s = await _context.Students.FindAsync(id);
            if (s != null)
            {
                _context.Students.Remove(s);
                await _context.SaveChangesAsync();
                TempData["Msg"] = "Student deleted.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
