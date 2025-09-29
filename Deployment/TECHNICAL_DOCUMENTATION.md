# Fatima School Management System - Technical Documentation
## Developer & Administrator Guide

### Table of Contents
1. [System Architecture](#system-architecture)
2. [Technology Stack](#technology-stack)
3. [Database Schema](#database-schema)
4. [Application Structure](#application-structure)
5. [Configuration](#configuration)
6. [Deployment](#deployment)
7. [Security](#security)
8. [Performance](#performance)
9. [Maintenance](#maintenance)
10. [Development](#development)

---

## System Architecture

### Overview
The Fatima School Management System is built using a **Model-View-Controller (MVC)** architecture pattern with the following layers:

```
┌─────────────────────────────────────┐
│           Presentation Layer        │
│    (Views, Controllers, CSS/JS)     │
├─────────────────────────────────────┤
│            Business Layer           │
│      (Models, ViewModels, DTOs)     │
├─────────────────────────────────────┤
│            Data Layer               │
│   (Entity Framework, SQLite DB)     │
└─────────────────────────────────────┘
```

### Core Components

#### 1. Web Application Layer
- **ASP.NET Core MVC**: Web framework providing HTTP request handling
- **Razor Views**: Server-side rendering with HTML generation
- **Bootstrap UI**: Responsive front-end framework
- **jQuery**: Client-side JavaScript functionality

#### 2. Business Logic Layer
- **Entity Framework Core**: Object-Relational Mapping (ORM)
- **LINQ Queries**: Data querying and manipulation
- **CBC Grade Calculator**: Custom grading logic implementation
- **Report Generators**: PDF and Excel export functionality

#### 3. Data Storage Layer
- **SQLite Database**: File-based relational database
- **Entity Models**: Object representation of database tables
- **Database Context**: EF Core DbContext for data operations

---

## Technology Stack

### Backend Technologies
- **.NET 8**: Modern cross-platform framework
- **ASP.NET Core 8.0**: Web application framework
- **Entity Framework Core 8.0**: ORM for database operations
- **SQLite**: Lightweight, file-based database
- **C# 12**: Primary programming language

### Frontend Technologies
- **HTML5**: Modern markup language
- **CSS3**: Styling with responsive design
- **Bootstrap 5.3**: CSS framework for responsive UI
- **JavaScript (ES6+)**: Client-side scripting
- **jQuery 3.7**: JavaScript library for DOM manipulation

### Additional Libraries
- **iTextSharp**: PDF generation for reports
- **ClosedXML**: Excel file generation and export
- **SixLabors.ImageSharp**: Image processing for student photos
- **Microsoft.AspNetCore.StaticFiles**: Static file serving
- **Microsoft.Extensions.Hosting**: Application hosting

### Development Tools
- **Visual Studio 2022**: Primary IDE
- **SQL Server Management Studio**: Database management (optional)
- **Git**: Version control system
- **NuGet**: Package management

---

## Database Schema

### Core Tables

#### Students Table
```sql
CREATE TABLE Students (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    FullName TEXT NOT NULL,
    DateOfBirth TEXT NOT NULL,
    Gender TEXT NOT NULL,
    RegistrationNumber TEXT UNIQUE NOT NULL,
    ClassId INTEGER NOT NULL,
    ParentContact TEXT,
    Address TEXT,
    PhotoPath TEXT,
    CreatedDate TEXT NOT NULL,
    FOREIGN KEY (ClassId) REFERENCES Classes(Id)
);
```

#### Classes Table
```sql
CREATE TABLE Classes (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    ClassName TEXT UNIQUE NOT NULL,
    Description TEXT,
    CreatedDate TEXT NOT NULL
);
```

#### Subjects Table
```sql
CREATE TABLE Subjects (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    SubjectName TEXT UNIQUE NOT NULL,
    Description TEXT,
    CreatedDate TEXT NOT NULL
);
```

#### Marks Table
```sql
CREATE TABLE Marks (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    StudentId INTEGER NOT NULL,
    SubjectId INTEGER NOT NULL,
    AssessmentType TEXT NOT NULL, -- BOT, MOT, EOT
    Score REAL NOT NULL,
    MaxMarks REAL NOT NULL,
    Term TEXT NOT NULL,
    Year INTEGER NOT NULL,
    Comments TEXT,
    CreatedDate TEXT NOT NULL,
    FOREIGN KEY (StudentId) REFERENCES Students(Id),
    FOREIGN KEY (SubjectId) REFERENCES Subjects(Id)
);
```

### Data Relationships
```
Students ─┐
          ├─→ Marks ←─┐
Classes ──┘            │
                       │
Subjects ──────────────┘
```

### Indexes for Performance
```sql
CREATE INDEX IX_Students_ClassId ON Students(ClassId);
CREATE INDEX IX_Students_RegistrationNumber ON Students(RegistrationNumber);
CREATE INDEX IX_Marks_StudentId ON Marks(StudentId);
CREATE INDEX IX_Marks_SubjectId ON Marks(SubjectId);
CREATE INDEX IX_Marks_Term_Year ON Marks(Term, Year);
```

---

## Application Structure

### Project Organization
```
FatimaSchoolManagement/
├── Controllers/
│   ├── HomeController.cs          # Dashboard and main navigation
│   ├── StudentsController.cs      # Student CRUD operations
│   ├── MarksController.cs         # Grade and assessment management
│   ├── ReportsController.cs       # Report generation
│   └── ClassesController.cs       # Class management
├── Models/
│   ├── Student.cs                 # Student entity model
│   ├── Class.cs                   # Class entity model
│   ├── Subject.cs                 # Subject entity model
│   ├── Mark.cs                    # Assessment record model
│   └── SchoolContext.cs           # EF Core database context
├── Views/
│   ├── Home/                      # Dashboard views
│   ├── Students/                  # Student management views
│   ├── Marks/                     # Grade entry and display views
│   ├── Reports/                   # Report generation views
│   ├── Classes/                   # Class management views
│   └── Shared/                    # Layout and shared views
├── wwwroot/
│   ├── css/                       # Stylesheets
│   ├── js/                        # JavaScript files
│   ├── lib/                       # Third-party libraries
│   └── uploads/                   # Student photo storage
├── Data/
│   └── SeedData.cs               # Database initialization
├── Services/
│   ├── GradeCalculatorService.cs  # CBC grade calculations
│   ├── ReportService.cs           # Report generation logic
│   └── PhotoService.cs            # Image handling
└── ViewModels/
    ├── DashboardViewModel.cs      # Dashboard data aggregation
    ├── StudentReportViewModel.cs  # Student report data
    └── ClassMarkSheetViewModel.cs # Class performance data
```

### Key Controllers

#### HomeController.cs
```csharp
public class HomeController : Controller
{
    // Dashboard with statistics
    public async Task<IActionResult> Index()
    {
        var viewModel = new DashboardViewModel
        {
            TotalStudents = await _context.Students.CountAsync(),
            TotalClasses = await _context.Classes.CountAsync(),
            RecentGrades = await GetRecentGrades(),
            PerformanceStats = await GetPerformanceStatistics()
        };
        return View(viewModel);
    }
}
```

#### StudentsController.cs
```csharp
public class StudentsController : Controller
{
    // CRUD operations for students
    // Photo upload handling
    // Class assignment management
    // Academic history display
}
```

#### MarksController.cs
```csharp
public class MarksController : Controller
{
    // Grade entry and editing
    // CBC calculation implementation
    // Assessment type handling (BOT/MOT/EOT)
    // Bulk grade import functionality
}
```

---

## Configuration

### Application Settings

#### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=FatimaSchool.db"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  },
  "AllowedHosts": "*",
  "FileUpload": {
    "MaxSizeBytes": 5242880,
    "AllowedExtensions": [".jpg", ".jpeg", ".png", ".gif"],
    "UploadPath": "wwwroot/uploads"
  },
  "CBC": {
    "BOT_Weight": 0.10,
    "MOT_Weight": 0.20,
    "EOT_Weight": 0.70,
    "GradeThresholds": {
      "Grade1": 80,
      "Grade2": 65,
      "Grade3": 50,
      "Grade4": 0
    }
  }
}
```

#### appsettings.Development.json
```json
{
  "DetailedErrors": true,
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "System": "Information",
      "Microsoft": "Information"
    }
  }
}
```

### Environment Variables
- **ASPNETCORE_ENVIRONMENT**: Development/Production
- **ASPNETCORE_URLS**: Binding URLs (http://localhost:5001)
- **ConnectionStrings__DefaultConnection**: Database connection override

### Startup Configuration

#### Program.cs
```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddDbContext<SchoolContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IGradeCalculatorService, GradeCalculatorService>();
builder.Services.AddScoped<IReportService, ReportService>();

var app = builder.Build();

// Configure pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Initialize database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SchoolContext>();
    context.Database.EnsureCreated();
    SeedData.Initialize(context);
}

app.Run();
```

---

## Deployment

### Self-Contained Deployment

#### Publishing Command
```bash
dotnet publish -c Release -r win-x64 --self-contained true \
  -p:PublishSingleFile=false -p:PublishTrimmed=false \
  -o "./Deployment"
```

#### Deployment Structure
```
Deployment/
├── FatimaSchoolManagement.exe    # Main executable
├── FatimaSchool_Template.db      # Database template
├── FatimaSchool.db              # Working database
├── appsettings.json             # Configuration
├── appsettings.Development.json # Development config
├── SmartStart.ps1               # PowerShell launcher
├── SmartStart.bat               # Batch launcher
├── wwwroot/                     # Static web assets
├── *.dll                        # Application dependencies
└── Documentation/               # User manuals and guides
```

### Smart Launcher Implementation

#### SmartStart.ps1 Features
- **Automatic Port Detection**: Finds available ports (5000-5202, 49152-65535)
- **Environment Configuration**: Sets ASPNETCORE_ENVIRONMENT=Development
- **Database Initialization**: Copies template if needed
- **Browser Auto-Launch**: Opens default browser when ready
- **Error Handling**: Comprehensive error detection and reporting

#### Port Conflict Resolution
```powershell
# Port availability testing
function Test-Port {
    param([int]$Port)
    try {
        $listener = [System.Net.Sockets.TcpListener]::new([System.Net.IPAddress]::Any, $Port)
        $listener.Start()
        $listener.Stop()
        return $true
    } catch {
        return $false
    }
}

# Find available port from predefined list
$PortsToTry = @(5000, 5001, 5002, 5202, 5203, 5204, 3000, 3001, 8000, 8080)
foreach ($Port in $PortsToTry) {
    if (Test-Port -Port $Port) {
        $SelectedPort = $Port
        break
    }
}
```

### System Requirements

#### Minimum Requirements
- **OS**: Windows 10 (1809) or later, 64-bit
- **RAM**: 4GB minimum, 8GB recommended
- **Storage**: 500MB for application, additional space for data
- **Network**: Not required (standalone application)

#### Runtime Dependencies
- **Self-contained**: All .NET runtime components included
- **No Prerequisites**: No additional software installation required
- **Portable**: Can run from any directory or removable media

---

## Security

### Data Protection

#### Database Security
- **File Permissions**: SQLite database protected by OS file permissions
- **Connection Security**: Local file access only, no network exposure
- **Backup Encryption**: Recommend encrypting backup files

#### Photo Upload Security
```csharp
public async Task<IActionResult> UploadPhoto(IFormFile photo)
{
    // Validate file type
    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
    var extension = Path.GetExtension(photo.FileName).ToLowerInvariant();
    if (!allowedExtensions.Contains(extension))
        return BadRequest("Invalid file type");

    // Validate file size (5MB limit)
    if (photo.Length > 5 * 1024 * 1024)
        return BadRequest("File too large");

    // Generate secure filename
    var fileName = Guid.NewGuid() + extension;
    var filePath = Path.Combine(_uploadPath, fileName);

    // Save file securely
    using var stream = new FileStream(filePath, FileMode.Create);
    await photo.CopyToAsync(stream);

    return Ok(new { fileName });
}
```

#### Input Validation
- **Model Validation**: Data annotations on all models
- **XSS Prevention**: Automatic HTML encoding in Razor views
- **SQL Injection Prevention**: Entity Framework parameterized queries
- **File Upload Validation**: Type and size restrictions

### Access Control

#### Authentication (Future Enhancement)
```csharp
// Placeholder for future authentication implementation
public class AuthenticationService
{
    // User role management (Teacher, Admin, Headteacher)
    // Session management
    // Password policies
    // Activity logging
}
```

#### Authorization Levels
- **Public Access**: Current implementation (no authentication)
- **Teacher Level**: Grade entry, student management
- **Admin Level**: System configuration, user management
- **Headteacher Level**: All functions, system reports

---

## Performance

### Database Optimization

#### Query Performance
```csharp
// Efficient student loading with related data
public async Task<List<Student>> GetStudentsWithClassesAsync()
{
    return await _context.Students
        .Include(s => s.Class)
        .OrderBy(s => s.FullName)
        .ToListAsync();
}

// Optimized grade calculations
public async Task<List<StudentGradeSummary>> GetClassPerformanceAsync(int classId)
{
    return await _context.Students
        .Where(s => s.ClassId == classId)
        .Select(s => new StudentGradeSummary
        {
            StudentName = s.FullName,
            AverageGrade = s.Marks
                .GroupBy(m => m.SubjectId)
                .Select(g => CalculateFinalGrade(g.ToList()))
                .Average()
        })
        .ToListAsync();
}
```

#### Caching Strategy
```csharp
// Memory caching for frequently accessed data
public class CachedDataService
{
    private readonly IMemoryCache _cache;
    
    public async Task<List<Class>> GetClassesAsync()
    {
        return await _cache.GetOrCreateAsync("classes", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
            return await _context.Classes.ToListAsync();
        });
    }
}
```

### Memory Management

#### Large Dataset Handling
```csharp
// Streaming large reports to avoid memory issues
public async Task<IActionResult> ExportLargeDataset()
{
    var stream = new MemoryStream();
    using (var workbook = new XLWorkbook())
    {
        var worksheet = workbook.Worksheets.Add("Students");
        
        // Process data in chunks
        var students = _context.Students.AsAsyncEnumerable();
        var row = 2;
        
        await foreach (var student in students)
        {
            worksheet.Cell(row, 1).Value = student.FullName;
            worksheet.Cell(row, 2).Value = student.RegistrationNumber;
            row++;
        }
        
        workbook.SaveAs(stream);
    }
    
    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
}
```

### Frontend Performance

#### Asset Optimization
- **Bundling**: CSS/JS file bundling for reduced HTTP requests
- **Minification**: Compressed CSS/JS files in production
- **Image Optimization**: Automatic image resizing for student photos
- **Caching**: Browser caching headers for static assets

#### Responsive Design
```css
/* Optimized responsive grid for student lists */
@media (max-width: 768px) {
    .student-card {
        flex-direction: column;
        margin-bottom: 1rem;
    }
    
    .student-photo {
        width: 100%;
        max-width: 150px;
        margin: 0 auto 1rem;
    }
}
```

---

## Maintenance

### Database Maintenance

#### Regular Tasks
```sql
-- Analyze database for optimization
ANALYZE;

-- Vacuum database to reclaim space
VACUUM;

-- Update statistics for query optimization
UPDATE sqlite_stat1 SET stat = NULL;
ANALYZE;
```

#### Data Archiving
```csharp
public class DataArchiveService
{
    public async Task ArchiveOldAcademicYear(int year)
    {
        // Export old data to archive database
        var oldMarks = await _context.Marks
            .Where(m => m.Year < year)
            .ToListAsync();
            
        // Save to archive database
        using var archiveContext = new ArchiveContext($"FatimaSchool_Archive_{year}.db");
        archiveContext.ArchivedMarks.AddRange(oldMarks.Select(m => new ArchivedMark(m)));
        await archiveContext.SaveChangesAsync();
        
        // Remove from main database
        _context.Marks.RemoveRange(oldMarks);
        await _context.SaveChangesAsync();
    }
}
```

### System Monitoring

#### Health Checks
```csharp
public class SystemHealthService
{
    public async Task<HealthReport> CheckSystemHealthAsync()
    {
        var health = new HealthReport();
        
        // Database connectivity
        try
        {
            await _context.Database.ExecuteSqlRawAsync("SELECT 1");
            health.DatabaseStatus = "Healthy";
        }
        catch (Exception ex)
        {
            health.DatabaseStatus = $"Error: {ex.Message}";
        }
        
        // Disk space
        var driveInfo = new DriveInfo(Path.GetPathRoot(Environment.CurrentDirectory));
        health.AvailableDiskSpace = driveInfo.AvailableFreeSpace;
        
        // Memory usage
        health.MemoryUsage = GC.GetTotalMemory(false);
        
        return health;
    }
}
```

#### Logging Configuration
```csharp
// Structured logging setup
builder.Services.AddSerilog(config =>
{
    config.WriteTo.File("logs/fatima-school-.log", 
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}");
});
```

### Backup Strategy

#### Automated Backup Script
```powershell
# PowerShell backup script
$BackupPath = "C:\FatimaSchool\Backups\$(Get-Date -Format 'yyyy-MM-dd_HH-mm-ss')"
New-Item -ItemType Directory -Path $BackupPath -Force

# Backup database
Copy-Item "FatimaSchool.db" "$BackupPath\FatimaSchool.db"

# Backup student photos
Copy-Item "wwwroot\uploads" "$BackupPath\uploads" -Recurse -Force

# Backup configuration
Copy-Item "appsettings*.json" $BackupPath

# Compress backup
Compress-Archive -Path $BackupPath -DestinationPath "$BackupPath.zip"
Remove-Item $BackupPath -Recurse -Force

Write-Host "Backup completed: $BackupPath.zip"
```

---

## Development

### Development Environment Setup

#### Prerequisites
1. **Visual Studio 2022** (Community or higher)
2. **.NET 8 SDK** (8.0.x or later)
3. **Git** for version control
4. **SQLite Browser** (optional, for database inspection)

#### Project Setup
```bash
# Clone repository
git clone <repository-url>
cd fatima-school-management

# Restore packages
dotnet restore

# Create development database
dotnet ef database update

# Run application
dotnet run
```

### Code Standards

#### Naming Conventions
```csharp
// Models: PascalCase
public class Student
{
    public int Id { get; set; }
    public string FullName { get; set; }
}

// Controllers: PascalCase with Controller suffix
public class StudentsController : Controller

// Views: PascalCase matching action names
// Views/Students/Index.cshtml

// Database columns: PascalCase
CREATE TABLE Students (
    Id INTEGER PRIMARY KEY,
    FullName TEXT NOT NULL
);
```

#### Error Handling
```csharp
public async Task<IActionResult> CreateStudent(Student student)
{
    try
    {
        if (!ModelState.IsValid)
            return View(student);
            
        _context.Students.Add(student);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Student created: {StudentName} ({Id})", 
            student.FullName, student.Id);
            
        return RedirectToAction(nameof(Index));
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error creating student: {StudentName}", student.FullName);
        ModelState.AddModelError("", "An error occurred while saving the student.");
        return View(student);
    }
}
```

### Testing Strategy

#### Unit Testing
```csharp
[TestClass]
public class GradeCalculatorTests
{
    [TestMethod]
    public void CalculateFinalGrade_ValidScores_ReturnsCorrectGrade()
    {
        // Arrange
        var calculator = new GradeCalculatorService();
        var botScore = 80; // 10% weight
        var motScore = 70; // 20% weight  
        var eotScore = 90; // 70% weight
        
        // Act
        var finalScore = calculator.CalculateFinalScore(botScore, motScore, eotScore);
        
        // Assert
        Assert.AreEqual(87, finalScore); // (80*0.1) + (70*0.2) + (90*0.7) = 87
    }
}
```

#### Integration Testing
```csharp
[TestClass]
public class StudentsControllerTests : TestBase
{
    [TestMethod]
    public async Task Index_ReturnsViewWithStudents()
    {
        // Arrange
        var controller = new StudentsController(Context);
        
        // Act
        var result = await controller.Index() as ViewResult;
        
        // Assert
        Assert.IsNotNull(result);
        var model = result.Model as List<Student>;
        Assert.IsTrue(model.Count > 0);
    }
}
```

### Version Control

#### Git Workflow
```bash
# Feature development
git checkout -b feature/new-reporting
git add .
git commit -m "Add class performance reporting"
git push origin feature/new-reporting

# Code review and merge
git checkout main
git pull origin main
git merge feature/new-reporting
git push origin main
```

#### Commit Message Format
```
type(scope): description

Examples:
feat(reports): add student report card generation
fix(grades): correct CBC calculation error
docs(readme): update installation instructions
style(ui): improve responsive design for mobile
refactor(database): optimize student queries
test(grades): add unit tests for grade calculator
```

### Deployment Pipeline

#### Build Script
```bash
#!/bin/bash
# Build and package application

# Clean previous builds
rm -rf ./Deployment
rm -rf ./bin/Release

# Build application
dotnet publish -c Release -r win-x64 --self-contained true \
  -p:PublishSingleFile=false -p:PublishTrimmed=false \
  -o "./Deployment"

# Copy additional files
cp FatimaSchool_Template.db ./Deployment/
cp SmartStart.* ./Deployment/
cp README.md ./Deployment/

# Create documentation
cp -r Documentation/ ./Deployment/Documentation/

echo "Build completed successfully!"
```

---

## Troubleshooting Guide

### Common Development Issues

#### Entity Framework Issues
```csharp
// Database connection problems
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    if (!optionsBuilder.IsConfigured)
    {
        optionsBuilder.UseSqlite("Data Source=FatimaSchool.db",
            options => options.CommandTimeout(30));
    }
}

// Migration issues
dotnet ef migrations add InitialCreate
dotnet ef database update
```

#### Performance Issues
```csharp
// N+1 query problem solution
public async Task<IActionResult> Index()
{
    // Bad: N+1 queries
    var students = await _context.Students.ToListAsync();
    // Each student.Class access triggers a new query
    
    // Good: Single query with Include
    var students = await _context.Students
        .Include(s => s.Class)
        .ToListAsync();
        
    return View(students);
}
```

### Production Deployment Issues

#### Port Conflicts
- **Use Smart Launchers**: SmartStart.ps1 automatically finds available ports
- **Manual Port Override**: Set ASPNETCORE_URLS environment variable
- **Check Running Processes**: `netstat -an | findstr :5000`

#### Database Corruption
```sql
-- Check database integrity
PRAGMA integrity_check;

-- Rebuild database if corrupted
.backup main backup.db
.restore main backup.db
```

#### Memory Issues
- **Monitor Usage**: Task Manager → Performance → Memory
- **Increase Virtual Memory**: System Properties → Advanced → Performance → Settings
- **Close Unnecessary Applications**: Free up system resources

---

*© 2024 Our Lady of Fatima Secondary School - Technical Documentation v2.0*
*For technical support, contact the system administrator or development team.*