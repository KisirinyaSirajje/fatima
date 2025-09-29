using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FatimaSchoolManagement.Models
{
    public class Mark
    {
        [Key]
        public int MarkId { get; set; }

        // Foreign Keys
        [Required]
        [Display(Name = "Student")]
        public int StudentId { get; set; }

        [Required]
        [Display(Name = "Subject")]
        public int SubjectId { get; set; }

        [Required]
        [Display(Name = "Class")]
        public int ClassId { get; set; }

        [Required]
        [Display(Name = "Academic Year")]
        public int AcademicYear { get; set; }

        [Required]
        [Display(Name = "Term")]
        public Term Term { get; set; }

        // Assessment Marks
        [Range(0, 100)]
        [Display(Name = "BOT Mark (10%)")]
        public decimal? BOTMark { get; set; }

        [Range(0, 100)]
        [Display(Name = "MOT Mark (20%)")]
        public decimal? MOTMark { get; set; }

        [Range(0, 100)]
        [Display(Name = "EOT Mark (70%)")]
        public decimal? EOTMark { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Last Modified")]
        public DateTime LastModified { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; } = null!;

        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; } = null!;

        [ForeignKey("ClassId")]
        public virtual Class Class { get; set; } = null!;

        // Calculated Properties
        [Display(Name = "Final Mark")]
        public decimal FinalMark
        {
            get
            {
                if (!BOTMark.HasValue || !MOTMark.HasValue || !EOTMark.HasValue)
                    return 0;

                return Math.Round((BOTMark.Value * 0.1m) + (MOTMark.Value * 0.2m) + (EOTMark.Value * 0.7m), 2);
            }
        }

        [Display(Name = "Grade")]
        public string Grade
        {
            get
            {
                var finalMark = FinalMark;
                if (finalMark >= 80) return "A";
                if (finalMark >= 70) return "B";
                if (finalMark >= 60) return "C";
                if (finalMark >= 50) return "D";
                return "E";
            }
        }

        [Display(Name = "Grade Description")]
        public string GradeDescription
        {
            get
            {
                return Grade switch
                {
                    "A" => "Exceptional Achievement",
                    "B" => "Outstanding Performance",
                    "C" => "Satisfactory Performance",
                    "D" => "Basic Understanding",
                    "E" => "Elementary Understanding",
                    _ => "Not Graded"
                };
            }
        }

        [Display(Name = "Has All Marks")]
        public bool HasAllMarks => BOTMark.HasValue && MOTMark.HasValue && EOTMark.HasValue;

        [Display(Name = "Completion Percentage")]
        public int CompletionPercentage
        {
            get
            {
                int count = 0;
                if (BOTMark.HasValue) count++;
                if (MOTMark.HasValue) count++;
                if (EOTMark.HasValue) count++;
                return (count * 100) / 3;
            }
        }
    }

    public enum Term
    {
        [Display(Name = "Term 1")]
        Term1 = 1,
        [Display(Name = "Term 2")]
        Term2 = 2,
        [Display(Name = "Term 3")]
        Term3 = 3
    }
}