using System.ComponentModel.DataAnnotations;

namespace FatimaSchoolManagement.Models.ViewModels
{
    public class MarkEntryViewModel
    {
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

        [Range(0, 100)]
        [Display(Name = "BOT Mark (10%)")]
        public decimal? BOTMark { get; set; }

        [Range(0, 100)]
        [Display(Name = "MOT Mark (20%)")]
        public decimal? MOTMark { get; set; }

        [Range(0, 100)]
        [Display(Name = "EOT Mark (70%)")]
        public decimal? EOTMark { get; set; }

        public List<Student> Students { get; set; } = new List<Student>();
        public List<Subject> Subjects { get; set; } = new List<Subject>();
        public List<Class> Classes { get; set; } = new List<Class>();
        
        public string StudentName { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
    }

    public class ClassMarkEntryViewModel
    {
        [Required]
        [Display(Name = "Class")]
        public int ClassId { get; set; }

        [Required]
        [Display(Name = "Subject")]
        public int SubjectId { get; set; }

        [Required]
        [Display(Name = "Academic Year")]
        public int AcademicYear { get; set; }

        [Required]
        [Display(Name = "Term")]
        public Term Term { get; set; }

        public List<StudentMarkEntry> StudentMarks { get; set; } = new List<StudentMarkEntry>();
        public List<Class> Classes { get; set; } = new List<Class>();
        public List<Subject> Subjects { get; set; } = new List<Subject>();
        
        public string ClassName { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
    }

    public class StudentMarkEntry
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string StudentNumber { get; set; } = string.Empty;

        [Range(0, 100)]
        [Display(Name = "BOT (10%)")]
        public decimal? BOTMark { get; set; }

        [Range(0, 100)]
        [Display(Name = "MOT (20%)")]
        public decimal? MOTMark { get; set; }

        [Range(0, 100)]
        [Display(Name = "EOT (70%)")]
        public decimal? EOTMark { get; set; }

        public decimal FinalMark { get; set; }
        public string Grade { get; set; } = string.Empty;
        public bool HasExistingRecord { get; set; }
        public int? MarkId { get; set; }
    }

    public class StudentReportViewModel
    {
        public Student Student { get; set; } = null!;
        public Class Class { get; set; } = null!;
        public int AcademicYear { get; set; }
        public Term Term { get; set; }
        public List<SubjectMarkDetail> SubjectMarks { get; set; } = new List<SubjectMarkDetail>();
        public decimal TotalMarks { get; set; }
        public decimal AverageMark { get; set; }
        public string OverallGrade { get; set; } = string.Empty;
        public int? ClassPosition { get; set; }
        public int TotalStudents { get; set; }
        public string? TeacherComments { get; set; }
        public string? HeadTeacherComments { get; set; }
        public DateTime? NextTermBegins { get; set; }
    }

    public class SubjectMarkDetail
    {
        public string SubjectCode { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public decimal? BOTMark { get; set; }
        public decimal? MOTMark { get; set; }
        public decimal? EOTMark { get; set; }
        public decimal FinalMark { get; set; }
        public string Grade { get; set; } = string.Empty;
        public string GradeDescription { get; set; } = string.Empty;

        // New properties for OLevel grading
        public decimal OLevelPoints { get; set; }
        public string OLevelGrade { get; set; } = string.Empty;
    }

    public class ClassMarkSheetViewModel
    {
        public Class Class { get; set; } = null!;
        public int AcademicYear { get; set; }
        public Term Term { get; set; }
        public List<Subject> Subjects { get; set; } = new List<Subject>();
        public List<StudentMarkSheetRow> StudentRows { get; set; } = new List<StudentMarkSheetRow>();
        public Dictionary<string, decimal> ClassAverages { get; set; } = new Dictionary<string, decimal>();
    }

    public class StudentMarkSheetRow
    {
        public Student Student { get; set; } = null!;
        public Dictionary<int, StudentSubjectMark> SubjectMarks { get; set; } = new Dictionary<int, StudentSubjectMark>();
        public decimal OverallAverage { get; set; }
        public string OverallGrade { get; set; } = string.Empty;

        // New properties for OLevel grading
        public decimal OverallOLevelPoints { get; set; }
        public string OverallOLevelGrade { get; set; } = string.Empty;

        public int Position { get; set; }
    }

    public class StudentSubjectMark
    {
        public decimal? BOTMark { get; set; }
        public decimal? MOTMark { get; set; }
        public decimal? EOTMark { get; set; }
        public decimal FinalMark { get; set; }
        public string Grade { get; set; } = string.Empty;

        // New properties for OLevel grading
        public decimal OLevelPoints { get; set; }
        public string OLevelGrade { get; set; } = string.Empty;

        public bool IsComplete => BOTMark.HasValue && MOTMark.HasValue && EOTMark.HasValue;
    }

    public class SubsMarkSheetViewModel
    {
        public Class Class { get; set; } = null!;
        public int AcademicYear { get; set; }
        public Term Term { get; set; }
        public string MarkType { get; set; } = string.Empty;
        public List<Subject> Subjects { get; set; } = new List<Subject>();
        public List<StudentSubsRow> StudentRows { get; set; } = new List<StudentSubsRow>();
    }

    public class StudentSubsRow
    {
        public Student Student { get; set; } = null!;
        public Dictionary<int, decimal?> SubjectMarks { get; set; } = new Dictionary<int, decimal?>();
    }
}
