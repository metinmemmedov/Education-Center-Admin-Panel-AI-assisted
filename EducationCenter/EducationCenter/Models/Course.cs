using System.ComponentModel.DataAnnotations;

namespace EducationCenter.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Course title is required")]
        [StringLength(100)]
        public string Title { get; set; } = "";

        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000)]
        public string Description { get; set; } = "";

        [Required(ErrorMessage = "Instructor name is required")]
        [StringLength(80)]
        public string Instructor { get; set; } = "";

        [Required(ErrorMessage = "Please choose a category")]
        [StringLength(40)]
        public string Category { get; set; } = "";

        [Required]
        [StringLength(20)]
        public string Level { get; set; } = "Beginner";

        [DataType(DataType.Currency)]
        [Range(0, 100000, ErrorMessage = "Price must be between 0 and 100000")]
        public decimal Price { get; set; }

        [Range(1, 500, ErrorMessage = "Capacity must be between 1 and 500")]
        public int Capacity { get; set; } = 20;

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } = DateTime.Today.AddDays(7);

        public string Status { get; set; } = "Open";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
