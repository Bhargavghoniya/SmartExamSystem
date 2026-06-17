using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartExamSystem.Models
{
    [Table("Exam_Student_Answers")]
    public class ExamStudentAnswer
    {
        [Key]
        public int AnswerId { get; set; }

        [Required]
        public int AttemptId { get; set; }

        [Required]
        public int QuestionId { get; set; }

        // ✅ STEP 24: Auto-saved answer
        [StringLength(1)]
        [RegularExpression("[A-D]?", ErrorMessage = "Selected option must be A, B, C, D, or null")]
        public string SelectedOption { get; set; }

        [Required]
        public bool IsCorrect { get; set; } = false;

        public DateTime AnsweredTime { get; set; } = DateTime.Now;

        // Foreign Keys
        [ForeignKey("AttemptId")]
        public ExamAttempt? Attempt { get; set; }

        [ForeignKey("QuestionId")]
        public ExamQuestion? Question { get; set; }
    }
}
