using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartExamSystem.Models
{
    [Table("Exam_Students")]
    public class ExamStudent
    {
        [Key]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Student name is required")]
        [StringLength(100, ErrorMessage = "Student name cannot exceed 100 characters")]
        public string StudentName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string Email { get; set; }

        [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 characters")]
        public string PhoneNumber { get; set; }

        [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "Password cannot exceed 100 characters")]
        public string Password { get; set; } = "123";

        // ✅ STEP 19: Added for Student Blocking/Unblocking
        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = SmartExamSystem.Helpers.TimeHelper.GetLocalTime();

        // Navigation Property
        public ICollection<ExamAttempt>? ExamAttempts { get; set; } = new List<ExamAttempt>();
    }
}
