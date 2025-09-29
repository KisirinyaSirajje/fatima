using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FatimaSchoolManagement.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    ClassId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClassName = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Level = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    AcademicYear = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.ClassId);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    SubjectId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SubjectCode = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    SubjectName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Level = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.SubjectId);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StudentNumber = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LINNumber = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    ParentsContact = table.Column<string>(type: "TEXT", maxLength: 15, nullable: true),
                    NextOfKin = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    NextOfKinContact = table.Column<string>(type: "TEXT", maxLength: 15, nullable: true),
                    PhotoPath = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    Level = table.Column<int>(type: "INTEGER", nullable: false),
                    ClassId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.StudentId);
                    table.ForeignKey(
                        name: "FK_Students_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "ClassId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Marks",
                columns: table => new
                {
                    MarkId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StudentId = table.Column<int>(type: "INTEGER", nullable: false),
                    SubjectId = table.Column<int>(type: "INTEGER", nullable: false),
                    ClassId = table.Column<int>(type: "INTEGER", nullable: false),
                    AcademicYear = table.Column<int>(type: "INTEGER", nullable: false),
                    Term = table.Column<int>(type: "INTEGER", nullable: false),
                    BOTMark = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    MOTMark = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    EOTMark = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marks", x => x.MarkId);
                    table.ForeignKey(
                        name: "FK_Marks_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "ClassId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Marks_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Marks_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "SubjectId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    ReportId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StudentId = table.Column<int>(type: "INTEGER", nullable: false),
                    ClassId = table.Column<int>(type: "INTEGER", nullable: false),
                    AcademicYear = table.Column<int>(type: "INTEGER", nullable: false),
                    Term = table.Column<int>(type: "INTEGER", nullable: false),
                    GeneratedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TotalMarks = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    AverageMark = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    OverallGrade = table.Column<string>(type: "TEXT", maxLength: 2, nullable: false),
                    ClassPosition = table.Column<int>(type: "INTEGER", nullable: true),
                    TotalStudents = table.Column<int>(type: "INTEGER", nullable: false),
                    TeacherComments = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    HeadTeacherComments = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    NextTermBegins = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_Reports_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "ClassId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reports_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Classes",
                columns: new[] { "ClassId", "AcademicYear", "ClassName", "Description", "IsActive", "Level" },
                values: new object[,]
                {
                    { 1, 2024, "S1", "Senior 1", true, 1 },
                    { 2, 2024, "S2", "Senior 2", true, 1 },
                    { 3, 2024, "S3", "Senior 3", true, 1 },
                    { 4, 2024, "S4", "Senior 4", true, 1 },
                    { 5, 2024, "S5", "Senior 5", true, 2 },
                    { 6, 2024, "S6", "Senior 6", true, 2 }
                });

            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "SubjectId", "Description", "IsActive", "Level", "SubjectCode", "SubjectName" },
                values: new object[,]
                {
                    { 1, null, true, 1, "ENG", "English Language" },
                    { 2, null, true, 1, "MATH", "Mathematics" },
                    { 3, null, true, 1, "PHYS", "Physics" },
                    { 4, null, true, 1, "CHEM", "Chemistry" },
                    { 5, null, true, 1, "BIO", "Biology" },
                    { 6, null, true, 1, "HIST", "History" },
                    { 7, null, true, 1, "GEO", "Geography" },
                    { 8, null, true, 1, "LIT", "Literature" },
                    { 9, null, true, 1, "REL", "Religious Education" },
                    { 10, null, true, 1, "LUG", "Luganda" },
                    { 11, null, true, 2, "A-ENG", "Advanced English" },
                    { 12, null, true, 2, "A-MATH", "Advanced Mathematics" },
                    { 13, null, true, 2, "A-PHYS", "Advanced Physics" },
                    { 14, null, true, 2, "A-CHEM", "Advanced Chemistry" },
                    { 15, null, true, 2, "A-BIO", "Advanced Biology" },
                    { 16, null, true, 2, "A-HIST", "Advanced History" },
                    { 17, null, true, 2, "A-GEO", "Advanced Geography" },
                    { 18, null, true, 2, "A-ECO", "Economics" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Classes_ClassName_AcademicYear",
                table: "Classes",
                columns: new[] { "ClassName", "AcademicYear" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Marks_ClassId",
                table: "Marks",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Marks_StudentId_SubjectId_AcademicYear_Term",
                table: "Marks",
                columns: new[] { "StudentId", "SubjectId", "AcademicYear", "Term" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Marks_SubjectId",
                table: "Marks",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ClassId",
                table: "Reports",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_StudentId_AcademicYear_Term",
                table: "Reports",
                columns: new[] { "StudentId", "AcademicYear", "Term" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_ClassId",
                table: "Students",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_StudentNumber",
                table: "Students",
                column: "StudentNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_SubjectCode",
                table: "Subjects",
                column: "SubjectCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Marks");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Classes");
        }
    }
}
