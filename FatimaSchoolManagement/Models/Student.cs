using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FatimaSchoolManagement.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Student Number")]
        public string StudentNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [StringLength(50)]
        [Display(Name = "LIN Number")]
        public string? LINNumber { get; set; }

        [StringLength(15)]
        [Display(Name = "Parents' Contact")]
        [Phone]
        public string? ParentsContact { get; set; }

        [StringLength(100)]
        [Display(Name = "Next of Kin")]
        public string? NextOfKin { get; set; }

        [StringLength(15)]
        [Display(Name = "Next of Kin Contact")]
        [Phone]
        public string? NextOfKinContact { get; set; }

        [StringLength(255)]
        [Display(Name = "Photo Path")]
        public string? PhotoPath { get; set; }

        [Required]
        [Display(Name = "Level")]
        public EducationLevel Level { get; set; }

        // Foreign Key
        [Required]
        [Display(Name = "Class")]
        public int ClassId { get; set; }

        // Navigation Properties
        [ForeignKey("ClassId")]
        public virtual Class Class { get; set; } = null!;

        public virtual ICollection<Mark> Marks { get; set; } = new List<Mark>();

        [Display(Name = "Age")]
        public int Age => DateTime.Now.Year - DateOfBirth.Year;

        [Display(Name = "Class Name")]
        public string ClassName => Class?.ClassName ?? string.Empty;
    }

    public enum EducationLevel
    {
        [Display(Name = "O-Level")]
        OLevel = 1,
        [Display(Name = "A-Level")]
        ALevel = 2
    }
}