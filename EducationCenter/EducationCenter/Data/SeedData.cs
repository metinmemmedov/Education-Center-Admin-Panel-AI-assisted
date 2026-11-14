using EducationCenter.Models;
using Microsoft.EntityFrameworkCore;

namespace EducationCenter.Data;

public static class SeedData
{
    public static async Task InitializeAsync(AppDbContext db)
    {
        await db.Database.EnsureCreatedAsync();

        if (await db.Courses.AnyAsync()) return;

        static Course C(string title, string cat, string level, string instr, decimal price, int cap, int days, string desc, string status = "Open") =>
            new() { Title = title, Category = cat, Level = level, Instructor = instr, Price = price, Capacity = cap,
                    StartDate = DateTime.Today.AddDays(days), Description = desc, Status = status };

        db.Courses.AddRange(
            C("English for Beginners", "Languages", "Beginner", "Leyla Aliyeva", 120, 15, 10, "Build a solid base in grammar, vocabulary and everyday speaking."),
            C("IELTS Preparation", "Languages", "Advanced", "Sevinj Karimova", 220, 12, 18, "Intensive practice for all four IELTS modules with mock exams."),
            C("Turkish Conversation Club", "Languages", "Intermediate", "Elchin Guliyev", 90, 14, 6, "Speak confidently with weekly themed conversation sessions."),
            C("Web Development with ASP.NET", "Technology", "Intermediate", "Rashad Mammadov", 300, 12, 14, "Build real web apps with C#, ASP.NET Core and Entity Framework."),
            C("Python for Everyone", "Technology", "Beginner", "Aysel Rzayeva", 180, 20, 8, "Learn programming from scratch by building small useful projects."),
            C("Data Analysis with Excel & SQL", "Technology", "Intermediate", "Orkhan Hasanov", 210, 16, 25, "Clean, query and visualise data to answer real business questions."),
            C("Mobile App Design (UI/UX)", "Arts & Design", "Beginner", "Gunel Ismayilova", 190, 14, 12, "Design beautiful, usable app screens in Figma."),
            C("Digital Illustration", "Arts & Design", "Beginner", "Nigar Hasanova", 140, 10, 30, "From sketches to polished digital artwork using modern tools."),
            C("Mathematics Exam Prep", "Science & Math", "Intermediate", "Farid Abbasov", 150, 20, 4, "Algebra, geometry and problem solving for university entrance exams."),
            C("Physics Made Simple", "Science & Math", "Beginner", "Tural Jafarov", 130, 18, 21, "Understand mechanics and electricity with hands-on experiments."),
            C("Entrepreneurship Bootcamp", "Business", "Intermediate", "Kamran Salimov", 260, 25, 16, "Validate your idea, build a business model and pitch to investors."),
            C("Digital Marketing Essentials", "Business", "Beginner", "Lala Mehdiyeva", 170, 22, 9, "Social media, SEO and ads: grow an audience and measure results."),
            C("Guitar for Beginners", "Music", "Beginner", "Vusal Najafov", 110, 8, 5, "Chords, rhythm and your first songs in just eight weeks."),
            C("Piano: Level 1", "Music", "Beginner", "Aynur Quliyeva", 160, 8, -20, "Reading music and playing with both hands. Course in progress.", "In Progress"),
            C("Photography Basics", "Arts & Design", "Beginner", "Gunel Ismayilova", 125, 12, -70, "Camera settings, light and composition. Course finished.", "Completed"));
        await db.SaveChangesAsync();

        string[] names =
        {
            "Aydan Mammadova","Murad Aliyev","Sabina Huseynova","Elnur Gasimov","Narmin Bayramova","Ramin Safarov",
            "Leyla Hajiyeva","Tahir Mustafayev","Zeynab Ahmadova","Orxan Valiyev","Samira Aslanova","Cavid Rahimov",
            "Fidan Ibrahimova","Nurlan Karimov","Gulnar Sadigova","Anar Mirzayev","Ulviyya Jabbarova","Elvin Hasanli",
            "Konul Rustamova","Emin Zeynalov","Nigar Aghayeva","Rauf Babayev","Aytan Shahbazova","Vugar Qasimov",
            "Mehriban Alizada","Ilkin Namazov","Sevda Orujova","Kenan Heydarov","Lamiya Taghiyeva","Parviz Əliyev",
            "Arzu Mikayilova","Sanan Isayev"
        };

        var students = names.Select((n, i) => new Student
        {
            FullName = n.Replace("Ə", "A"),
            Email = n.Replace("Ə", "A").ToLower().Replace(" ", ".") + "@example.com",
            Phone = $"+994 50 {100 + i * 7:000} {10 + i:00} {20 + i * 2:00}",
            DateOfBirth = new DateTime(1990 + i % 14, 1 + i % 12, 1 + (i * 3) % 27)
        }).ToList();
        db.Students.AddRange(students);
        await db.SaveChangesAsync();

        var rnd = new Random(7);
        var allCourses = await db.Courses.ToListAsync();
        var counts = allCourses.ToDictionary(c => c.Id, _ => 0);
        string[] grades = { "A", "A-", "B+", "B", "B-", "C+" };

        foreach (var s in students)
        {
            foreach (var c in allCourses.OrderBy(_ => rnd.Next()).Take(rnd.Next(1, 5)))
            {
                if (counts[c.Id] >= c.Capacity) continue;
                counts[c.Id]++;
                db.Enrollments.Add(new Enrollment
                {
                    CourseId = c.Id,
                    StudentId = s.Id,
                    EnrolledOn = DateTime.Today.AddDays(-rnd.Next(1, 60)),
                    Grade = c.StartDate < DateTime.Today ? grades[rnd.Next(grades.Length)] : null
                });
            }
        }
        await db.SaveChangesAsync();
    }
}
