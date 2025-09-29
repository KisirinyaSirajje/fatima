using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FatimaSchoolManagement.Models
{
    public class Subject
    {
        [Key]
        public int SubjectId { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "Subject Code")]
        public string SubjectCode { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Display(Name = "Subject Name")]
        public string SubjectName { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Level")]
        public EducationLevel Level { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public virtual ICollection<Mark> Marks { get; set; } = new List<Mark>();

        [Display(Name = "Level Description")]
        public string LevelDescription => Level == EducationLevel.OLevel ? "O-Level" : "A-Level";
    }
}