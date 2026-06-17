using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartExamSystem.Models
{
    [Table("Exam_Proctoring_Logs")]
    public class ExamProctoringLog
    {
        [Key]
        public int LogId { get; set; }

        [Required]
        public int AttemptId { get; set; }

        // ✅ STEP 18: Activity type logged
        [Required]
        [StringLength(50, ErrorMessage = "Activity type cannot exceed 50 characters")]
        public string ActivityType { get; set; }

        // Supported Activity Types:
        // - TAB_CHANGED: Student switched tabs
        // - RIGHT_CLICK: Right-click attempt
        // - CTRL_C: Copy attempt
        // - CTRL_V: Paste attempt
        // - CTRL_U: View source attempt
        // - F12: Developer tools attempt
        // - ESCAPE_KEY: Escape key pressed
        // - FULLSCREEN_EXIT: Exited fullscreen
        // - COPY: Copy event triggered
        // - PASTE: Paste event triggered

        [Required]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        [Required]
        public DateTime LogTime { get; set; } = DateTime.Now;

        // Foreign Key
        [ForeignKey("AttemptId")]
        public ExamAttempt? Attempt { get; set; }
    }
}
