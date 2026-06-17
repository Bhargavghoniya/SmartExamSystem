using Microsoft.EntityFrameworkCore;
using SmartExamSystem.Models;

namespace SmartExamSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ExamStudent> ExamStudents { get; set; }
        public DbSet<ExamMaster> ExamMasters { get; set; }
        public DbSet<ExamQuestion> ExamQuestions { get; set; }
        public DbSet<ExamAttempt> ExamAttempts { get; set; }
        public DbSet<ExamStudentAnswer> ExamStudentAnswers { get; set; }
        public DbSet<ExamProctoringLog> ExamProctoringLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ExamStudentAnswer>()
                .HasOne(a => a.Attempt)
                .WithMany()
                .HasForeignKey(a => a.AttemptId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ExamStudentAnswer>()
                .HasOne(a => a.Question)
                .WithMany()
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ExamProctoringLog>()
                .HasOne(p => p.Attempt)
                .WithMany()
                .HasForeignKey(p => p.AttemptId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}