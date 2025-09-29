using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FatimaSchoolManagement.Data;
using FatimaSchoolManagement.Models;
using FatimaSchoolManagement.Models.ViewModels;

namespace FatimaSchoolManagement.Controllers
{
    public class MarksController : Controller
    {
        private readonly SchoolDbContext _context;

        public MarksController(SchoolDbContext context)
        {
            _context = context;
        }

        // GET: Marks
        public async Task<IActionResult> Index()
        {
            var marks = await _context.Marks
                .Include(m => m.Student)
                .Include(m => m.Subject)
                .Include(m => m.Class)
                .OrderByDescending(m => m.LastModified)
                .Take(50)
                .ToListAsync();

            return View(marks);
        }

        // GET: Marks/ClassEntry
        public async Task<IActionResult> ClassEntry()
        {
            var classes = await _context.Classes.Where(c => c.IsActive).OrderBy(c => c.ClassName).ToListAsync();
            var subjects = await _context.Subjects.Where(s => s.IsActive).OrderBy(s => s.SubjectName).ToListAsync();

            var viewModel = new ClassMarkEntryViewModel
            {
                Classes = classes,
                Subjects = subjects,
                AcademicYear = DateTime.Now.Year,
                Term = Term.Term1
            };

            return View(viewModel);
        }

        // POST: Marks/LoadClassStudents
        [HttpPost]
        public async Task<IActionResult> LoadClassStudents(int classId, int subjectId, int academicYear, Term term)
        {
            var students = await _context.Students
                .Where(s => s.ClassId == classId)
                .OrderBy(s => s.FullName)
                .ToListAsync();

            var existingMarks = await _context.Marks
                .Where(m => m.ClassId == classId && m.SubjectId == subjectId && m.AcademicYear == academicYear && m.Term == term)
                .ToListAsync();

            var studentMarks = students.Select(s =>
            {
                var existingMark = existingMarks.FirstOrDefault(m => m.StudentId == s.StudentId);
                return new StudentMarkEntry
                {
                    StudentId = s.StudentId,
                    StudentName = s.FullName,
                    StudentNumber = s.StudentNumber,
                    BOTMark = existingMark?.BOTMark,
                    MOTMark = existingMark?.MOTMark,
                    EOTMark = existingMark?.EOTMark,
                    HasExistingRecord = existingMark != null,
                    MarkId = existingMark?.MarkId
                };
            }).ToList();

            // Calculate final marks and grades
            foreach (var sm in studentMarks)
            {
                if (sm.BOTMark.HasValue && sm.MOTMark.HasValue && sm.EOTMark.HasValue)
                {
                    sm.FinalMark = Math.Round((sm.BOTMark.Value * 0.1m) + (sm.MOTMark.Value * 0.2m) + (sm.EOTMark.Value * 0.7m), 2);
                    sm.Grade = GetGrade(sm.FinalMark);
                }
            }

            var classes = await _context.Classes.Where(c => c.IsActive).OrderBy(c => c.ClassName).ToListAsync();
            var subjects = await _context.Subjects.Where(s => s.IsActive).OrderBy(s => s.SubjectName).ToListAsync();
            var selectedClass = await _context.Classes.FindAsync(classId);
            var selectedSubject = await _context.Subjects.FindAsync(subjectId);

            var viewModel = new ClassMarkEntryViewModel
            {
                ClassId = classId,
                SubjectId = subjectId,
                AcademicYear = academicYear,
                Term = term,
                StudentMarks = studentMarks,
                Classes = classes,
                Subjects = subjects,
                ClassName = selectedClass?.ClassName ?? "",
                SubjectName = selectedSubject?.SubjectName ?? ""
            };

            return View("ClassEntry", viewModel);
        }

