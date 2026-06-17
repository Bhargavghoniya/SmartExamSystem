using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartExamSystem.Models
{
    [Table("Exam_Questions")]
    public class ExamQuestion
    {
        [Key]
        public int QuestionId { get; set; }

        [Required(ErrorMessage = "Exam ID is required")]
        public int ExamId { get; set; }

        [Required(ErrorMessage = "Question text is required")]
        [StringLength(2000, ErrorMessage = "Question cannot exceed 2000 characters")]
        public string QuestionText { get; set; }

        [Required(ErrorMessage = "Option A is required")]
        [StringLength(500, ErrorMessage = "Option A cannot exceed 500 characters")]
        public string OptionA { get; set; }

        [Required(ErrorMessage = "Option B is required")]
        [StringLength(500, ErrorMessage = "Option B cannot exceed 500 characters")]
        public string OptionB { get; set; }

        [Required(ErrorMessage = "Option C is required")]
        [StringLength(500, ErrorMessage = "Option C cannot exceed 500 characters")]
        public string OptionC { get; set; }

        [Required(ErrorMessage = "Option D is required")]
        [StringLength(500, ErrorMessage = "Option D cannot exceed 500 characters")]
        public string OptionD { get; set; }

        [Required(ErrorMessage = "Correct option is required")]
        [StringLength(1, ErrorMessage = "Correct option must be A, B, C, or D")]
        [RegularExpression("[A-D]", ErrorMessage = "Correct option must be A, B, C, or D")]
        public string CorrectOption { get; set; }

        // ✅ STEP 21: Marks for each question
        [Required(ErrorMessage = "Marks is required")]
        [Range(1, 100, ErrorMessage = "Marks must be between 1 and 100")]
        public int Marks { get; set; } = 1;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? UpdatedDate { get; set; }

        // Foreign Key
        [ForeignKey("ExamId")]
        public ExamMaster? Exam { get; set; }

        // Navigation Property
        public ICollection<ExamStudentAnswer>? StudentAnswers { get; set; } = new List<ExamStudentAnswer>();
    }
}
