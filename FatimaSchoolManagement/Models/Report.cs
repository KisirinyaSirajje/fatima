using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FatimaSchoolManagement.Models
{
    public class Report
    {
        [Key]
        public int ReportId { get; set; }

        // Foreign Keys
        [Required]
        [Display(Name = "Student")]
        public int StudentId { get; set; }

        [Required]
        [Display(Name = "Class")]
        public int ClassId { get; set; }

        [Required]
        [Display(Name = "Academic Year")]
        public int AcademicYear { get; set; }

        [Required]
        [Display(Name = "Term")]
        public Term Term { get; set; }

        [Display(Name = "Generated Date")]
        public DateTime GeneratedDate { get; set; } = DateTime.Now;

        [Display(Name = "Total Marks")]
        public decimal TotalMarks { get; set; }

        [Display(Name = "Average Mark")]
        public decimal AverageMark { get; set; }

        [Display(Name = "Overall Grade")]
        public string OverallGrade { get; set; } = string.Empty;

        [Display(Name = "Class Position")]
        public int? ClassPosition { get; set; }

        [Display(Name = "Total Students")]
        public int TotalStudents { get; set; }

        [StringLength(1000)]
        [Display(Name = "Teacher's Comments")]
        public string? TeacherComments { get; set; }

        [StringLength(1000)]
        [Display(Name = "Head Teacher's Comments")]
        public string? HeadTeacherComments { get; set; }

        [Display(Name = "Next Term Begins")]
        [DataType(DataType.Date)]
        public DateTime? NextTermBegins { get; set; }

        // Navigation Properties
        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; } = null!;

        [ForeignKey("ClassId")]
        public virtual Class Class { get; set; } = null!;

        [Display(Name = "Student Name")]
        public string StudentName => Student?.FullName ?? string.Empty;

        [Display(Name = "Class Name")]
        public string ClassName => Class?.ClassName ?? string.Empty;

        [Display(Name = "Term Description")]
        public string TermDescription => Term switch
        {
            Term.Term1 => "Term 1",
            Term.Term2 => "Term 2",
            Term.Term3 => "Term 3",
            _ => "Unknown"
        };
    }
}