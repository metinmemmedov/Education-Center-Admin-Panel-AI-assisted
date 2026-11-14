using System.ComponentModel.DataAnnotations;

namespace EducationCenter.Models;

public class Student
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Full name is required"), StringLength(100), Display(Name = "Full name")]
    public string FullName { get; set; } = "";

    [Required(ErrorMessage = "Email is required"), EmailAddress(ErrorMessage = "Enter a valid email")]
    public string Email { get; set; } = "";

    [Phone]
    public string? Phone { get; set; }

    [DataType(DataType.Date), Display(Name = "Date of birth")]
    public DateTime? DateOfBirth { get; set; }

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
