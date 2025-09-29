using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace FatimaSchoolManagement.Models.ViewModels
{
    public class StudentCreateViewModel
    {
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

        [Display(Name = "Photo")]
        public IFormFile? Photo { get; set; }

        [Required]
        [Display(Name = "Level")]
        public EducationLevel Level { get; set; }

        [Required]
        [Display(Name = "Class")]
        public int ClassId { get; set; }

        public List<Class> AvailableClasses { get; set; } = new List<Class>();
    }

    public class StudentEditViewModel : StudentCreateViewModel
    {
        public int StudentId { get; set; }
        public string? CurrentPhotoPath { get; set; }
    }

    public class StudentListViewModel
    {
        public List<Student> Students { get; set; } = new List<Student>();
        public List<Class> Classes { get; set; } = new List<Class>();
        public string? SearchTerm { get; set; }
        public int? SelectedClassId { get; set; }
        public EducationLevel? SelectedLevel { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int TotalStudents { get; set; }
        public int PageSize { get; set; } = 20;
    }
}