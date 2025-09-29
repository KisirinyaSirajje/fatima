using System.ComponentModel.DataAnnotations;

namespace FatimaSchoolManagement.Models.ViewModels
{
    public class DashboardViewModel
    {
        [Display(Name = "Total Students")]
        public int TotalStudents { get; set; }

        [Display(Name = "Total Classes")]
        public int TotalClasses { get; set; }

        [Display(Name = "Total Subjects")]
        public int TotalSubjects { get; set; }

        [Display(Name = "O-Level Students")]
        public int OLevelStudents { get; set; }

        [Display(Name = "A-Level Students")]
        public int ALevelStudents { get; set; }

        [Display(Name = "Average Performance")]
        public decimal AveragePerformance { get; set; }

        [Display(Name = "Students with Complete Records")]
        public int StudentsWithCompleteRecords { get; set; }

        [Display(Name = "Recent Students")]
        public List<Student> RecentStudents { get; set; } = new List<Student>();

        [Display(Name = "Top Performing Students")]
        public List<StudentPerformance> TopPerformers { get; set; } = new List<StudentPerformance>();

        [Display(Name = "Grade Distribution")]
        public Dictionary<string, int> GradeDistribution { get; set; } = new Dictionary<string, int>();

        [Display(Name = "Class Performance")]
        public List<ClassPerformance> ClassPerformances { get; set; } = new List<ClassPerformance>();
    }

    public class StudentPerformance
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public decimal AverageMark { get; set; }
        public string Grade { get; set; } = string.Empty;
        public string PhotoPath { get; set; } = string.Empty;
    }

    public class ClassPerformance
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public int StudentCount { get; set; }
        public decimal AveragePerformance { get; set; }
        public string TopGrade { get; set; } = string.Empty;
        public EducationLevel Level { get; set; }
    }
}