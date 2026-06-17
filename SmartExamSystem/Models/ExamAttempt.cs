using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartExamSystem.Models
{
    [Table("Exam_Attempts")]
    public class ExamAttempt
    {
        [Key]
        public int AttemptId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int ExamId { get; set; }

        // ✅ STEP 23: Server-based timer
        [Required]
        public DateTime StartTime { get; set; } = DateTime.Now;

        public DateTime? EndTime { get; set; }

        [Range(0, 1000, ErrorMessage = "Score cannot be negative")]
        public int Score { get; set; } = 0;

        [StringLength(50)]
        [RegularExpression("^(Started|Submitted|Terminated)$",
            ErrorMessage = "Status must be Started, Submitted, or Terminated")]
        public string Status { get; set; } = "Started";

        // ✅ STEP 18: Warning count for proctoring
        [Range(0, 3, ErrorMessage = "Warning count must be between 0 and 3")]
        public int WarningCount { get; set; } = 0;

        // ✅ STEP 18: Termination reason
        [StringLength(500, ErrorMessage = "Termination reason cannot exceed 500 characters")]
        public string TerminateReason { get; set; }

        // Calculated property: Duration in minutes
        [NotMapped]
        public int DurationMinutes
        {
            get
            {
                if (EndTime.HasValue)
                {
                    return (int)(EndTime.Value - StartTime).TotalMinutes;
                }
                else
                {
                    return (int)(DateTime.Now - StartTime).TotalMinutes;
                }
            }
        }

        // Foreign Keys
        [ForeignKey("StudentId")]
        public ExamStudent? Student { get; set; }

        [ForeignKey("ExamId")]
        public ExamMaster? Exam { get; set; }

        // Navigation Properties
        public ICollection<ExamStudentAnswer>? StudentAnswers { get; set; } = new List<ExamStudentAnswer>();
        public ICollection<ExamProctoringLog>? ProctoringLogs { get; set; } = new List<ExamProctoringLog>();
    }
}
