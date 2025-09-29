using System.ComponentModel.DataAnnotations;

namespace FatimaSchoolManagement.Models
{
    public class Class
    {
        [Key]
        public int ClassId { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "Class Name")]
        public string ClassName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Level")]
        public EducationLevel Level { get; set; }

        [StringLength(500)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Academic Year")]
        public int AcademicYear { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
        public virtual ICollection<Mark> Marks { get; set; } = new List<Mark>();

        [Display(Name = "Student Count")]
        public int StudentCount => Students?.Count ?? 0;

        [Display(Name = "Level Description")]
        public string LevelDescription => Level == EducationLevel.OLevel ? "O-Level" : "A-Level";
    }
}