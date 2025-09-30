using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FatimaSchoolManagement.Data;
using FatimaSchoolManagement.Models;
using FatimaSchoolManagement.Models.ViewModels;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ClosedXML.Excel;
using System.Text;

namespace FatimaSchoolManagement.Controllers
{
    public class ReportsController : Controller
    {
        private readonly SchoolDbContext _context;

        public ReportsController(SchoolDbContext context)
        {
            _context = context;
        }

        // GET: Reports
        public async Task<IActionResult> Index()
        {
            var classes = await _context.Classes.Where(c => c.IsActive).OrderBy(c => c.ClassName).ToListAsync();
            ViewBag.Classes = classes;
            ViewBag.Terms = Enum.GetValues<Term>();
            ViewBag.AcademicYears = new[] { 2023, 2024, 2025 };

            return View();
        }

        // GET: Reports/ClassPerformance
        public async Task<IActionResult> ClassPerformance()
        {
            var classes = await _context.Classes.Where(c => c.IsActive).OrderBy(c => c.ClassName).ToListAsync();
            ViewBag.Classes = classes;
            ViewBag.Terms = Enum.GetValues<Term>();
            ViewBag.AcademicYears = new[] { 2023, 2024, 2025 };

            return View();
        }

        // GET: Reports/Subs
        public async Task<IActionResult> Subs()
        {
            var classes = await _context.Classes.Where(c => c.IsActive).OrderBy(c => c.ClassName).ToListAsync();
            ViewBag.Classes = classes;
            ViewBag.Terms = Enum.GetValues<Term>();
            ViewBag.AcademicYears = new[] { 2023, 2024, 2025 };

            return View();
        }

        // POST: Reports/ViewSubsMarks
        [HttpPost]
        public async Task<IActionResult> ViewSubsMarks(int classId, int academicYear, Term term, string markType)
        {
            var classEntity = await _context.Classes.FindAsync(classId);
            if (classEntity == null)
            {
                return NotFound();
            }

            var students = await _context.Students
                .Where(s => s.ClassId == classId)
                .OrderBy(s => s.FullName)
                .ToListAsync();

            var subjects = await _context.Subjects
                .Where(s => s.Level == classEntity.Level && s.IsActive)
                .OrderBy(s => s.SubjectName)
                .ToListAsync();

            var allMarks = await _context.Marks
                .Where(m => students.Select(s => s.StudentId).Contains(m.StudentId)
                           && m.AcademicYear == academicYear && m.Term == term)
                .ToListAsync();

            var studentRows = new List<StudentSubsRow>();

            foreach (var student in students)
            {
                var studentMarks = allMarks.Where(m => m.StudentId == student.StudentId).ToList();
                var subjectMarksDict = new Dictionary<int, decimal?>();

                foreach (var subject in subjects)
                {
                    var mark = studentMarks.FirstOrDefault(m => m.SubjectId == subject.SubjectId);
                    decimal? selectedMark = null;
                    if (mark != null)
                    {
                        selectedMark = markType switch
                        {
                            "BOT" => mark.BOTMark,
                            "MOT" => mark.MOTMark,
                            "EOT" => mark.EOTMark,
                            _ => null
                        };
                    }
                    subjectMarksDict[subject.SubjectId] = selectedMark;
                }

                studentRows.Add(new StudentSubsRow
                {
                    Student = student,
                    SubjectMarks = subjectMarksDict
                });
            }

            var viewModel = new SubsMarkSheetViewModel
            {
                Class = classEntity,
                AcademicYear = academicYear,
                Term = term,
                MarkType = markType,
                Subjects = subjects,
                StudentRows = studentRows
            };

            return View(viewModel);
        }

        // GET: Reports/StudentReportCard/5?academicYear=2024&term=Term1
        public async Task<IActionResult> StudentReportCard(int studentId, int academicYear, Term term)
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
                GradeDescription = m.GradeDescription,
                OLevelPoints = m.OLevelPoints,
                OLevelGrade = m.OLevelGrade
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

        // GET: Reports/GenerateStudentReportCardPDF/5?academicYear=2024&term=Term1
        public async Task<IActionResult> GenerateStudentReportCardPDF(int studentId, int academicYear, Term term)
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

            var pdfBytes = GenerateReportCardPDF(student, marks, academicYear, term, totalMarks, averageMark, position, classAverages.Count);

