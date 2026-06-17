using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartExamSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordToStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exam_Attempts_Exam_Exams_ExamId",
                table: "Exam_Attempts");

            migrationBuilder.DropForeignKey(
                name: "FK_Exam_ProctoringLogs_Exam_Attempts_AttemptId",
                table: "Exam_ProctoringLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Exam_Questions_Exam_Exams_ExamId",
                table: "Exam_Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Exam_StudentAnswers_Exam_Attempts_AttemptId",
                table: "Exam_StudentAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_Exam_StudentAnswers_Exam_Questions_QuestionId",
                table: "Exam_StudentAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Exam_StudentAnswers",
                table: "Exam_StudentAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Exam_ProctoringLogs",
                table: "Exam_ProctoringLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Exam_Exams",
                table: "Exam_Exams");

            migrationBuilder.RenameTable(
                name: "Exam_StudentAnswers",
                newName: "Exam_Student_Answers");

            migrationBuilder.RenameTable(
                name: "Exam_ProctoringLogs",
                newName: "Exam_Proctoring_Logs");

            migrationBuilder.RenameTable(
                name: "Exam_Exams",
                newName: "Exam_Masters");

            migrationBuilder.RenameColumn(
                name: "IsBlocked",
                table: "Exam_Students",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Exam_Students",
                newName: "StudentName");

            migrationBuilder.RenameIndex(
                name: "IX_Exam_StudentAnswers_QuestionId",
                table: "Exam_Student_Answers",
                newName: "IX_Exam_Student_Answers_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_Exam_StudentAnswers_AttemptId",
                table: "Exam_Student_Answers",
                newName: "IX_Exam_Student_Answers_AttemptId");

            migrationBuilder.RenameIndex(
                name: "IX_Exam_ProctoringLogs_AttemptId",
                table: "Exam_Proctoring_Logs",
                newName: "IX_Exam_Proctoring_Logs_AttemptId");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Exam_Students",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Exam_Students",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Exam_Students",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "QuestionText",
                table: "Exam_Questions",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "OptionD",
                table: "Exam_Questions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "OptionC",
                table: "Exam_Questions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "OptionB",
                table: "Exam_Questions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "OptionA",
                table: "Exam_Questions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CorrectOption",
                table: "Exam_Questions",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Exam_Questions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Exam_Questions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TerminateReason",
                table: "Exam_Attempts",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Exam_Attempts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "SelectedOption",
                table: "Exam_Student_Answers",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AnsweredTime",
                table: "Exam_Student_Answers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ExamAttemptAttemptId",
                table: "Exam_Student_Answers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExamQuestionQuestionId",
                table: "Exam_Student_Answers",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Exam_Proctoring_Logs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ActivityType",
                table: "Exam_Proctoring_Logs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "ExamAttemptAttemptId",
                table: "Exam_Proctoring_Logs",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExamName",
                table: "Exam_Masters",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Exam_Masters",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Exam_Masters",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Instructions",
                table: "Exam_Masters",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Exam_Masters",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Exam_Student_Answers",
                table: "Exam_Student_Answers",
                column: "AnswerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Exam_Proctoring_Logs",
                table: "Exam_Proctoring_Logs",
                column: "LogId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Exam_Masters",
                table: "Exam_Masters",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_Exam_Student_Answers_ExamAttemptAttemptId",
                table: "Exam_Student_Answers",
                column: "ExamAttemptAttemptId");

            migrationBuilder.CreateIndex(
                name: "IX_Exam_Student_Answers_ExamQuestionQuestionId",
                table: "Exam_Student_Answers",
                column: "ExamQuestionQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Exam_Proctoring_Logs_ExamAttemptAttemptId",
                table: "Exam_Proctoring_Logs",
                column: "ExamAttemptAttemptId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exam_Attempts_Exam_Masters_ExamId",
                table: "Exam_Attempts",
                column: "ExamId",
                principalTable: "Exam_Masters",
                principalColumn: "ExamId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exam_Proctoring_Logs_Exam_Attempts_AttemptId",
                table: "Exam_Proctoring_Logs",
                column: "AttemptId",
                principalTable: "Exam_Attempts",
                principalColumn: "AttemptId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exam_Proctoring_Logs_Exam_Attempts_ExamAttemptAttemptId",
                table: "Exam_Proctoring_Logs",
                column: "ExamAttemptAttemptId",
                principalTable: "Exam_Attempts",
                principalColumn: "AttemptId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exam_Questions_Exam_Masters_ExamId",
                table: "Exam_Questions",
                column: "ExamId",
                principalTable: "Exam_Masters",
                principalColumn: "ExamId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exam_Student_Answers_Exam_Attempts_AttemptId",
                table: "Exam_Student_Answers",
                column: "AttemptId",
                principalTable: "Exam_Attempts",
                principalColumn: "AttemptId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exam_Student_Answers_Exam_Attempts_ExamAttemptAttemptId",
                table: "Exam_Student_Answers",
                column: "ExamAttemptAttemptId",
                principalTable: "Exam_Attempts",
                principalColumn: "AttemptId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exam_Student_Answers_Exam_Questions_ExamQuestionQuestionId",
                table: "Exam_Student_Answers",
                column: "ExamQuestionQuestionId",
                principalTable: "Exam_Questions",
                principalColumn: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exam_Student_Answers_Exam_Questions_QuestionId",
                table: "Exam_Student_Answers",
                column: "QuestionId",
                principalTable: "Exam_Questions",
                principalColumn: "QuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exam_Attempts_Exam_Masters_ExamId",
                table: "Exam_Attempts");

            migrationBuilder.DropForeignKey(
                name: "FK_Exam_Proctoring_Logs_Exam_Attempts_AttemptId",
                table: "Exam_Proctoring_Logs");

            migrationBuilder.DropForeignKey(
                name: "FK_Exam_Proctoring_Logs_Exam_Attempts_ExamAttemptAttemptId",
                table: "Exam_Proctoring_Logs");

            migrationBuilder.DropForeignKey(
                name: "FK_Exam_Questions_Exam_Masters_ExamId",
                table: "Exam_Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Exam_Student_Answers_Exam_Attempts_AttemptId",
                table: "Exam_Student_Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Exam_Student_Answers_Exam_Attempts_ExamAttemptAttemptId",
                table: "Exam_Student_Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Exam_Student_Answers_Exam_Questions_ExamQuestionQuestionId",
                table: "Exam_Student_Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Exam_Student_Answers_Exam_Questions_QuestionId",
                table: "Exam_Student_Answers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Exam_Student_Answers",
                table: "Exam_Student_Answers");

            migrationBuilder.DropIndex(
                name: "IX_Exam_Student_Answers_ExamAttemptAttemptId",
                table: "Exam_Student_Answers");

            migrationBuilder.DropIndex(
                name: "IX_Exam_Student_Answers_ExamQuestionQuestionId",
                table: "Exam_Student_Answers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Exam_Proctoring_Logs",
                table: "Exam_Proctoring_Logs");

            migrationBuilder.DropIndex(
                name: "IX_Exam_Proctoring_Logs_ExamAttemptAttemptId",
                table: "Exam_Proctoring_Logs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Exam_Masters",
                table: "Exam_Masters");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Exam_Students");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Exam_Students");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Exam_Students");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Exam_Questions");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Exam_Questions");

            migrationBuilder.DropColumn(
                name: "AnsweredTime",
                table: "Exam_Student_Answers");

            migrationBuilder.DropColumn(
                name: "ExamAttemptAttemptId",
                table: "Exam_Student_Answers");

            migrationBuilder.DropColumn(
                name: "ExamQuestionQuestionId",
                table: "Exam_Student_Answers");

            migrationBuilder.DropColumn(
                name: "ExamAttemptAttemptId",
                table: "Exam_Proctoring_Logs");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Exam_Masters");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Exam_Masters");

            migrationBuilder.DropColumn(
                name: "Instructions",
                table: "Exam_Masters");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Exam_Masters");

            migrationBuilder.RenameTable(
                name: "Exam_Student_Answers",
                newName: "Exam_StudentAnswers");

            migrationBuilder.RenameTable(
                name: "Exam_Proctoring_Logs",
                newName: "Exam_ProctoringLogs");

            migrationBuilder.RenameTable(
                name: "Exam_Masters",
                newName: "Exam_Exams");

            migrationBuilder.RenameColumn(
                name: "StudentName",
                table: "Exam_Students",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Exam_Students",
                newName: "IsBlocked");

            migrationBuilder.RenameIndex(
                name: "IX_Exam_Student_Answers_QuestionId",
                table: "Exam_StudentAnswers",
                newName: "IX_Exam_StudentAnswers_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_Exam_Student_Answers_AttemptId",
                table: "Exam_StudentAnswers",
                newName: "IX_Exam_StudentAnswers_AttemptId");

            migrationBuilder.RenameIndex(
                name: "IX_Exam_Proctoring_Logs_AttemptId",
                table: "Exam_ProctoringLogs",
                newName: "IX_Exam_ProctoringLogs_AttemptId");

            migrationBuilder.AlterColumn<string>(
                name: "QuestionText",
                table: "Exam_Questions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "OptionD",
                table: "Exam_Questions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "OptionC",
                table: "Exam_Questions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "OptionB",
                table: "Exam_Questions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "OptionA",
                table: "Exam_Questions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "CorrectOption",
                table: "Exam_Questions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1)",
                oldMaxLength: 1);

            migrationBuilder.AlterColumn<string>(
                name: "TerminateReason",
                table: "Exam_Attempts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Exam_Attempts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "SelectedOption",
                table: "Exam_StudentAnswers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1)",
                oldMaxLength: 1);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Exam_ProctoringLogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "ActivityType",
                table: "Exam_ProctoringLogs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ExamName",
                table: "Exam_Exams",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Exam_StudentAnswers",
                table: "Exam_StudentAnswers",
                column: "AnswerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Exam_ProctoringLogs",
                table: "Exam_ProctoringLogs",
                column: "LogId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Exam_Exams",
                table: "Exam_Exams",
                column: "ExamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exam_Attempts_Exam_Exams_ExamId",
                table: "Exam_Attempts",
                column: "ExamId",
                principalTable: "Exam_Exams",
                principalColumn: "ExamId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exam_ProctoringLogs_Exam_Attempts_AttemptId",
                table: "Exam_ProctoringLogs",
                column: "AttemptId",
                principalTable: "Exam_Attempts",
                principalColumn: "AttemptId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exam_Questions_Exam_Exams_ExamId",
                table: "Exam_Questions",
                column: "ExamId",
                principalTable: "Exam_Exams",
                principalColumn: "ExamId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exam_StudentAnswers_Exam_Attempts_AttemptId",
                table: "Exam_StudentAnswers",
                column: "AttemptId",
                principalTable: "Exam_Attempts",
                principalColumn: "AttemptId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exam_StudentAnswers_Exam_Questions_QuestionId",
                table: "Exam_StudentAnswers",
                column: "QuestionId",
                principalTable: "Exam_Questions",
                principalColumn: "QuestionId");
        }
    }
}
