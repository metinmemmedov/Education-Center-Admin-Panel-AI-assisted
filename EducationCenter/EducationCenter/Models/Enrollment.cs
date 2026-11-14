using System.ComponentModel.DataAnnotations;

namespace EducationCenter.Models;

public class Enrollment
{
    public int Id { get; set; }

    public int CourseId { get; set; }
    public Course? Course { get; set; }

    public int StudentId { get; set; }
    public Student? Student { get; set; }

    [DataType(DataType.Date)]
    public DateTime EnrolledOn { get; set; } = DateTime.Today;

    [StringLength(5)]
    public string? Grade { get; set; }
}