            var fileName = $"ReportCard_{student.StudentNumber}_{academicYear}_{term}.pdf";
            return File(pdfBytes, "application/pdf", fileName);
        }

        // GET: Reports/GenerateClassReportCards?classId=1&academicYear=2024&term=Term1
        public async Task<IActionResult> GenerateClassReportCards(int classId, int academicYear, Term term)
        {
            var classEntity = await _context.Classes.FindAsync(classId);
            if (classEntity == null)
            {
                return NotFound();
            }

            var students = await _context.Students
                .Where(s => s.ClassId == classId)
                .Include(s => s.Class)
                .OrderBy(s => s.FullName)
                .ToListAsync();

            if (!students.Any())
            {
                TempData["ErrorMessage"] = "No students found in this class.";
                return RedirectToAction(nameof(Index));
            }

            var allMarks = await _context.Marks
                .Include(m => m.Subject)
                .Where(m => students.Select(s => s.StudentId).Contains(m.StudentId) && m.AcademicYear == academicYear && m.Term == term)
                .ToListAsync();

            // Calculate positions for all students
            var studentAverages = allMarks
                .Where(m => m.BOTMark.HasValue && m.MOTMark.HasValue && m.EOTMark.HasValue)
                .GroupBy(m => m.StudentId)
                .Select(g => new { 
                    StudentId = g.Key, 
                    Average = g.Average(m => (m.BOTMark!.Value * 0.1m) + (m.MOTMark!.Value * 0.2m) + (m.EOTMark!.Value * 0.7m))
                })
                .OrderByDescending(x => x.Average)
                .ToList();

            using var memoryStream = new MemoryStream();
            var document = new Document(PageSize.A4);
            var writer = PdfWriter.GetInstance(document, memoryStream);

            document.Open();

            foreach (var student in students)
            {
                var studentMarks = allMarks.Where(m => m.StudentId == student.StudentId).ToList();
                var completeMarks = studentMarks.Where(m => m.BOTMark.HasValue && m.MOTMark.HasValue && m.EOTMark.HasValue).ToList();
                var totalMarks = completeMarks.Sum(m => m.FinalMark);
                var averageMark = completeMarks.Any() ? completeMarks.Average(m => m.FinalMark) : 0;
                var position = studentAverages.FindIndex(x => x.StudentId == student.StudentId) + 1;

                GenerateReportCardPage(document, student, studentMarks, academicYear, term, totalMarks, averageMark, position, studentAverages.Count);

                if (student != students.Last())
                {
                    document.NewPage();
                }
            }

            document.Close();

            var fileName = $"ClassReportCards_{classEntity.ClassName}_{academicYear}_{term}.pdf";
            return File(memoryStream.ToArray(), "application/pdf", fileName);
        }

