using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartExamSystem.Models
{
    [Table("Exam_Masters")]
    public class ExamMaster
    {
        [Key]
        public int ExamId { get; set; }

        [Required(ErrorMessage = "Exam name is required")]
        [StringLength(200, ErrorMessage = "Exam name cannot exceed 200 characters")]
        public string ExamName { get; set; }

        // ✅ STEP 20 & 23: Duration in minutes
        [Required(ErrorMessage = "Duration is required")]
        [Range(5, 480, ErrorMessage = "Duration must be between 5 and 480 minutes")]
        public int DurationMinutes { get; set; }

        // ✅ STEP 20 & 23: Total marks for the exam
        [Required(ErrorMessage = "Total marks is required")]
        [Range(1, 1000, ErrorMessage = "Total marks must be between 1 and 1000")]
        public int TotalMarks { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        [StringLength(100, ErrorMessage = "Subject cannot exceed 100 characters")]
        public string? Subject { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string? Description { get; set; }

        [StringLength(2000, ErrorMessage = "Instructions cannot exceed 2000 characters")]
        public string? Instructions { get; set; }

        // ✅ STEP 20: Activate/Deactivate exams
        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = SmartExamSystem.Helpers.TimeHelper.GetLocalTime();

        public DateTime? UpdatedDate { get; set; }

        // Navigation Properties
        public ICollection<ExamQuestion>? ExamQuestions { get; set; } = new List<ExamQuestion>();
        public ICollection<ExamAttempt>? ExamAttempts { get; set; } = new List<ExamAttempt>();
    }
}
