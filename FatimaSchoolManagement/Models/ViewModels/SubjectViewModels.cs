using System.ComponentModel.DataAnnotations;
using FatimaSchoolManagement.Models;

namespace FatimaSchoolManagement.Models.ViewModels
{
    public class SubjectIndexViewModel
    {
        public List<Subject> Subjects { get; set; } = new List<Subject>();
        public EducationLevel? SelectedLevel { get; set; }
        public string? SearchTerm { get; set; }
        public int TotalOLevelSubjects { get; set; }
        public int TotalALevelSubjects { get; set; }
    }

    public class SubjectCreateEditViewModel
    {
        public Subject Subject { get; set; } = new Subject();
        public IEnumerable<EducationLevel> AvailableLevels { get; set; } = new List<EducationLevel>();
    }

    public class SubjectDeleteViewModel
    {
        public Subject Subject { get; set; } = new Subject();
        public bool HasMarks { get; set; }
        public int StudentsAffected { get; set; }
        public bool HasRelatedMarks { get; set; }
        public int TotalMarks { get; set; }
    }

    public class SubjectStatisticsViewModel
    {
        public Subject Subject { get; set; } = new Subject();
        public int TotalStudentsEnrolled { get; set; }
        public double AverageGrade { get; set; }
        public List<Mark> RecentMarks { get; set; } = new List<Mark>();
    }

    public class SubjectSelectionViewModel
    {
        public int SubjectId { get; set; }
        public string SubjectCode { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public EducationLevel Level { get; set; }
        public string DisplayText => $"{SubjectCode} - {SubjectName}";
    }

    public class StudentSubjectFilterViewModel
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public EducationLevel StudentLevel { get; set; }
        public List<SubjectSelectionViewModel> AvailableSubjects { get; set; } = new List<SubjectSelectionViewModel>();
    }
}