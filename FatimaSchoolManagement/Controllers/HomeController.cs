using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FatimaSchoolManagement.Data;
using FatimaSchoolManagement.Models;
using FatimaSchoolManagement.Models.ViewModels;

namespace FatimaSchoolManagement.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly SchoolDbContext _context;

    public HomeController(ILogger<HomeController> logger, SchoolDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var viewModel = new DashboardViewModel();

        // Get basic statistics
        viewModel.TotalStudents = await _context.Students.CountAsync();
        viewModel.TotalClasses = await _context.Classes.Where(c => c.IsActive).CountAsync();
        viewModel.TotalSubjects = await _context.Subjects.Where(s => s.IsActive).CountAsync();
        viewModel.OLevelStudents = await _context.Students.Where(s => s.Level == EducationLevel.OLevel).CountAsync();
        viewModel.ALevelStudents = await _context.Students.Where(s => s.Level == EducationLevel.ALevel).CountAsync();

        // Get recent students (last 5 added)
        viewModel.RecentStudents = await _context.Students
            .Include(s => s.Class)
            .OrderByDescending(s => s.StudentId)
            .Take(5)
            .ToListAsync();

        // Calculate average performance for students with complete marks
        var marksWithAllValues = await _context.Marks
            .Where(m => m.BOTMark.HasValue && m.MOTMark.HasValue && m.EOTMark.HasValue)
            .ToListAsync();

        if (marksWithAllValues.Any())
        {
            viewModel.AveragePerformance = marksWithAllValues.Average(m => m.FinalMark);
            viewModel.StudentsWithCompleteRecords = marksWithAllValues.Select(m => m.StudentId).Distinct().Count();

            // Grade distribution
            var grades = marksWithAllValues.GroupBy(m => m.Grade)
                .ToDictionary(g => g.Key, g => g.Count());

            viewModel.GradeDistribution = new Dictionary<string, int>
            {
                { "A", grades.GetValueOrDefault("A", 0) },
                { "B", grades.GetValueOrDefault("B", 0) },
                { "C", grades.GetValueOrDefault("C", 0) },
                { "D", grades.GetValueOrDefault("D", 0) },
                { "E", grades.GetValueOrDefault("E", 0) }
            };

            // Top performers (top 5 students by average)
            var studentAverages = marksWithAllValues
                .GroupBy(m => m.StudentId)
                .Select(g => new
                {
                    StudentId = g.Key,
                    AverageMark = g.Average(m => m.FinalMark)
                })
                .OrderByDescending(s => s.AverageMark)
                .Take(5);

            viewModel.TopPerformers = new List<StudentPerformance>();
            foreach (var avg in studentAverages)
            {
                var student = await _context.Students
                    .Include(s => s.Class)
                    .FirstOrDefaultAsync(s => s.StudentId == avg.StudentId);
                
                if (student != null)
                {
                    var grade = GetGrade(avg.AverageMark);
                    viewModel.TopPerformers.Add(new StudentPerformance
                    {
                        StudentId = student.StudentId,
                        StudentName = student.FullName,
                        ClassName = student.Class.ClassName,
                        AverageMark = avg.AverageMark,
                        Grade = grade,
                        PhotoPath = student.PhotoPath ?? ""
                    });
                }
            }
        }

        // Class performance
        var classMarks = await _context.Marks
            .Include(m => m.Class)
            .Where(m => m.BOTMark.HasValue && m.MOTMark.HasValue && m.EOTMark.HasValue)
            .GroupBy(m => m.ClassId)
            .Select(g => new
            {
                ClassId = g.Key,
                AveragePerformance = g.Average(m => (m.BOTMark!.Value * 0.1m) + (m.MOTMark!.Value * 0.2m) + (m.EOTMark!.Value * 0.7m)),
                StudentCount = g.Select(m => m.StudentId).Distinct().Count()
            })
            .ToListAsync();

        viewModel.ClassPerformances = new List<ClassPerformance>();
        foreach (var classPerf in classMarks)
        {
            var classEntity = await _context.Classes.FindAsync(classPerf.ClassId);
            if (classEntity != null)
            {
                viewModel.ClassPerformances.Add(new ClassPerformance
                {
                    ClassId = classEntity.ClassId,
                    ClassName = classEntity.ClassName,
                    StudentCount = classPerf.StudentCount,
                    AveragePerformance = classPerf.AveragePerformance,
                    TopGrade = GetGrade(classPerf.AveragePerformance),
                    Level = classEntity.Level
                });
            }
        }

        return View(viewModel);
    }

    private string GetGrade(decimal mark)
    {
        if (mark >= 80) return "A";
        if (mark >= 70) return "B";
        if (mark >= 60) return "C";
        if (mark >= 50) return "D";
        return "E";
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
