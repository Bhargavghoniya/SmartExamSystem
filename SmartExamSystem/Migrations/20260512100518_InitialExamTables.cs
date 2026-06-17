using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartExamSystem.Migrations
{
    /// <inheritdoc />
    public partial class InitialExamTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exam_Exams",
                columns: table => new
                {
                    ExamId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExamName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    TotalMarks = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exam_Exams", x => x.ExamId);
                });

            migrationBuilder.CreateTable(
                name: "Exam_Students",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exam_Students", x => x.StudentId);
                });

            migrationBuilder.CreateTable(
                name: "Exam_Questions",
                columns: table => new
                {
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExamId = table.Column<int>(type: "int", nullable: false),
                    QuestionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionB = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorrectOption = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Marks = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exam_Questions", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_Exam_Questions_Exam_Exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exam_Exams",
                        principalColumn: "ExamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Exam_Attempts",
                columns: table => new
                {
                    AttemptId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    ExamId = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Score = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WarningCount = table.Column<int>(type: "int", nullable: false),
                    TerminateReason = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exam_Attempts", x => x.AttemptId);
                    table.ForeignKey(
                        name: "FK_Exam_Attempts_Exam_Exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exam_Exams",
                        principalColumn: "ExamId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Exam_Attempts_Exam_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Exam_Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Exam_ProctoringLogs",
                columns: table => new
                {
                    LogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttemptId = table.Column<int>(type: "int", nullable: false),
                    ActivityType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exam_ProctoringLogs", x => x.LogId);
                    table.ForeignKey(
                        name: "FK_Exam_ProctoringLogs_Exam_Attempts_AttemptId",
                        column: x => x.AttemptId,
                        principalTable: "Exam_Attempts",
                        principalColumn: "AttemptId");
                });

            migrationBuilder.CreateTable(
                name: "Exam_StudentAnswers",
                columns: table => new
                {
                    AnswerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttemptId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    SelectedOption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exam_StudentAnswers", x => x.AnswerId);
                    table.ForeignKey(
                        name: "FK_Exam_StudentAnswers_Exam_Attempts_AttemptId",
                        column: x => x.AttemptId,
                        principalTable: "Exam_Attempts",
                        principalColumn: "AttemptId");
                    table.ForeignKey(
                        name: "FK_Exam_StudentAnswers_Exam_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Exam_Questions",
                        principalColumn: "QuestionId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exam_Attempts_ExamId",
                table: "Exam_Attempts",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_Exam_Attempts_StudentId",
                table: "Exam_Attempts",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Exam_ProctoringLogs_AttemptId",
                table: "Exam_ProctoringLogs",
                column: "AttemptId");

            migrationBuilder.CreateIndex(
                name: "IX_Exam_Questions_ExamId",
                table: "Exam_Questions",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_Exam_StudentAnswers_AttemptId",
                table: "Exam_StudentAnswers",
                column: "AttemptId");

            migrationBuilder.CreateIndex(
                name: "IX_Exam_StudentAnswers_QuestionId",
                table: "Exam_StudentAnswers",
                column: "QuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Exam_ProctoringLogs");

            migrationBuilder.DropTable(
                name: "Exam_StudentAnswers");

            migrationBuilder.DropTable(
                name: "Exam_Attempts");

            migrationBuilder.DropTable(
                name: "Exam_Questions");

            migrationBuilder.DropTable(
                name: "Exam_Students");

            migrationBuilder.DropTable(
                name: "Exam_Exams");
        }
    }
}
