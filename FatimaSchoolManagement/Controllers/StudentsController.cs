using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FatimaSchoolManagement.Data;
using FatimaSchoolManagement.Models;
using FatimaSchoolManagement.Models.ViewModels;

namespace FatimaSchoolManagement.Controllers
{
    public class StudentsController : Controller
    {
        private readonly SchoolDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public StudentsController(SchoolDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Students
        public async Task<IActionResult> Index(string searchTerm, int? classId, EducationLevel? level, int page = 1, int pageSize = 20)
        {
            var studentsQuery = _context.Students.Include(s => s.Class).AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                studentsQuery = studentsQuery.Where(s => 
                    s.FullName.Contains(searchTerm) || 
                    s.StudentNumber.Contains(searchTerm));
            }

            if (classId.HasValue)
            {
                studentsQuery = studentsQuery.Where(s => s.ClassId == classId.Value);
            }

            if (level.HasValue)
            {
                studentsQuery = studentsQuery.Where(s => s.Level == level.Value);
            }

            var totalStudents = await studentsQuery.CountAsync();
            var totalPages = (int)Math.Ceiling(totalStudents / (double)pageSize);

            var students = await studentsQuery
                .OrderBy(s => s.FullName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var classes = await _context.Classes.Where(c => c.IsActive).OrderBy(c => c.ClassName).ToListAsync();

            var viewModel = new StudentListViewModel
            {
                Students = students,
                Classes = classes,
                SearchTerm = searchTerm,
                SelectedClassId = classId,
                SelectedLevel = level,
                CurrentPage = page,
                TotalPages = totalPages,
                TotalStudents = totalStudents,
                PageSize = pageSize
            };

            return View(viewModel);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Class)
                .Include(s => s.Marks)
                    .ThenInclude(m => m.Subject)
                .FirstOrDefaultAsync(m => m.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public async Task<IActionResult> Create()
        {
            var classes = await _context.Classes.Where(c => c.IsActive).OrderBy(c => c.ClassName).ToListAsync();
            var viewModel = new StudentCreateViewModel
            {
                AvailableClasses = classes
            };
            return View(viewModel);
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Check if student number already exists
                var existingStudent = await _context.Students
                    .FirstOrDefaultAsync(s => s.StudentNumber == viewModel.StudentNumber);
                
                if (existingStudent != null)
                {
                    ModelState.AddModelError("StudentNumber", "A student with this number already exists.");
                    viewModel.AvailableClasses = await _context.Classes.Where(c => c.IsActive).OrderBy(c => c.ClassName).ToListAsync();
                    return View(viewModel);
                }

                var student = new Student
                {
                    StudentNumber = viewModel.StudentNumber,
                    FullName = viewModel.FullName,
                    DateOfBirth = viewModel.DateOfBirth,
                    LINNumber = viewModel.LINNumber,
                    ParentsContact = viewModel.ParentsContact,
                    NextOfKin = viewModel.NextOfKin,
                    NextOfKinContact = viewModel.NextOfKinContact,
                    Level = viewModel.Level,
                    ClassId = viewModel.ClassId
                };

                // Handle photo upload
                if (viewModel.Photo != null && viewModel.Photo.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "students");
                    Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = $"{Guid.NewGuid()}_{viewModel.Photo.FileName}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await viewModel.Photo.CopyToAsync(fileStream);
                    }

                    student.PhotoPath = $"/uploads/students/{uniqueFileName}";
                }

                _context.Add(student);
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = "Student created successfully!";
                return RedirectToAction(nameof(Index));
            }

            viewModel.AvailableClasses = await _context.Classes.Where(c => c.IsActive).OrderBy(c => c.ClassName).ToListAsync();
            return View(viewModel);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            var classes = await _context.Classes.Where(c => c.IsActive).OrderBy(c => c.ClassName).ToListAsync();
            var viewModel = new StudentEditViewModel
            {
                StudentId = student.StudentId,
                StudentNumber = student.StudentNumber,
                FullName = student.FullName,
                DateOfBirth = student.DateOfBirth,
                LINNumber = student.LINNumber,
                ParentsContact = student.ParentsContact,
                NextOfKin = student.NextOfKin,
                NextOfKinContact = student.NextOfKinContact,
                Level = student.Level,
                ClassId = student.ClassId,
                CurrentPhotoPath = student.PhotoPath,
                AvailableClasses = classes
            };