        // GET: Reports/ClassMarkSheet?classId=1&academicYear=2024&term=Term1
        public async Task<IActionResult> ClassMarkSheet(int classId, int academicYear, Term term)
        {
            var classEntity = await _context.Classes.FindAsync(classId);
            if (classEntity == null)
            {
                return NotFound();
            }

            var students = await _context.Students
                .Where(s => s.ClassId == classId)
                .OrderBy(s => s.FullName)
                .ToListAsync();

            var subjects = await _context.Subjects
                .Where(s => s.Level == classEntity.Level && s.IsActive)
                .OrderBy(s => s.SubjectName)
                .ToListAsync();

            var allMarks = await _context.Marks
                .Where(m => students.Select(s => s.StudentId).Contains(m.StudentId) 
                           && m.AcademicYear == academicYear && m.Term == term)
                .ToListAsync();

            var studentRows = new List<StudentMarkSheetRow>();

            foreach (var student in students)
            {
                var studentMarks = allMarks.Where(m => m.StudentId == student.StudentId).ToList();
                var subjectMarksDict = new Dictionary<int, StudentSubjectMark>();
                
                foreach (var subject in subjects)
                {
                    var mark = studentMarks.FirstOrDefault(m => m.SubjectId == subject.SubjectId);
                    if (mark != null)
                    {
                        subjectMarksDict[subject.SubjectId] = new StudentSubjectMark
                        {
                            BOTMark = mark.BOTMark,
                            MOTMark = mark.MOTMark,
                            EOTMark = mark.EOTMark,
                            FinalMark = mark.FinalMark,
                            Grade = mark.Grade,
                            OLevelPoints = mark.OLevelPoints,
                            OLevelGrade = mark.OLevelGrade
                        };
                    }
                    else
                    {
                        subjectMarksDict[subject.SubjectId] = new StudentSubjectMark
                        {
                            BOTMark = null,
                            MOTMark = null,
                            EOTMark = null,
                            FinalMark = 0,
                            Grade = "-",
                            OLevelPoints = 0,
                            OLevelGrade = "-"
                        };
                    }
                }

                var completeMarks = subjectMarksDict.Values.Where(sm => sm.IsComplete).ToList();
                var overallAverage = completeMarks.Any() ? completeMarks.Average(sm => sm.FinalMark) : 0;
                var overallOLevelPoints = completeMarks.Any() ? completeMarks.Average(sm => sm.OLevelPoints) : 0;
                var overallOLevelGrade = GetOLevelGrade(overallOLevelPoints);

                studentRows.Add(new StudentMarkSheetRow
                {
                    Student = student,
                    SubjectMarks = subjectMarksDict,
                    OverallAverage = overallAverage,
                    OverallGrade = GetGrade(overallAverage),
                    OverallOLevelPoints = overallOLevelPoints,
                    OverallOLevelGrade = overallOLevelGrade
                });
            }

            // Calculate positions
            var sortedStudents = studentRows.OrderByDescending(sr => sr.OverallAverage).ToList();
            for (int i = 0; i < sortedStudents.Count; i++)
            {
                sortedStudents[i].Position = i + 1;
            }

            // Calculate class averages per subject
            var classAverages = new Dictionary<string, decimal>();
            foreach (var subject in subjects)
            {
                var subjectMarks = studentRows
                    .Select(sr => sr.SubjectMarks[subject.SubjectId])
                    .Where(sm => sm.IsComplete)
                    .ToList();
                
                if (subjectMarks.Any())
                {
                    classAverages[subject.SubjectCode] = subjectMarks.Average(sm => sm.FinalMark);
                }
                else
                {
                    classAverages[subject.SubjectCode] = 0;
                }
            }

            var viewModel = new ClassMarkSheetViewModel
            {
                Class = classEntity,
                AcademicYear = academicYear,
                Term = term,
                Subjects = subjects,
                StudentRows = studentRows.OrderBy(sr => sr.Student.FullName).ToList(),
                ClassAverages = classAverages
            };

            return View(viewModel);
        }

