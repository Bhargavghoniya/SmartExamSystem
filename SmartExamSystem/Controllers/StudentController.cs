using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartExamSystem.Data;
using SmartExamSystem.Models;

namespace SmartExamSystem.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ UPDATED: Student Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var student = _context.ExamStudents
                .FirstOrDefault(s => s.Email == email);

            if (student == null)
            {
                ViewBag.Error = "Student not found. Please contact admin.";
                return View();
            }

            if (student.Password != password)
            {
                ViewBag.Error = "Incorrect password. Please try again.";
                return View();
            }

            if (!student.IsActive)
            {
                ViewBag.Error = "Your account has been blocked. Please contact the administrator.";
                return View();
            }

            // Set session
            HttpContext.Session.SetInt32("StudentId", student.StudentId);
            HttpContext.Session.SetString("StudentName", student.StudentName);
            HttpContext.Session.SetString("StudentEmail", student.Email);

            return RedirectToAction("AvailableExams");
        }

        // ✅ UPDATED: Available Exams - shows only active exams
        public IActionResult AvailableExams()
        {
            int? studentId = HttpContext.Session.GetInt32("StudentId");

            if (studentId == null)
            {
                return RedirectToAction("Login");
            }

            var student = _context.ExamStudents
                .FirstOrDefault(s => s.StudentId == studentId.Value);

            if (student == null || !student.IsActive)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login");
            }

            // Auto-terminate any expired "Started" attempts
            var expiredAttempts = _context.ExamAttempts
                .Include(a => a.Exam)
                .Where(a => a.StudentId == studentId.Value && a.Status == "Started")
                .ToList()
                .Where(a => a.Exam != null && DateTime.Now > a.StartTime.AddMinutes(a.Exam.DurationMinutes))
                .ToList();

            foreach (var expired in expiredAttempts)
            {
                // Calculate score from any saved answers
                var savedAnswers = _context.ExamStudentAnswers
                    .Where(sa => sa.AttemptId == expired.AttemptId && sa.IsCorrect)
                    .Join(_context.ExamQuestions, sa => sa.QuestionId, q => q.QuestionId, (sa, q) => q.Marks)
                    .Sum();

                expired.Score = savedAnswers;
                expired.EndTime = expired.StartTime.AddMinutes(expired.Exam.DurationMinutes);
                expired.Status = "Submitted";
            }

            if (expiredAttempts.Any())
            {
                _context.SaveChanges();
            }

            var exams = _context.ExamMasters
                .Where(e => e.IsActive == true)
                .OrderByDescending(e => e.CreatedDate)
                .ToList();

            var attemptedExams = _context.ExamAttempts
                .Where(a => a.StudentId == studentId.Value)
                .Select(a => new
                {
                    a.AttemptId,
                    a.ExamId,
                    a.Status,
                    a.Score
                })
                .ToList();

            ViewBag.AttemptedExams = attemptedExams;
            ViewBag.StudentName = student.StudentName;

            ViewBag.ExamQuestionCounts = _context.ExamQuestions
                .GroupBy(q => q.ExamId)
                .ToDictionary(g => g.Key, g => g.Count());

            return View(exams);
        }


        // ✅ NEW: Student Dashboard
        public IActionResult Dashboard()
        {
            int? studentId = HttpContext.Session.GetInt32("StudentId");

            if (studentId == null)
            {
                return RedirectToAction("Login");
            }

            var student = _context.ExamStudents
                .Include(s => s.ExamAttempts)
                .FirstOrDefault(s => s.StudentId == studentId.Value);

            if (student == null || !student.IsActive)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login");
            }

            // Get attempt summary
            var attempts = _context.ExamAttempts
                .Include(a => a.Exam)
                .Where(a => a.StudentId == studentId.Value)
                .OrderByDescending(a => a.StartTime)
                .ToList();

            ViewBag.StudentName = student.StudentName;
            ViewBag.TotalAttempts = attempts.Count;
            ViewBag.CompletedAttempts = attempts.Count(a => a.Status == "Submitted");
            ViewBag.TerminatedAttempts = attempts.Count(a => a.Status == "Terminated");

            return View(attempts);
        }

        // ✅ NEW: View single exam result
        public IActionResult ExamResult(int attemptId)
        {
            int? studentId = HttpContext.Session.GetInt32("StudentId");

            if (studentId == null)
            {
                return RedirectToAction("Login");
            }

            var attempt = _context.ExamAttempts
                .Include(a => a.Exam)
                .Include(a => a.StudentAnswers)
                .FirstOrDefault(a => a.AttemptId == attemptId && a.StudentId == studentId.Value);

            if (attempt == null)
            {
                return NotFound();
            }

            ViewBag.StudentName = HttpContext.Session.GetString("StudentName");
            return View(attempt);
        }

        // ✅ NEW: Student Profile
        public IActionResult Profile()
        {
            int? studentId = HttpContext.Session.GetInt32("StudentId");

            if (studentId == null)
            {
                return RedirectToAction("Login");
            }

            var student = _context.ExamStudents
                .FirstOrDefault(s => s.StudentId == studentId.Value);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // ✅ NEW: Edit Profile (GET)
        [HttpGet]
        public IActionResult EditProfile()
        {
            int? studentId = HttpContext.Session.GetInt32("StudentId");

            if (studentId == null)
            {
                return RedirectToAction("Login");
            }

            var student = _context.ExamStudents
                .FirstOrDefault(s => s.StudentId == studentId.Value);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // ✅ NEW: Edit Profile (POST)
        [HttpPost]
        public IActionResult EditProfile(ExamStudent model)
        {
            int? studentId = HttpContext.Session.GetInt32("StudentId");

            if (studentId == null)
            {
                return RedirectToAction("Login");
            }

            var student = _context.ExamStudents
                .FirstOrDefault(s => s.StudentId == studentId.Value);

            if (student == null)
            {
                return NotFound();
            }

            // ✅ UPDATED: Update model properties correctly
            student.StudentName = model.StudentName;
            student.Email = model.Email;
            student.PhoneNumber = model.PhoneNumber;
            student.Address = model.Address;

            _context.ExamStudents.Update(student);
            _context.SaveChanges();

            // Update session
            HttpContext.Session.SetString("StudentName", student.StudentName);
            HttpContext.Session.SetString("StudentEmail", student.Email);

            TempData["Success"] = "Profile updated successfully!";
            return RedirectToAction("Profile");
        }

        // ✅ UPDATED: Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