            return View(viewModel);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StudentEditViewModel viewModel)
        {
            if (id != viewModel.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var student = await _context.Students.FindAsync(id);
                    if (student == null)
                    {
                        return NotFound();
                    }

                    // Check if student number already exists (excluding current student)
                    var existingStudent = await _context.Students
                        .FirstOrDefaultAsync(s => s.StudentNumber == viewModel.StudentNumber && s.StudentId != id);
                    
                    if (existingStudent != null)
                    {
                        ModelState.AddModelError("StudentNumber", "A student with this number already exists.");
                        viewModel.AvailableClasses = await _context.Classes.Where(c => c.IsActive).OrderBy(c => c.ClassName).ToListAsync();
                        return View(viewModel);
                    }

                    student.StudentNumber = viewModel.StudentNumber;
                    student.FullName = viewModel.FullName;
                    student.DateOfBirth = viewModel.DateOfBirth;
                    student.LINNumber = viewModel.LINNumber;
                    student.ParentsContact = viewModel.ParentsContact;
                    student.NextOfKin = viewModel.NextOfKin;
                    student.NextOfKinContact = viewModel.NextOfKinContact;
                    student.Level = viewModel.Level;
                    student.ClassId = viewModel.ClassId;

                    // Handle photo upload
                    if (viewModel.Photo != null && viewModel.Photo.Length > 0)
                    {
                        // Delete old photo if exists
                        if (!string.IsNullOrEmpty(student.PhotoPath))
                        {
                            var oldPhotoPath = Path.Combine(_environment.WebRootPath, student.PhotoPath.TrimStart('/'));
                            if (System.IO.File.Exists(oldPhotoPath))
                            {
                                System.IO.File.Delete(oldPhotoPath);
                            }
                        }

                        var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "students");
                        Directory.CreateDirectory(uploadsFolder);

                        var uniqueFileName = $"{Guid.NewGuid()}_{viewModel.Photo.FileName}";
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await viewModel.Photo.CopyToAsync(fileStream);
                        }

                        student.PhotoPath = $"/uploads/students/{uniqueFileName}";
                    }

                    _context.Update(student);
                    await _context.SaveChangesAsync();
                    
                    TempData["SuccessMessage"] = "Student updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(viewModel.StudentId))
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

            viewModel.AvailableClasses = await _context.Classes.Where(c => c.IsActive).OrderBy(c => c.ClassName).ToListAsync();
            return View(viewModel);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Class)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                // Delete photo if exists
                if (!string.IsNullOrEmpty(student.PhotoPath))
                {
                    var photoPath = Path.Combine(_environment.WebRootPath, student.PhotoPath.TrimStart('/'));
                    if (System.IO.File.Exists(photoPath))
                    {
                        System.IO.File.Delete(photoPath);
                    }
                }

                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Student deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        // API endpoints for AJAX calls
        [HttpGet]
        public async Task<IActionResult> GetStudentsByClass(int classId)
        {
            var students = await _context.Students
                .Where(s => s.ClassId == classId)
                .Select(s => new { s.StudentId, s.StudentNumber, s.FullName })
                .OrderBy(s => s.FullName)
                .ToListAsync();

            return Json(students);
        }

        [HttpGet("/api/students")]
        public async Task<IActionResult> GetAllStudentsApi()
        {
            var students = await _context.Students
                .Include(s => s.Class)
                .Select(s => new { 
                    s.StudentId, 
                    s.StudentNumber, 
                    s.FullName,
                    ClassName = s.Class.ClassName
                })
                .OrderBy(s => s.FullName)
                .ToListAsync();

            return Json(students);
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentId == id);
        }
    }
}