        // GET: Reports/ExportClassMarkSheetToExcel?classId=1&academicYear=2024&term=Term1
        public async Task<IActionResult> ExportClassMarkSheetToExcel(int classId, int academicYear, Term term)
        {
            var classEntity = await _context.Classes.FindAsync(classId);
            if (classEntity == null)
            {
                return NotFound();
            }

            var students = await _context.Students
                .Where(s => s.ClassId == classId)
                .OrderBy(s => s.FullName)
                .ToListAsync();

            var subjects = await _context.Subjects
                .Where(s => s.Level == classEntity.Level && s.IsActive)
                .OrderBy(s => s.SubjectName)
                .ToListAsync();

            var allMarks = await _context.Marks
                .Where(m => students.Select(s => s.StudentId).Contains(m.StudentId) 
                           && m.AcademicYear == academicYear && m.Term == term)
                .ToListAsync();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add($"{classEntity.ClassName} Mark Sheet");

            // Header information
            worksheet.Cell(1, 1).Value = "OUR LADY OF FATIMA SECONDARY SCHOOL";
            worksheet.Cell(1, 1).Style.Font.Bold = true;
            worksheet.Cell(1, 1).Style.Font.FontSize = 16;
            worksheet.Range(1, 1, 1, subjects.Count + 6).Merge();
            worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            worksheet.Cell(2, 1).Value = $"CLASS MARK SHEET - {classEntity.ClassName} - {term} {academicYear}";
            worksheet.Cell(2, 1).Style.Font.Bold = true;
            worksheet.Cell(2, 1).Style.Font.FontSize = 14;
            worksheet.Range(2, 1, 2, subjects.Count + 6).Merge();
            worksheet.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // Column headers
            int row = 4;
            int col = 1;
            
            worksheet.Cell(row, col++).Value = "S/N";
            worksheet.Cell(row, col++).Value = "Student Number";
            worksheet.Cell(row, col++).Value = "Student Name";

            // Subject columns (BOT, MOT, EOT, Final, Grade for each subject)
            foreach (var subject in subjects)
            {
                worksheet.Cell(row, col).Value = subject.SubjectCode;
                worksheet.Cell(row + 1, col++).Value = "BOT";
                worksheet.Cell(row + 1, col++).Value = "MOT";
                worksheet.Cell(row + 1, col++).Value = "EOT";
                worksheet.Cell(row + 1, col++).Value = "Final";
                worksheet.Cell(row + 1, col++).Value = "Grade";
            }

            worksheet.Cell(row, col++).Value = "Average";
            worksheet.Cell(row, col++).Value = "Grade";
            worksheet.Cell(row, col++).Value = "Position";

            // Apply header styling
            var headerRange = worksheet.Range(row, 1, row + 1, col - 1);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
            headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
            headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // Student data
            row += 2;
            int studentNumber = 1;

            // Calculate student rows first for position calculation
            var studentRows = new List<StudentMarkSheetRow>();
            foreach (var student in students)
            {
                var studentMarks = allMarks.Where(m => m.StudentId == student.StudentId).ToList();
                var subjectMarksDict = new Dictionary<int, StudentSubjectMark>();
                
                foreach (var subject in subjects)
                {
                    var mark = studentMarks.FirstOrDefault(m => m.SubjectId == subject.SubjectId);
                    if (mark != null)
                    {
                        subjectMarksDict[subject.SubjectId] = new StudentSubjectMark
                        {
                            BOTMark = mark.BOTMark,
                            MOTMark = mark.MOTMark,
                            EOTMark = mark.EOTMark,
                            FinalMark = mark.FinalMark,
                            Grade = mark.Grade
                        };
                    }
                    else
                    {
                        subjectMarksDict[subject.SubjectId] = new StudentSubjectMark
                        {
                            BOTMark = null,
                            MOTMark = null,
                            EOTMark = null,
                            FinalMark = 0,
                            Grade = "-"
                        };
                    }
                }

                var completeMarks = subjectMarksDict.Values.Where(sm => sm.IsComplete).ToList();
                var overallAverage = completeMarks.Any() ? completeMarks.Average(sm => sm.FinalMark) : 0;

                studentRows.Add(new StudentMarkSheetRow
                {
                    Student = student,
                    SubjectMarks = subjectMarksDict,
                    OverallAverage = overallAverage,
                    OverallGrade = GetGrade(overallAverage)
                });
            }

            // Calculate positions
            var sortedStudents = studentRows.OrderByDescending(sr => sr.OverallAverage).ToList();
            for (int i = 0; i < sortedStudents.Count; i++)
            {
                sortedStudents[i].Position = i + 1;
            }

            // Add student data to Excel
            foreach (var studentRow in studentRows.OrderBy(sr => sr.Student.FullName))
            {
                col = 1;
                worksheet.Cell(row, col++).Value = studentNumber++;
                worksheet.Cell(row, col++).Value = studentRow.Student.StudentNumber;
                worksheet.Cell(row, col++).Value = studentRow.Student.FullName;

                foreach (var subject in subjects)
                {
                    var subjectMark = studentRow.SubjectMarks[subject.SubjectId];
                    worksheet.Cell(row, col++).Value = subjectMark.BOTMark?.ToString("F1") ?? "-";
                    worksheet.Cell(row, col++).Value = subjectMark.MOTMark?.ToString("F1") ?? "-";
                    worksheet.Cell(row, col++).Value = subjectMark.EOTMark?.ToString("F1") ?? "-";
                    worksheet.Cell(row, col++).Value = subjectMark.FinalMark.ToString("F1");
                    worksheet.Cell(row, col++).Value = subjectMark.Grade;
                }

                worksheet.Cell(row, col++).Value = studentRow.OverallAverage.ToString("F1");
                worksheet.Cell(row, col++).Value = studentRow.OverallGrade;
                worksheet.Cell(row, col++).Value = studentRow.Position;

                row++;
            }

            // Add class averages row
            row++;
            col = 1;
            worksheet.Cell(row, col++).Value = "";
            worksheet.Cell(row, col++).Value = "";
            worksheet.Cell(row, col++).Value = "CLASS AVERAGE";
            worksheet.Cell(row, col - 1).Style.Font.Bold = true;

            foreach (var subject in subjects)
            {
                var subjectMarks = studentRows
                    .Select(sr => sr.SubjectMarks[subject.SubjectId])
                    .Where(sm => sm.IsComplete)
                    .ToList();
                
                col += 3; // Skip BOT, MOT, EOT columns
                
                if (subjectMarks.Any())
                {
                    worksheet.Cell(row, col++).Value = subjectMarks.Average(sm => sm.FinalMark).ToString("F1");
                }
                else
                {
                    worksheet.Cell(row, col++).Value = "-";
                }
                
                col++; // Skip grade column
            }

            var overallClassAverage = studentRows.Where(sr => sr.OverallAverage > 0).Any() 
                ? studentRows.Where(sr => sr.OverallAverage > 0).Average(sr => sr.OverallAverage) 
                : 0;
            worksheet.Cell(row, col++).Value = overallClassAverage.ToString("F1");
            worksheet.Cell(row, col++).Value = GetGrade(overallClassAverage);

            // Auto-fit columns
            worksheet.Columns().AdjustToContents();

            // Add borders to data
            var dataRange = worksheet.Range(4, 1, row, col - 1);
            dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
            dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            // Generate file
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            
            var fileName = $"MarkSheet_{classEntity.ClassName}_{academicYear}_{term}.xlsx";
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        // GET: Reports/ExportSubsToExcel?classId=1&academicYear=2024&term=Term1&markType=BOT
        public async Task<IActionResult> ExportSubsToExcel(int classId, int academicYear, Term term, string markType)
        {
            var classEntity = await _context.Classes.FindAsync(classId);
            if (classEntity == null)
            {
                return NotFound();
            }

            var students = await _context.Students
                .Where(s => s.ClassId == classId)
                .OrderBy(s => s.FullName)
                .ToListAsync();

            var subjects = await _context.Subjects
                .Where(s => s.Level == classEntity.Level && s.IsActive)
                .OrderBy(s => s.SubjectName)
                .ToListAsync();

            var allMarks = await _context.Marks
                .Where(m => students.Select(s => s.StudentId).Contains(m.StudentId)
                           && m.AcademicYear == academicYear && m.Term == term)
                .ToListAsync();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add($"{classEntity.ClassName} {markType} Marks");

            // Header information
            worksheet.Cell(1, 1).Value = "OUR LADY OF FATIMA SECONDARY SCHOOL";
            worksheet.Cell(1, 1).Style.Font.Bold = true;
            worksheet.Cell(1, 1).Style.Font.FontSize = 16;
            worksheet.Range(1, 1, 1, subjects.Count + 3).Merge();
            worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            worksheet.Cell(2, 1).Value = $"{markType} MARKS - {classEntity.ClassName} - {term} {academicYear}";
            worksheet.Cell(2, 1).Style.Font.Bold = true;
            worksheet.Cell(2, 1).Style.Font.FontSize = 14;
            worksheet.Range(2, 1, 2, subjects.Count + 3).Merge();
            worksheet.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // Column headers
            int row = 4;
            int col = 1;

            worksheet.Cell(row, col++).Value = "S/N";
            worksheet.Cell(row, col++).Value = "Student Number";
            worksheet.Cell(row, col++).Value = "Student Name";

            // Subject columns
            foreach (var subject in subjects)
            {
                worksheet.Cell(row, col++).Value = subject.SubjectName;
            }

            // Apply header styling
            var headerRange = worksheet.Range(row, 1, row, col - 1);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
            headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
            headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // Student data
            row++;
            int studentNumber = 1;

            foreach (var student in students)
            {
                var studentMarks = allMarks.Where(m => m.StudentId == student.StudentId).ToList();
                col = 1;
                worksheet.Cell(row, col++).Value = studentNumber++;
                worksheet.Cell(row, col++).Value = student.StudentNumber;
                worksheet.Cell(row, col++).Value = student.FullName;

                foreach (var subject in subjects)
                {
                    var mark = studentMarks.FirstOrDefault(m => m.SubjectId == subject.SubjectId);
                    decimal? selectedMark = null;
                    if (mark != null)
                    {
                        selectedMark = markType switch
                        {
                            "BOT" => mark.BOTMark,
                            "MOT" => mark.MOTMark,
                            "EOT" => mark.EOTMark,
                            _ => null
                        };
                    }
                    worksheet.Cell(row, col++).Value = selectedMark?.ToString("F1") ?? "-";
                }

                row++;
            }

            // Auto-fit columns
            worksheet.Columns().AdjustToContents();

            // Add borders to data
            var dataRange = worksheet.Range(4, 1, row - 1, col - 1);
            dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
            dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            // Generate file
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            var fileName = $"{markType}Marks_{classEntity.ClassName}_{academicYear}_{term}.xlsx";
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        private byte[] GenerateReportCardPDF(Student student, List<Mark> marks, int academicYear, Term term, decimal totalMarks, decimal averageMark, int position, int totalStudents)
        {
            using var memoryStream = new MemoryStream();
            var document = new Document(PageSize.A4);
            var writer = PdfWriter.GetInstance(document, memoryStream);

            document.Open();
            GenerateReportCardPage(document, student, marks, academicYear, term, totalMarks, averageMark, position, totalStudents);
            document.Close();

            return memoryStream.ToArray();
        }

        private void GenerateReportCardPage(Document document, Student student, List<Mark> marks, int academicYear, Term term, decimal totalMarks, decimal averageMark, int position, int totalStudents)
        {
            // Fonts
            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
            var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
            var smallFont = FontFactory.GetFont(FontFactory.HELVETICA, 8);

            // School Header
            var schoolNameParagraph = new Paragraph("OUR LADY OF FATIMA SECONDARY SCHOOL", titleFont)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 5f
            };
            document.Add(schoolNameParagraph);

            var reportTitleParagraph = new Paragraph($"STUDENT REPORT CARD - {term} {academicYear}", headerFont)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 15f
            };
            document.Add(reportTitleParagraph);

            // Student Information Table
            var studentInfoTable = new PdfPTable(2) { WidthPercentage = 100f };
            studentInfoTable.SetWidths(new float[] { 1f, 2f });

            AddTableCell(studentInfoTable, "Student Name:", normalFont);
            AddTableCell(studentInfoTable, student.FullName, normalFont);
            AddTableCell(studentInfoTable, "Student Number:", normalFont);
            AddTableCell(studentInfoTable, student.StudentNumber, normalFont);
            AddTableCell(studentInfoTable, "Class:", normalFont);
            AddTableCell(studentInfoTable, student.Class.ClassName, normalFont);
            AddTableCell(studentInfoTable, "Level:", normalFont);
            AddTableCell(studentInfoTable, student.Level == EducationLevel.OLevel ? "O-Level" : "A-Level", normalFont);
            AddTableCell(studentInfoTable, "Date of Birth:", normalFont);
            AddTableCell(studentInfoTable, student.DateOfBirth.ToString("dd/MM/yyyy"), normalFont);

            document.Add(studentInfoTable);
            document.Add(new Paragraph(" ", normalFont)); // Space

            // Marks Table
            var marksTable = new PdfPTable(7) { WidthPercentage = 100f };
            marksTable.SetWidths(new float[] { 1.5f, 2.5f, 1f, 1f, 1f, 1f, 1f });

            // Headers
            AddTableHeaderCell(marksTable, "Code", headerFont);
            AddTableHeaderCell(marksTable, "Subject", headerFont);
            AddTableHeaderCell(marksTable, "BOT(10%)", headerFont);
            AddTableHeaderCell(marksTable, "MOT(20%)", headerFont);
            AddTableHeaderCell(marksTable, "EOT(70%)", headerFont);
            AddTableHeaderCell(marksTable, "Final", headerFont);
            AddTableHeaderCell(marksTable, "Grade", headerFont);

            // Marks data
            foreach (var mark in marks.OrderBy(m => m.Subject.SubjectName))
            {
                AddTableCell(marksTable, mark.Subject.SubjectCode, normalFont);
                AddTableCell(marksTable, mark.Subject.SubjectName, normalFont);
                AddTableCell(marksTable, mark.BOTMark?.ToString("F1") ?? "-", normalFont);
                AddTableCell(marksTable, mark.MOTMark?.ToString("F1") ?? "-", normalFont);
                AddTableCell(marksTable, mark.EOTMark?.ToString("F1") ?? "-", normalFont);
                var finalMarkText = student.Level == EducationLevel.OLevel
                    ? $"{mark.FinalMark.ToString("F1")} ({mark.OLevelPoints.ToString("F2")})"
                    : mark.FinalMark.ToString("F1");
                AddTableCell(marksTable, finalMarkText, normalFont);
                AddTableCell(marksTable, mark.Grade, normalFont);
            }

            document.Add(marksTable);
            document.Add(new Paragraph(" ", normalFont)); // Space

            // Summary Table
            var summaryTable = new PdfPTable(2) { WidthPercentage = 60f };
            summaryTable.SetWidths(new float[] { 1.5f, 1f });

            AddTableCell(summaryTable, "Total Marks:", headerFont);
            AddTableCell(summaryTable, totalMarks.ToString("F1"), normalFont);
            AddTableCell(summaryTable, "Average Mark:", headerFont);
            AddTableCell(summaryTable, averageMark.ToString("F1"), normalFont);
            AddTableCell(summaryTable, "Overall Grade:", headerFont);
            AddTableCell(summaryTable, GetGrade(averageMark), normalFont);
            AddTableCell(summaryTable, "Class Position:", headerFont);
            AddTableCell(summaryTable, $"{position} of {totalStudents}", normalFont);

            document.Add(summaryTable);
            document.Add(new Paragraph(" ", normalFont)); // Space

            // Grading System
            var gradingParagraph = new Paragraph("Grading System:", headerFont)
            {
                SpacingBefore = 15f,
                SpacingAfter = 5f
            };
            document.Add(gradingParagraph);

            var gradingText = student.Level == EducationLevel.OLevel
                ? "A (2.50 – 3.00): Exceptional Achievement | B (2.10 – 2.49): Outstanding Performance | " +
                  "C (1.60 – 2.09): Satisfactory Performance | D (1.00 – 1.59): Basic Understanding | " +
                  "E (0.00 – 0.99): Elementary Understanding"
                : "A (80-100): Exceptional Achievement | B (70-79): Outstanding Performance | " +
                  "C (60-69): Satisfactory Performance | D (50-59): Basic Understanding | " +
                  "E (0-49): Elementary Understanding";
            var gradingInfo = new Paragraph(gradingText, smallFont);
            document.Add(gradingInfo);

            // Comments Section
            var commentsTable = new PdfPTable(1) { WidthPercentage = 100f };
            commentsTable.SpacingBefore = 15f;

            AddTableHeaderCell(commentsTable, "Teacher's Comments", headerFont);
            var commentCell = new PdfPCell(new Phrase("_____________________________________" +
                                                     "_____________________________________" +
                                                     "_____________________________________", normalFont))
            {
                MinimumHeight = 40f,
                Border = Rectangle.BOX
            };
            commentsTable.AddCell(commentCell);

            AddTableHeaderCell(commentsTable, "Head Teacher's Comments", headerFont);
            var headCommentCell = new PdfPCell(new Phrase("_____________________________________" +
                                                         "_____________________________________" +
                                                         "_____________________________________", normalFont))
            {
                MinimumHeight = 40f,
                Border = Rectangle.BOX
            };
            commentsTable.AddCell(headCommentCell);

            document.Add(commentsTable);

            // Footer
            var footerText = new Paragraph($"Generated on: {DateTime.Now:dd/MM/yyyy HH:mm}", smallFont)
            {
                Alignment = Element.ALIGN_RIGHT,
                SpacingBefore = 20f
            };
            document.Add(footerText);
        }

        private void AddTableCell(PdfPTable table, string text, Font font)
        {
            var cell = new PdfPCell(new Phrase(text, font))
            {
                Padding = 5f,
                Border = Rectangle.BOX
            };
            table.AddCell(cell);
        }

        private void AddTableHeaderCell(PdfPTable table, string text, Font font)
        {
            var cell = new PdfPCell(new Phrase(text, font))
            {
                Padding = 5f,
                Border = Rectangle.BOX,
                BackgroundColor = new BaseColor(211, 211, 211), // Light gray
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            table.AddCell(cell);
        }

        private string GetGrade(decimal mark)
        {
            if (mark >= 80) return "A";
            if (mark >= 70) return "B";
            if (mark >= 60) return "C";
            if (mark >= 50) return "D";
            return "E";
        }

        private string GetOLevelGrade(decimal points)
        {
            if (points >= 2.50m) return "A";
            if (points >= 2.10m) return "B";
            if (points >= 1.60m) return "C";
            if (points >= 1.00m) return "D";
            return "E";
        }
    }
}