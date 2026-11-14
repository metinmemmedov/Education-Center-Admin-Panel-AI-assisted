using EducationCenter.Data;
using EducationCenter.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EducationCenter.Controllers;

public class EnrollmentsController : Controller
{
    private readonly AppDbContext _db;
    public EnrollmentsController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index() =>
        View(await _db.Enrollments.Include(e => e.Course).Include(e => e.Student)
            .OrderByDescending(e => e.EnrolledOn).ToListAsync());

    public async Task<IActionResult> Create(int? courseId, int? studentId)
    {
        await LoadListsAsync(courseId, studentId);
        return View(new Enrollment { CourseId = courseId ?? 0, StudentId = studentId ?? 0 });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Enrollment e)
    {
        var course = await _db.Courses.Include(c => c.Enrollments).FirstOrDefaultAsync(c => c.Id == e.CourseId);
        if (course is null)
            ModelState.AddModelError("", "Please choose a course.");
        else if (course.Enrollments.Count >= course.Capacity)
            ModelState.AddModelError("", "This course is full.");
        else if (course.Enrollments.Any(x => x.StudentId == e.StudentId))
            ModelState.AddModelError("", "Student is already enrolled in this course.");

        ModelState.Remove(nameof(Enrollment.Course));
        ModelState.Remove(nameof(Enrollment.Student));

        if (!ModelState.IsValid)
        {
            await LoadListsAsync(e.CourseId, e.StudentId);
            return View(e);
        }

        _db.Add(e);
        await _db.SaveChangesAsync();
        TempData["Msg"] = "Student enrolled.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> SetGrade(int id, string? grade)
    {
        var e = await _db.Enrollments.FindAsync(id);
        if (e is not null) { e.Grade = grade; await _db.SaveChangesAsync(); }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var e = await _db.Enrollments.FindAsync(id);
        if (e is not null) { _db.Remove(e); await _db.SaveChangesAsync(); }
        TempData["Msg"] = "Enrollment removed.";
        return RedirectToAction(nameof(Index));
    }

    private async Task LoadListsAsync(int? courseId, int? studentId)
    {
        ViewBag.Courses = new SelectList(await _db.Courses.OrderBy(c => c.Title).ToListAsync(), "Id", "Title", courseId);
        ViewBag.Students = new SelectList(await _db.Students.OrderBy(s => s.FullName).ToListAsync(), "Id", "FullName", studentId);
    }
}
