using Microsoft.EntityFrameworkCore;
using FatimaSchoolManagement.Models;

namespace FatimaSchoolManagement.Data
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options)
        {
        }

        // DbSets for our entities
        public DbSet<Student> Students { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Mark> Marks { get; set; }
        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Student entity
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.StudentId);
                entity.HasIndex(e => e.StudentNumber).IsUnique();
                entity.Property(e => e.StudentNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LINNumber).HasMaxLength(50);
                entity.Property(e => e.ParentsContact).HasMaxLength(15);
                entity.Property(e => e.NextOfKin).HasMaxLength(100);
                entity.Property(e => e.NextOfKinContact).HasMaxLength(15);
                entity.Property(e => e.PhotoPath).HasMaxLength(255);

                // Relationships
                entity.HasOne(s => s.Class)
                      .WithMany(c => c.Students)
                      .HasForeignKey(s => s.ClassId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(s => s.Marks)
                      .WithOne(m => m.Student)
                      .HasForeignKey(m => m.StudentId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Class entity
            modelBuilder.Entity<Class>(entity =>
            {
                entity.HasKey(e => e.ClassId);
                entity.Property(e => e.ClassName).IsRequired().HasMaxLength(10);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.HasIndex(e => new { e.ClassName, e.AcademicYear }).IsUnique();
            });

            // Configure Subject entity
            modelBuilder.Entity<Subject>(entity =>
            {
                entity.HasKey(e => e.SubjectId);
                entity.HasIndex(e => e.SubjectCode).IsUnique();
                entity.Property(e => e.SubjectCode).IsRequired().HasMaxLength(10);
                entity.Property(e => e.SubjectName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
            });

            // Configure Mark entity
            modelBuilder.Entity<Mark>(entity =>
            {
                entity.HasKey(e => e.MarkId);

                // Configure decimal precision for marks
                entity.Property(e => e.BOTMark)
                      .HasColumnType("decimal(5,2)");
                entity.Property(e => e.MOTMark)
                      .HasColumnType("decimal(5,2)");
                entity.Property(e => e.EOTMark)
                      .HasColumnType("decimal(5,2)");

                // Unique constraint to prevent duplicate marks for same student/subject/term
                entity.HasIndex(e => new { e.StudentId, e.SubjectId, e.AcademicYear, e.Term })
                      .IsUnique();

                // Relationships
                entity.HasOne(m => m.Student)
                      .WithMany(s => s.Marks)
                      .HasForeignKey(m => m.StudentId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(m => m.Subject)
                      .WithMany(s => s.Marks)
                      .HasForeignKey(m => m.SubjectId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(m => m.Class)
                      .WithMany(c => c.Marks)
                      .HasForeignKey(m => m.ClassId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Report entity
            modelBuilder.Entity<Report>(entity =>
            {
                entity.HasKey(e => e.ReportId);

                // Configure decimal precision
                entity.Property(e => e.TotalMarks)
                      .HasColumnType("decimal(8,2)");
                entity.Property(e => e.AverageMark)
                      .HasColumnType("decimal(5,2)");

                entity.Property(e => e.OverallGrade).HasMaxLength(2);
                entity.Property(e => e.TeacherComments).HasMaxLength(1000);
                entity.Property(e => e.HeadTeacherComments).HasMaxLength(1000);

                // Unique constraint for student/term/year reports
                entity.HasIndex(e => new { e.StudentId, e.AcademicYear, e.Term })
                      .IsUnique();

                // Relationships
                entity.HasOne(r => r.Student)
                      .WithMany()
                      .HasForeignKey(r => r.StudentId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(r => r.Class)
                      .WithMany()
                      .HasForeignKey(r => r.ClassId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Seed initial data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Classes
            var classes = new[]
            {
                new Class { ClassId = 1, ClassName = "S1", Level = EducationLevel.OLevel, AcademicYear = 2024, Description = "Senior 1" },
                new Class { ClassId = 2, ClassName = "S2", Level = EducationLevel.OLevel, AcademicYear = 2024, Description = "Senior 2" },
                new Class { ClassId = 3, ClassName = "S3", Level = EducationLevel.OLevel, AcademicYear = 2024, Description = "Senior 3" },
                new Class { ClassId = 4, ClassName = "S4", Level = EducationLevel.OLevel, AcademicYear = 2024, Description = "Senior 4" },
                new Class { ClassId = 5, ClassName = "S5", Level = EducationLevel.ALevel, AcademicYear = 2024, Description = "Senior 5" },
                new Class { ClassId = 6, ClassName = "S6", Level = EducationLevel.ALevel, AcademicYear = 2024, Description = "Senior 6" }
            };
            modelBuilder.Entity<Class>().HasData(classes);

            // Seed Subjects for O-Level
            var oLevelSubjects = new[]
            {
                new Subject { SubjectId = 1, SubjectCode = "ENG", SubjectName = "English Language", Level = EducationLevel.OLevel },
                new Subject { SubjectId = 2, SubjectCode = "MATH", SubjectName = "Mathematics", Level = EducationLevel.OLevel },
                new Subject { SubjectId = 3, SubjectCode = "PHYS", SubjectName = "Physics", Level = EducationLevel.OLevel },
                new Subject { SubjectId = 4, SubjectCode = "CHEM", SubjectName = "Chemistry", Level = EducationLevel.OLevel },
                new Subject { SubjectId = 5, SubjectCode = "BIO", SubjectName = "Biology", Level = EducationLevel.OLevel },
                new Subject { SubjectId = 6, SubjectCode = "HIST", SubjectName = "History", Level = EducationLevel.OLevel },
                new Subject { SubjectId = 7, SubjectCode = "GEO", SubjectName = "Geography", Level = EducationLevel.OLevel },
                new Subject { SubjectId = 8, SubjectCode = "LIT", SubjectName = "Literature", Level = EducationLevel.OLevel },
                new Subject { SubjectId = 9, SubjectCode = "REL", SubjectName = "Religious Education", Level = EducationLevel.OLevel },
                new Subject { SubjectId = 10, SubjectCode = "LUG", SubjectName = "Luganda", Level = EducationLevel.OLevel }
            };

            // Seed Subjects for A-Level
            var aLevelSubjects = new[]
            {
                new Subject { SubjectId = 11, SubjectCode = "A-ENG", SubjectName = "Advanced English", Level = EducationLevel.ALevel },
                new Subject { SubjectId = 12, SubjectCode = "A-MATH", SubjectName = "Advanced Mathematics", Level = EducationLevel.ALevel },
                new Subject { SubjectId = 13, SubjectCode = "A-PHYS", SubjectName = "Advanced Physics", Level = EducationLevel.ALevel },
                new Subject { SubjectId = 14, SubjectCode = "A-CHEM", SubjectName = "Advanced Chemistry", Level = EducationLevel.ALevel },
                new Subject { SubjectId = 15, SubjectCode = "A-BIO", SubjectName = "Advanced Biology", Level = EducationLevel.ALevel },
                new Subject { SubjectId = 16, SubjectCode = "A-HIST", SubjectName = "Advanced History", Level = EducationLevel.ALevel },
                new Subject { SubjectId = 17, SubjectCode = "A-GEO", SubjectName = "Advanced Geography", Level = EducationLevel.ALevel },
                new Subject { SubjectId = 18, SubjectCode = "A-ECO", SubjectName = "Economics", Level = EducationLevel.ALevel }
            };

            var allSubjects = oLevelSubjects.Concat(aLevelSubjects).ToArray();
            modelBuilder.Entity<Subject>().HasData(allSubjects);
        }
    }
}