        // POST: Marks/SaveClassMarks
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveClassMarks(ClassMarkEntryViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var studentMark in viewModel.StudentMarks)
                    {
                        var existingMark = await _context.Marks
                            .FirstOrDefaultAsync(m => m.StudentId == studentMark.StudentId 
                                                   && m.SubjectId == viewModel.SubjectId 
                                                   && m.AcademicYear == viewModel.AcademicYear 
                                                   && m.Term == viewModel.Term);

                        if (existingMark != null)
                        {
                            // Update existing mark
                            existingMark.BOTMark = studentMark.BOTMark;
                            existingMark.MOTMark = studentMark.MOTMark;
                            existingMark.EOTMark = studentMark.EOTMark;
                            existingMark.LastModified = DateTime.Now;
                            _context.Update(existingMark);
                        }
                        else if (studentMark.BOTMark.HasValue || studentMark.MOTMark.HasValue || studentMark.EOTMark.HasValue)
                        {
                            // Create new mark record
                            var newMark = new Mark
                            {
                                StudentId = studentMark.StudentId,
                                SubjectId = viewModel.SubjectId,
                                ClassId = viewModel.ClassId,
                                AcademicYear = viewModel.AcademicYear,
                                Term = viewModel.Term,
                                BOTMark = studentMark.BOTMark,
                                MOTMark = studentMark.MOTMark,
                                EOTMark = studentMark.EOTMark,
                                CreatedDate = DateTime.Now,
                                LastModified = DateTime.Now
                            };
                            _context.Add(newMark);
                        }
                    }

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Marks saved successfully!";
                    return RedirectToAction(nameof(ClassEntry));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Error saving marks: " + ex.Message;
                }
            }

            // Reload the form data
            viewModel.Classes = await _context.Classes.Where(c => c.IsActive).OrderBy(c => c.ClassName).ToListAsync();
            viewModel.Subjects = await _context.Subjects.Where(s => s.IsActive).OrderBy(s => s.SubjectName).ToListAsync();
            return View("ClassEntry", viewModel);
        }

        // GET: Marks/StudentReport/5
        public async Task<IActionResult> StudentReport(int studentId, int academicYear, Term term)
        {
            var student = await _context.Students
                .Include(s => s.Class)
                .FirstOrDefaultAsync(s => s.StudentId == studentId);

            if (student == null)
            {
                return NotFound();
            }

            var marks = await _context.Marks
                .Include(m => m.Subject)
                .Where(m => m.StudentId == studentId && m.AcademicYear == academicYear && m.Term == term)
                .OrderBy(m => m.Subject.SubjectName)
                .ToListAsync();

            var subjectMarks = marks.Select(m => new SubjectMarkDetail
            {
                SubjectCode = m.Subject.SubjectCode,
                SubjectName = m.Subject.SubjectName,
                BOTMark = m.BOTMark,
                MOTMark = m.MOTMark,
                EOTMark = m.EOTMark,
                FinalMark = m.FinalMark,
                Grade = m.Grade,
                GradeDescription = m.GradeDescription
            }).ToList();

            var completeMarks = marks.Where(m => m.BOTMark.HasValue && m.MOTMark.HasValue && m.EOTMark.HasValue).ToList();
            var totalMarks = completeMarks.Sum(m => m.FinalMark);
            var averageMark = completeMarks.Any() ? completeMarks.Average(m => m.FinalMark) : 0;

            // Calculate class position
            var classStudents = await _context.Students.Where(s => s.ClassId == student.ClassId).Select(s => s.StudentId).ToListAsync();
            
            // Get all marks for class students (fetch to client first)
            var classMarksRaw = await _context.Marks
                .Where(m => classStudents.Contains(m.StudentId) && m.AcademicYear == academicYear && m.Term == term 
                           && m.BOTMark.HasValue && m.MOTMark.HasValue && m.EOTMark.HasValue)
                .ToListAsync();
            
            // Calculate averages on client side
            var classAverages = classMarksRaw
                .GroupBy(m => m.StudentId)
                .Select(g => new { 
                    StudentId = g.Key, 
                    Average = g.Average(m => (m.BOTMark!.Value * 0.1m) + (m.MOTMark!.Value * 0.2m) + (m.EOTMark!.Value * 0.7m))
                })
                .OrderByDescending(x => x.Average)
                .ToList();

            var position = classAverages.FindIndex(x => x.StudentId == studentId) + 1;

            var viewModel = new StudentReportViewModel
            {
                Student = student,
                Class = student.Class,
                AcademicYear = academicYear,
                Term = term,
                SubjectMarks = subjectMarks,
                TotalMarks = totalMarks,
                AverageMark = averageMark,
                OverallGrade = GetGrade(averageMark),
                ClassPosition = position > 0 ? position : null,
                TotalStudents = classAverages.Count
            };

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

        private bool MarkExists(int id)
        {
            return _context.Marks.Any(e => e.MarkId == id);
        }
    }
}