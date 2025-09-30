using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FatimaSchoolManagement.Data;
using FatimaSchoolManagement.Models;
using FatimaSchoolManagement.Models.ViewModels;

namespace FatimaSchoolManagement.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly SchoolDbContext _context;

        public SubjectsController(SchoolDbContext context)
        {
            _context = context;
        }

        // GET: Subjects
        public async Task<IActionResult> Index(EducationLevel? level = null, string searchTerm = null)
        {
            var subjectsQuery = _context.Subjects.AsQueryable();

            // Filter by level if specified
            if (level.HasValue)
            {
                subjectsQuery = subjectsQuery.Where(s => s.Level == level.Value);
            }

            // Filter by search term if provided
            if (!string.IsNullOrEmpty(searchTerm))
            {
                subjectsQuery = subjectsQuery.Where(s => 
                    s.SubjectName.Contains(searchTerm) || 
                    s.SubjectCode.Contains(searchTerm) ||
                    s.Description.Contains(searchTerm));
            }

            var subjects = await subjectsQuery
                .OrderBy(s => s.Level)
                .ThenBy(s => s.SubjectName)
                .ToListAsync();

            var viewModel = new SubjectIndexViewModel
            {
                Subjects = subjects,
                SelectedLevel = level,
                SearchTerm = searchTerm,
                TotalOLevelSubjects = await _context.Subjects.CountAsync(s => s.Level == EducationLevel.OLevel && s.IsActive),
                TotalALevelSubjects = await _context.Subjects.CountAsync(s => s.Level == EducationLevel.ALevel && s.IsActive)
            };

            return View(viewModel);
        }

        // GET: Subjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .Include(s => s.Marks)
                .FirstOrDefaultAsync(s => s.SubjectId == id);

            if (subject == null)
            {
                return NotFound();
            }

            // Get statistics for this subject
            var marksWithAllValues = await _context.Marks
                .Where(m => m.SubjectId == id && m.BOTMark.HasValue && m.MOTMark.HasValue && m.EOTMark.HasValue)
                .ToListAsync();

            var stats = new SubjectStatisticsViewModel
            {
                Subject = subject,
                TotalStudentsEnrolled = await _context.Marks
                    .Where(m => m.SubjectId == id)
                    .Select(m => m.StudentId)
                    .Distinct()
                    .CountAsync(),
                AverageGrade = marksWithAllValues.Any() 
                    ? marksWithAllValues.Average(m => (double)((m.BOTMark!.Value * 0.1m) + (m.MOTMark!.Value * 0.2m) + (m.EOTMark!.Value * 0.7m)))
                    : 0.0,
                RecentMarks = await _context.Marks
                    .Where(m => m.SubjectId == id)
                    .Include(m => m.Student)
                        .ThenInclude(s => s.Class)
                    .OrderByDescending(m => m.LastModified)
                    .Take(10)
                    .ToListAsync()
            };

            return View(stats);
        }

        // GET: Subjects/Create
        public IActionResult Create()
        {
            var viewModel = new SubjectCreateEditViewModel
            {
                Subject = new Subject(),
                AvailableLevels = Enum.GetValues<EducationLevel>()
            };
            return View(viewModel);
        }

        // POST: Subjects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubjectCreateEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Check if subject code already exists
                var existingSubject = await _context.Subjects
                    .FirstOrDefaultAsync(s => s.SubjectCode == viewModel.Subject.SubjectCode);
                
                if (existingSubject != null)
                {
                    ModelState.AddModelError("Subject.SubjectCode", "A subject with this code already exists.");
                    viewModel.AvailableLevels = Enum.GetValues<EducationLevel>();
                    return View(viewModel);
                }

                viewModel.Subject.IsActive = true;
                _context.Add(viewModel.Subject);
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = $"Subject '{viewModel.Subject.SubjectName}' has been created successfully.";
                return RedirectToAction(nameof(Index));
            }

            viewModel.AvailableLevels = Enum.GetValues<EducationLevel>();
            return View(viewModel);
        }

        // GET: Subjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }

            var viewModel = new SubjectCreateEditViewModel
            {
                Subject = subject,
                AvailableLevels = Enum.GetValues<EducationLevel>()
            };

            return View(viewModel);
        }

        // POST: Subjects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SubjectCreateEditViewModel viewModel)
        {
            if (id != viewModel.Subject.SubjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Check if subject code already exists (excluding current subject)
                    var existingSubject = await _context.Subjects
                        .FirstOrDefaultAsync(s => s.SubjectCode == viewModel.Subject.SubjectCode && s.SubjectId != id);
                    
                    if (existingSubject != null)
                    {
                        ModelState.AddModelError("Subject.SubjectCode", "A subject with this code already exists.");
                        viewModel.AvailableLevels = Enum.GetValues<EducationLevel>();
                        return View(viewModel);
                    }

                    _context.Update(viewModel.Subject);
                    await _context.SaveChangesAsync();
                    
                    TempData["SuccessMessage"] = $"Subject '{viewModel.Subject.SubjectName}' has been updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubjectExists(viewModel.Subject.SubjectId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            viewModel.AvailableLevels = Enum.GetValues<EducationLevel>();
            return View(viewModel);
        }

        // GET: Subjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .Include(s => s.Marks)
                .FirstOrDefaultAsync(m => m.SubjectId == id);

            if (subject == null)
            {
                return NotFound();
            }

            var viewModel = new SubjectDeleteViewModel
            {
                Subject = subject,
                HasMarks = subject.Marks.Any(),
                StudentsAffected = subject.Marks.Select(m => m.StudentId).Distinct().Count(),
                HasRelatedMarks = subject.Marks.Any(),
                TotalMarks = subject.Marks.Count()
            };

            return View(viewModel);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subject = await _context.Subjects
                .Include(s => s.Marks)
                .FirstOrDefaultAsync(s => s.SubjectId == id);

            if (subject != null)
            {
                if (subject.Marks.Any())
                {
                    // Soft delete - deactivate instead of deleting
                    subject.IsActive = false;
                    _context.Update(subject);
                    TempData["InfoMessage"] = $"Subject '{subject.SubjectName}' has been deactivated because it has associated marks.";
                }
                else
                {
                    // Hard delete if no marks exist
                    _context.Subjects.Remove(subject);
                    TempData["SuccessMessage"] = $"Subject '{subject.SubjectName}' has been deleted successfully.";
                }
                
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Subjects/Activate/5
        public async Task<IActionResult> Activate(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject != null)
            {
                subject.IsActive = true;
                _context.Update(subject);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Subject '{subject.SubjectName}' has been activated.";
            }

            return RedirectToAction(nameof(Index));
        }

        // API Endpoint: Get subjects by level
        [HttpGet]
        public async Task<IActionResult> GetSubjectsByLevel(EducationLevel level)
        {
            var subjects = await _context.Subjects
                .Where(s => s.Level == level && s.IsActive)
                .OrderBy(s => s.SubjectName)
                .Select(s => new { 
                    value = s.SubjectId, 
                    text = $"{s.SubjectCode} - {s.SubjectName}",
                    code = s.SubjectCode,
                    name = s.SubjectName
                })
                .ToListAsync();

            return Json(subjects);
        }

        // API Endpoint: Get subjects for a specific student (based on their class level)
        [HttpGet]
        public async Task<IActionResult> GetSubjectsForStudent(int studentId)
        {
            var student = await _context.Students
                .Include(s => s.Class)
                .FirstOrDefaultAsync(s => s.StudentId == studentId);

            if (student == null)
            {
                return Json(new { error = "Student not found" });
            }

            var subjects = await _context.Subjects
                .Where(s => s.Level == student.Class.Level && s.IsActive)
                .OrderBy(s => s.SubjectName)
                .Select(s => new { 
                    value = s.SubjectId, 
                    text = $"{s.SubjectCode} - {s.SubjectName}",
                    code = s.SubjectCode,
                    name = s.SubjectName
                })
                .ToListAsync();

            return Json(subjects);
        }

        // API Endpoint: Get subjects for a specific class
        [HttpGet]
        public async Task<IActionResult> GetSubjectsForClass(int classId)
        {
            var classInfo = await _context.Classes.FirstOrDefaultAsync(c => c.ClassId == classId);
            
            if (classInfo == null)
            {
                return Json(new { error = "Class not found" });
            }

            var subjects = await _context.Subjects
                .Where(s => s.Level == classInfo.Level && s.IsActive)
                .OrderBy(s => s.SubjectName)
                .Select(s => new { 
                    value = s.SubjectId, 
                    text = $"{s.SubjectCode} - {s.SubjectName}",
                    code = s.SubjectCode,
                    name = s.SubjectName
                })
                .ToListAsync();

            return Json(subjects);
        }

        private bool SubjectExists(int id)
        {
            return _context.Subjects.Any(e => e.SubjectId == id);
        }
    }
}